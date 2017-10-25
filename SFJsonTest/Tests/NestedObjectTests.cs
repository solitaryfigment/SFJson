using System;
using NUnit.Framework;
using SFJson;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Utils;

namespace SFJsonTest
{
    [TestFixture]
    public class NestedObjectTests
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
        public void CanConvertNestedObjects()
        {
            var obj = new ObjectWithNestedType.NestedClass()
            {
                PropString = "Nested"
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropString\":\"Nested\"}", str);   
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithNestedType+NestedClass, SFJsonTest\",\"PropString\":\"Nested\"}", strWithType);

            var strDeserialized = _deserializer.Deserialize<ObjectWithNestedType.NestedClass>(str);
            
            Assert.NotNull(strDeserialized);
            Assert.AreEqual(obj.PropString, strDeserialized.PropString);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithNestedType.NestedClass>(strWithType);
            
            Assert.NotNull(strWithTypeDeserialized);
            Assert.AreEqual(obj.PropString, strWithTypeDeserialized.PropString);
        }

        [Test]
        public void CanConvertReferenceNestedObjects()
        {
            var obj = new ObjectWithNestedType()
            {
                PropInt = 100,
                PropNested = new ObjectWithNestedType.NestedClass()
                {
                    PropString = "Nested"
                }
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropNested\":{\"PropString\":\"Nested\"},\"PropInt\":100}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithNestedType, SFJsonTest\",\"PropNested\":{\"$type\":\"SFJsonTest.ObjectWithNestedType+NestedClass, SFJsonTest\",\"PropString\":\"Nested\"},\"PropInt\":100}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithNestedType>(str);
            
            Assert.NotNull(strDeserialized);
            Assert.AreEqual(obj.PropInt, strDeserialized.PropInt);
            Assert.NotNull(strDeserialized.PropNested);
            Assert.AreEqual(obj.PropNested.PropString, strDeserialized.PropNested.PropString);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithNestedType>(strWithType);
            
            Assert.NotNull(strWithTypeDeserialized);
            Assert.AreEqual(obj.PropInt, strWithTypeDeserialized.PropInt);
            Assert.NotNull(strWithTypeDeserialized.PropNested);
            Assert.AreEqual(obj.PropNested.PropString, strWithTypeDeserialized.PropNested.PropString);
        }

        [Test]
        public void CanConvertNestedNestedObjects()
        {
            var obj = new ObjectWithNestedNestedType.NestedClass.NestedNestedClass()
            {
                PropString = "Nested"
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropString\":\"Nested\"}", str);   
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithNestedNestedType+NestedClass+NestedNestedClass, SFJsonTest\",\"PropString\":\"Nested\"}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithNestedNestedType.NestedClass.NestedNestedClass>(str);
            
            Assert.NotNull(strDeserialized);
            Assert.AreEqual(obj.PropString, strDeserialized.PropString);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithNestedNestedType.NestedClass.NestedNestedClass>(strWithType);
            
            Assert.NotNull(strWithTypeDeserialized);
            Assert.AreEqual(obj.PropString, strWithTypeDeserialized.PropString);
        }

        [Test]
        public void CanConvertReferenceNestedNestedObjects()
        {
            var obj = new ObjectWithNestedNestedType()
            {
                PropNested = new ObjectWithNestedNestedType.NestedClass()
                {
                    PropNested = new ObjectWithNestedNestedType.NestedClass.NestedNestedClass()
                    {
                        PropString = "Nested"
                    }
                }
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropNested\":{\"PropNested\":{\"PropString\":\"Nested\"}}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithNestedNestedType, SFJsonTest\",\"PropNested\":{\"$type\":\"SFJsonTest.ObjectWithNestedNestedType+NestedClass, SFJsonTest\",\"PropNested\":{\"$type\":\"SFJsonTest.ObjectWithNestedNestedType+NestedClass+NestedNestedClass, SFJsonTest\",\"PropString\":\"Nested\"}}}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithNestedNestedType>(str);
            
            Assert.NotNull(strDeserialized);
            Assert.NotNull(strDeserialized.PropNested);
            Assert.NotNull(strDeserialized.PropNested.PropNested);
            Assert.AreEqual(obj.PropNested.PropNested.PropString, strDeserialized.PropNested.PropNested.PropString);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithNestedNestedType>(strWithType);
            
            Assert.NotNull(strWithTypeDeserialized);
            Assert.NotNull(strWithTypeDeserialized.PropNested);
            Assert.NotNull(strWithTypeDeserialized.PropNested.PropNested);
            Assert.AreEqual(obj.PropNested.PropNested.PropString, strWithTypeDeserialized.PropNested.PropNested.PropString);
        }
    }
}