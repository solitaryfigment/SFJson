using System;
using System.Collections.Generic;
using NUnit.Framework;
using SFJson;

namespace SFJsonTest
{
    [TestFixture]
    public class GenericTests
    {
        private Serializer _serializer;
        private Deserializer _deserializer;

        [OneTimeSetUp]
        public void Init()
        {
            _serializer = new Serializer();
            _deserializer = new Deserializer();
        }
        
        [Test]
        public void CanConvertWithGenericString()
        {
            var obj = new GenericObject<string>()
            {
                GenericProp = "String"
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"GenericProp\":\"String\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.GenericObject`1[[System.String, mscorlib]], SFJsonTest\",\"GenericProp\":\"String\"}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<GenericObject<string>>(str);
            Assert.NotNull(strDeserialized);
            Assert.AreEqual(obj.GenericProp, strDeserialized.GenericProp);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<GenericObject<string>>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.AreEqual(obj.GenericProp, strWithTypeDeserialized.GenericProp);
        }

        [Test]
        public void CanConvertWithGenericInt()
        {
            var obj = new GenericObject<int>()
            {
                GenericProp = 100
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"GenericProp\":100}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.GenericObject`1[[System.Int32, mscorlib]], SFJsonTest\",\"GenericProp\":100}", strWithType);
            
            
            var strDeserialized = _deserializer.Deserialize<GenericObject<int>>(str);
            Assert.NotNull(strDeserialized);
            Assert.AreEqual(obj.GenericProp, strDeserialized.GenericProp);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<GenericObject<int>>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.AreEqual(obj.GenericProp, strWithTypeDeserialized.GenericProp);
        }
    }
}