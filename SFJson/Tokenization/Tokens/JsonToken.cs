using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SFJson.Conversion.Settings;
using SFJson.Utils;

namespace SFJson.Tokenization.Tokens
{
    /// <summary>
    /// Base class for all token created during the tokenizaiton phase.
    /// </summary>
    /// <seealso cref="JsonCollection"/>
    /// <seealso cref="JsonDictionary"/>
    /// <seealso cref="JsonObject"/>
    /// <seealso cref="JsonValue"/>
    public abstract class JsonToken
    {
        public string Name;
        public List<JsonToken> Children = new List<JsonToken>();
        internal SettingsManager SettingsManager;
        
        protected Func<Type, object> OnNullValue;

        /// <summary>
        /// Returns the <c>JsonTokenType</c>.
        /// </summary>
        public abstract JsonTokenType JsonTokenType { get; }

        /// <summary>
        /// <see cref="JsonToken.GetValue"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public T GetValue<T>()
        {
            return (T) GetValue(typeof(T));
        }
        
        /// <summary>
        /// Converts the tokenized value to <paramref name="type"/>. 
        /// </summary>
        /// <param name="type"></param>
        /// <returns>
        /// The <paramref name="type"/> this token represents as an <c>object</c>
        /// </returns>
        public abstract object GetValue(Type type);

        protected Type DetermineType(Type type)
        {
            Type determinedType = type;
            if(Children.Count > 0 && Children[0].Name == "$type")
            {
                var typestring = Children[0].GetValue<string>();
                var inheiritedType = Type.GetType(typestring);
                if(inheiritedType != null)
                {
                    determinedType = CheckForBoundTypes(inheiritedType);
                }
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

        protected object CreateInstance(Type type)
        {
            if(type == null)
            {
                return null;
            }

            var elementType = type.GetElementType();
            if(type.IsArray && elementType != null)
            {
                JsonToken list = Children.FirstOrDefault(c => c.Name == "$values");
                list = list ?? this;
                return Array.CreateInstance(elementType, list.Children.Count);
            }

            return Activator.CreateInstance(type);
        }

        private Type DetermineTypeFromInterface(Type type)
        {
            var genericTypes = type.GetGenericArguments();

            if(!type.IsGenericType && type.Implements(typeof(IEnumerable)))
            {
                throw new TypeLoadException("Could not determine underlying type, try using the generic counterpart");
            }

            if(type.IsGenericType && genericTypes.Length == 1 && type.Implements(Type.GetType($"System.Collections.Generic.IEnumerable`1[[{genericTypes[0].AssemblyQualifiedName}]]")))
            {
                var listType = Type.GetType($"System.Collections.Generic.List`1[[{genericTypes[0].AssemblyQualifiedName}]]");
                return listType ?? throw new Exception("List type could not be generated");
            }
            else if(genericTypes.Length == 2 && type.Implements(Type.GetType($"System.Collections.Generic.IDictionary`2[[{genericTypes[0].AssemblyQualifiedName}],[{genericTypes[1].AssemblyQualifiedName}]]")))
            {
                var dictionaryType = Type.GetType($"System.Collections.Generic.Dictionary`2[[{genericTypes[0].AssemblyQualifiedName}],[{genericTypes[1].AssemblyQualifiedName}]]");
                return dictionaryType ?? throw new Exception();
            }
            else if(type.Implements(typeof(IDictionary)))
            {
                return typeof(Dictionary<object, object>);
            }

            return type;
        }

        protected object GetDictionaryValues(Type type, IDictionary obj)
        {
            var keyType = type.GetGenericArguments()[0];
            var valueType = type.GetGenericArguments()[1];
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

                obj.Add(keyValue, child.GetValue(valueType));
            }

            return obj;
        }

        private object ReturnNull(Type type)
        {
            return null;
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

        public string PrettyPrint()
        {
            var stringBuilder = new StringBuilder();
            InternalPrettyPrint(0, stringBuilder, false);
            return stringBuilder.ToString();
        }

        internal virtual void InternalPrettyPrint(int indentLevel, StringBuilder stringBuilder, bool forceIndent = true)
        {
            if(forceIndent)
            {
                stringBuilder.Append('\n');
                PrettyPrintIndent(indentLevel, stringBuilder);
            }
            if(!string.IsNullOrEmpty(Name))
            {
                stringBuilder.Append(Name + " : ");
            }
            
            PrettyPrintControl(false, stringBuilder);
            for(var index = 0; index < Children.Count; index++)
            {
                var token = Children[index];
                if(index > 0)
                {
                    stringBuilder.Append(Constants.COMMA);
                }
                token.InternalPrettyPrint(indentLevel + 1, stringBuilder);
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
                stringBuilder.Append("   ");
            }
        }
    }
}
