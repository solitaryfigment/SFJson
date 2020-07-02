using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Text;
using SFJson.Attributes;
using SFJson.Conversion.Settings;
using SFJson.Tokenization.Tokens;

namespace SFJson.Conversion
{
    public class EnumerableElementConverter<T, TList> : CustomConverter where T : CustomConverter, new() where TList : IList
    {
        protected T _elementConverter;
        
        public EnumerableElementConverter()
        {
            _elementConverter = new T();
        }
        
        public override object Deserialize()
        {
            Console.WriteLine(_token.JsonTokenType);

            IList list;
                
            if(typeof(TList).IsAssignableFrom(_defaultType))
            {
                list = Activator.CreateInstance<TList>();
            }
            else
            {
                list = Activator.CreateInstance(_defaultType) as IList;
            }

            if(list != null)
            {
                var listToken = _token.Children.FirstOrDefault(c => c.Name == "$values") ?? _token;
                foreach(var tokenChild in listToken.Children)
                {
                    if(tokenChild.Name == "$type")
                    {
                        continue;
                    }

                    var elementType = list.GetType().GetElementType() ?? list.GetType().GetGenericArguments()[0];
                    tokenChild.SetupChildrenForType(elementType);
                    list.Add(_elementConverter.Deserialize(tokenChild, elementType));
                }
            }

            return list;
        }

        public override string Serialize(object obj)
        {
            var list = obj as IList;
            if(list == null)
            {
                return null;
            }
            
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            for(int i = 0; i < list.Count; i++)
            {
                sb.Append(_elementConverter.Serialize(list[i]));
                if(i == list.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append("}");
            
            return sb.ToString();
        }
    }

    public abstract class CustomConverter
    {
        protected JsonToken _token;
        protected Type _defaultType;

        internal object Deserialize(JsonToken token, Type defaultType)
        {
            _token = token;
            _defaultType = defaultType;
            return Deserialize();
        }

        protected T GetValueOfChild<T>(Type tokenType, string childName)
        {
            var value = GetValueOfChild(tokenType, typeof(T), childName);
            return value == null ? default : (T)value;
        }

        protected object GetValueOfChild(Type tokenType, Type childType, string childName)
        {
            if(tokenType != null)
            {
                _token.SetupChildrenForType(tokenType);
            }

            var child = _token.Children.FirstOrDefault(c => c.Name == childName);
            Console.WriteLine(child);
            var customConverterAttribute = (CustomConverterAttribute)child?.MemberInfo?.GetCustomAttributes(typeof(CustomConverterAttribute), true).FirstOrDefault();
            var customConverter = customConverterAttribute != null ? (CustomConverter)Activator.CreateInstance(customConverterAttribute.ConverterType) : null;
            
            if(child?.MemberInfo is PropertyInfo propertyInfo)
            {
                Console.WriteLine("PropertyInfo");
                if(customConverter != null)
                {
                    return customConverter.Deserialize(child, propertyInfo.PropertyType);
                }
                return child.GetValue(propertyInfo.PropertyType);
            }
            
            if(child?.MemberInfo is FieldInfo fieldInfo)
            {
                Console.WriteLine("FieldInfo");
                if(customConverter != null)
                {
                    return customConverter.Deserialize(child, fieldInfo.FieldType);
                }
                return child.GetValue(fieldInfo.FieldType);
            }

            return child?.GetValue(childType);
        }

        public abstract object Deserialize();
        public abstract string Serialize(object obj);
    }
    
    /// <summary>
    /// Static wrapper containing methods for serialization and deserialization.
    /// </summary>
    public static class Converter
    {
        private static readonly Serializer _serializer;
        private static readonly Deserializer _deserializer;
        
        static Converter()
        {
            _serializer = new Serializer();
            _deserializer = new Deserializer();
        }
        
        /// <see cref="Serializer.Serialize"/>
        public static string Serialize(object objectToSerialize, SerializerSettings serializerSettings = null)
        {
            return _serializer.Serialize(objectToSerialize, serializerSettings);
        }
        
        /// <see cref="Deserializer.Deserialize"/>
        public static T Deserialize<T>(string stringToDeserialize, DeserializerSettings deserializerSettings = null)
        {
            return _deserializer.Deserialize<T>(stringToDeserialize, deserializerSettings);
        }
        
        /// <see cref="Deserializer.Deserialize"/>
        public static object Deserialize(Type type, string stringToDeserialize, DeserializerSettings deserializerSettings = null)
        {
            return _deserializer.Deserialize(type, stringToDeserialize, deserializerSettings);
        }
        
        /// <see cref="Deserializer.Deserialize"/>
        public static T DeserializeOnToInstance<T>(object instance, string stringToDeserialize, DeserializerSettings deserializerSettings = null)
        {
            return _deserializer.Deserialize<T>(stringToDeserialize, deserializerSettings);
        }
        
        /// <see cref="Deserializer.Deserialize"/>
        public static object DeserializeOnToInstance(Type type, object instance, string stringToDeserialize, DeserializerSettings deserializerSettings = null)
        {
            return _deserializer.Deserialize(type, stringToDeserialize, deserializerSettings);
        }
    }
}