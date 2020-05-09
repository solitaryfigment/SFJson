using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Exceptions;
using SFJson.Utils;
using SFJson.WrapperObjects;

namespace SFJson.Tokenization.Tokens
{
    /// <summary>
    /// Base class for all token created during the tokenization phase.
    /// </summary>
    /// <seealso cref="JsonCollection"/>
    /// <seealso cref="JsonDictionary"/>
    /// <seealso cref="JsonObject"/>
    /// <seealso cref="JsonValue"/>
    public abstract class JsonToken
    {
        public string Name;
        public readonly List<JsonToken> Children = new List<JsonToken>();
        internal SettingsManager SettingsManager;
        internal MemberInfo MemberInfo;
        
        protected Func<Type, object> OnNullValue;

        /// <summary>
        /// Returns the <c>JsonTokenType</c>.
        /// </summary>
        public abstract JsonTokenType JsonTokenType { get; }

        /// <summary>
        /// <see cref="JsonToken.GetValue"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T GetValue<T>(object instance = null)
        {
            return (T) GetValue(typeof(T), instance);
        }

        /// <summary>
        /// Converts the tokenized value to <paramref name="type"/>. 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="instance"></param>
        /// <returns>
        /// The <paramref name="type"/> this token represents as an <c>object</c>
        /// </returns>
        public abstract object GetValue(Type type, object instance = null);

        public virtual void SetupChildrenForType(Type type)
        {
        }
        
        protected Type DetermineType(Type type)
        {
            Type determinedType = type;
            if(Children.Count > 0 && Children[0].Name == "$type")
            {
                var typeName = Children[0].GetValue<string>();
                var inheritedType = Type.GetType(typeName);
                if(inheritedType != null)
                {
                    determinedType = CheckForBoundTypes(inheritedType);
                }
            }

            if(determinedType == null)
            {
                throw new DeserializationException($"Type cannot be null at token {Name}", this);
            }
            determinedType = CheckForBoundTypes(determinedType);

            if(determinedType.IsInterface)
            {
                determinedType = DetermineTypeFromInterface(determinedType);
            }
            return determinedType;
        }

        private Type CheckForBoundTypes(Type type)
        {
            var returnType = SettingsManager.TryGetTypeBinding(type);
            return returnType ?? type;
        }

        protected object CreateInstance(Type type, object instance = null, params object[] args)
        {
            if(type == null)
            {
                return null;
            }

            if(instance != null && type.IsInstanceOfType(instance))
            {
                return instance;
            }

            var elementType = type.GetElementType();
            if(type.IsArray && elementType != null)
            {
                JsonToken list = Children.FirstOrDefault(c => c.Name == "$values");
                list = list ?? this;
                return Array.CreateInstance(elementType, list.Children.Count);
            }
            
            var obj = Activator.CreateInstance(type, args);
            // instance.GetType().InvokeMember()
            // var methods = instance.GetType().GetMethods().Select(m => m.GetCustomAttributes(typeof(SerializeStep), false).Length > 0);
            return obj;
        }
        
        protected bool IsGenericList(object obj, Type type, out IListWrapper listWrapper)
        {
            listWrapper = null;
            foreach(var interfaceType in type.GetInterfaces())
            {
                if(interfaceType.IsGenericType && typeof(IList<>).IsAssignableFrom(interfaceType.GetGenericTypeDefinition()))
                {
                    var genArgs = interfaceType.GenericTypeArguments;
                    listWrapper = Serializer.CreateListWrapper(obj, interfaceType, genArgs[0]);
                    return true;
                }
                else if (typeof(IList).IsAssignableFrom(interfaceType))
                {
                    listWrapper = Serializer.CreateListWrapper(obj);
                    return true;
                }
            }
        
            return false;
        }

        protected bool IsGenericDictionary(object obj, Type type, out IDictionaryWrapper dictionaryWrapper)
        {
            dictionaryWrapper = null;
            foreach(var interfaceType in type.GetInterfaces())
            {
                if(interfaceType.IsGenericType && typeof(IDictionary<,>).IsAssignableFrom(interfaceType.GetGenericTypeDefinition()))
                {
                    var genArgs = interfaceType.GenericTypeArguments;
                    dictionaryWrapper = Serializer.CreateDictionaryWrapper(obj, interfaceType, genArgs[0], genArgs[1]);
                    return true;
                }
                else if (typeof(IDictionary).IsAssignableFrom(interfaceType))
                {
                    dictionaryWrapper = Serializer.CreateDictionaryWrapper(obj);
                    return true;
                }
            }

            return false;
        }
        
