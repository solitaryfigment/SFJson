using System;
using System.Collections;
using System.Linq;
using System.Reflection;
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
        
        public override object Convert()
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
                    list.Add(_elementConverter.Convert(tokenChild, elementType));
                }
            }

            return list;
        }
    }

    public abstract class CustomConverter
    {
        protected JsonToken _token;
        protected Type _defaultType;

        internal object Convert(JsonToken token, Type defaultType)
        {
            _token = token;
            _defaultType = defaultType;
            return Convert();
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
                    return customConverter.Convert(child, propertyInfo.PropertyType);
                }
                return child.GetValue(propertyInfo.PropertyType);
            }
            
            if(child?.MemberInfo is FieldInfo fieldInfo)
            {
                Console.WriteLine("FieldInfo");
                if(customConverter != null)
                {
                    return customConverter.Convert(child, fieldInfo.FieldType);
                }
                return child.GetValue(fieldInfo.FieldType);
            }

            return child?.GetValue(childType);
        }

        public abstract object Convert();
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