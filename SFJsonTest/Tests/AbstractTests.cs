using System;
using NUnit.Framework;
using SFJson;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Exceptions;
using SFJson.Utils;

namespace SFJsonTest
{
    [TestFixture]
    public class AbstractTests
    {
        private Serializer _serializer;
        private Deserializer _deserializer;

        [OneTimeSetUp]
        public void Init()
        {
            _deserializer = new Deserializer();
            _serializer = new Serializer();
        }
        
        [Test]
        public void CanConvertObjectWithAbstractType()
        {
            var obj = new ObjectWithAbstractClass
            {
                ObjectImplementingAbstractClass = new ObjectImplementingAbstractClass()
                {
                    AbstractPropInt = 50,
                    PropInt = 100
                }
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings { SerializationType = SerializationType.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"ObjectImplementingAbstractClass\":{\"AbstractPropInt\":50,\"PropInt\":100}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithAbstractClass, SFJsonTest\",\"ObjectImplementingAbstractClass\":{\"$type\":\"SFJsonTest.ObjectImplementingAbstractClass, SFJsonTest\",\"AbstractPropInt\":50,\"PropInt\":100}}", strWithType);

            Assert.Throws<DeserializationException>(() =>
            {
                _deserializer.Deserialize<ObjectWithAbstractClass>(str);
            });
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithAbstractClass>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithAbstractClass>(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectImplementingAbstractClass>(strWithTypeDeserialized.ObjectImplementingAbstractClass);
            Assert.AreEqual(((ObjectImplementingAbstractClass)obj.ObjectImplementingAbstractClass).PropInt, ((ObjectImplementingAbstractClass)strWithTypeDeserialized.ObjectImplementingAbstractClass).PropInt);
            Assert.AreEqual(((ObjectImplementingAbstractClass)obj.ObjectImplementingAbstractClass).AbstractPropInt, ((ObjectImplementingAbstractClass)strWithTypeDeserialized.ObjectImplementingAbstractClass).AbstractPropInt);
        }
    }
}