        private Type DetermineTypeFromInterface(Type type)
        {
            var genericTypes = type.GetGenericArguments();
            // Type dictionaryType
            // if(IsGenericDictionary(type))
            // {
            //     
            // }
            if(!type.IsGenericType && type.Implements(typeof(IEnumerable)))
            {
                if(type.Implements(typeof(IDictionary)))
                {
                    return typeof(Dictionary<Object, Object>);
                }

                return typeof(List<Object>);
            }

            if(type.IsGenericType && genericTypes.Length == 1 && type.Implements(Type.GetType($"System.Collections.Generic.IEnumerable`1[[{genericTypes[0].AssemblyQualifiedName}]]")))
            {
                var listType = Type.GetType($"System.Collections.Generic.List`1[[{genericTypes[0].AssemblyQualifiedName}]]");
                return listType ?? throw new Exception("List type could not be generated");
            }
            
            if(genericTypes.Length == 2 && type.Implements(Type.GetType($"System.Collections.Generic.IDictionary`2[[{genericTypes[0].AssemblyQualifiedName}],[{genericTypes[1].AssemblyQualifiedName}]]")))
            {
                var dictionaryType = Type.GetType($"System.Collections.Generic.Dictionary`2[[{genericTypes[0].AssemblyQualifiedName}],[{genericTypes[1].AssemblyQualifiedName}]]");
                return dictionaryType ?? throw new TypeLoadException("Could not determine underlying type, try using the generic counterpart");
            }

            if(type.IsInterface)
            {
                throw new TypeLoadException("Could not determine underlying type, try using the generic counterpart");
            }
            return type;
        }

        protected object GetDictionaryValues(IDictionaryWrapper dictionaryWrapper)
        {
            var keyType = dictionaryWrapper.KeyType;
            var valueType = dictionaryWrapper.ValueType;
            foreach(var child in Children)
            {
                if(child.Name == "$type")
                {
                    continue;
                }

                var key = child.Name;
                var token = new Tokenizer().Tokenize(key, SettingsManager);
                token.OnNullValue = ReturnNull;
                var keyValue = token.GetValue(keyType);
                if(keyValue == null)
                {
                    if(SettingsManager.SkipNullKeysInKeyValuedCollections)
                    {
                        continue;
                    }

                    throw new NullReferenceException("Cannot add null key to dictionary.");
                }

                dictionaryWrapper[keyValue] = child.GetValue(valueType);
            }

            return dictionaryWrapper.Dictionary;
        }

        private object ReturnNull(Type type)
        {
            return null;
        }
        
        protected object GetListValues(Type type, IListWrapper obj)
        {
            var list = Children.FirstOrDefault(c => c.Name == "$values");
            var elementType = obj.ElementType;
            list = list ?? this;
            
            try
            {
                for(var i = 0; i < list.Children.Count; i++)
                {
                    if(obj.List.GetType().IsArray)
                    {
                        obj[i] = list.Children[i].GetValue(elementType);
                    }
                    else
                    {
                        obj.Add(list.Children[i].GetValue(elementType));
                    }
                }
        
                return obj.List;
            }
            catch (NotSupportedException)
            {
                if(obj.IsReadOnly)
                {
                    IList thing = Array.CreateInstance(elementType, list.Children.Count);
                
                    for(var i = 0; i < list.Children.Count; i++)
                    {
                        thing[i] = list.Children[i].GetValue(elementType);
                    }
                
                    return CreateInstance(type, null, thing);
                }
                throw;
            }
        }

        protected object GetListValues(Type type, IList obj)
        {
            var list = Children.FirstOrDefault(c => c.Name == "$values");
            var elementType = (type.IsArray) ? type.GetElementType() : type.GetGenericArguments()[0];
            list = list ?? this;
            for(var i = 0; i < list.Children.Count; i++)
            {
                if(type.IsArray)
                {
                    obj[i] = list.Children[i].GetValue(elementType);
                }
                else
                {
                    obj.Add(list.Children[i].GetValue(elementType));
                }
            }

            return obj;
        }

        public string ToStringFormatted()
        {
            var stringBuilder = new StringBuilder();
            InternalToStringFormatted(0, stringBuilder, false);
            return stringBuilder.ToString();
        }

        internal virtual void InternalToStringFormatted(int indentLevel, StringBuilder stringBuilder, bool forceIndent = true)
        {
            if(forceIndent)
            {
                stringBuilder.Append('\n');
                PrettyPrintIndent(indentLevel, stringBuilder);
            }
            if(!string.IsNullOrEmpty(Name))
            {
                stringBuilder.Append($"\"{Name}\" : ");
            }
            
            PrettyPrintControl(false, stringBuilder);
            for(var index = 0; index < Children.Count; index++)
            {
                var token = Children[index];
                if(index > 0)
                {
                    stringBuilder.Append(Constants.COMMA);
                }
                token.InternalToStringFormatted(indentLevel + 1, stringBuilder);
            }

            stringBuilder.Append('\n');
            PrettyPrintIndent(indentLevel, stringBuilder);
            PrettyPrintControl(true, stringBuilder);
        }

        protected void PrettyPrintControl(bool isClose, StringBuilder stringBuilder)
        {
            switch(JsonTokenType)
            {
                case JsonTokenType.Collection:
                    if(isClose)
                    {
                        stringBuilder.Append(Constants.CLOSE_BRACKET);
                    }
                    else
                    {
                        stringBuilder.Append(Constants.OPEN_BRACKET);
                    }
                    break;
                case JsonTokenType.Dictionary:
                case JsonTokenType.Object:
                    if(isClose)
                    {
                        stringBuilder.Append(Constants.CLOSE_CURLY);
                    }
                    else
                    {
                        stringBuilder.Append(Constants.OPEN_CURLY);
                    }
                    break;
            }
        }

        protected void PrettyPrintIndent(int indentLevel, StringBuilder stringBuilder)
        {
            for(var i = 0; i < indentLevel; i++)
            {
                stringBuilder.Append('\t');
            }
        }
    }
}
