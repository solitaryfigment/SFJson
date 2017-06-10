using System;
using NUnit.Framework;
using SFJson;

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
            
            _serializer.ObjectToSerialize = obj;
            var str = _serializer.Serialize();
            var strWithType = _serializer.Serialize(new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropString\":\"Nested\"}", str);   
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithNestedType+NestedClass, SFJsonTest\",\"PropString\":\"Nested\"}", strWithType);

            _deserializer.StringToDeserialize = str;
            var strDeserialized = _deserializer.Deserialize<ObjectWithNestedType.NestedClass>();
            
            Assert.NotNull(strDeserialized);
            Assert.AreEqual(obj.PropString, strDeserialized.PropString);
            
            _deserializer.StringToDeserialize = strWithType;
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithNestedType.NestedClass>();
            
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
            
            _serializer.ObjectToSerialize = obj;
            var str = _serializer.Serialize();
            var strWithType = _serializer.Serialize(new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropNested\":{\"PropString\":\"Nested\"},\"PropInt\":100}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithNestedType, SFJsonTest\",\"PropNested\":{\"$type\":\"SFJsonTest.ObjectWithNestedType+NestedClass, SFJsonTest\",\"PropString\":\"Nested\"},\"PropInt\":100}", strWithType);
            
            _deserializer.StringToDeserialize = str;
            var strDeserialized = _deserializer.Deserialize<ObjectWithNestedType>();
            
            Assert.NotNull(strDeserialized);
            Assert.AreEqual(obj.PropInt, strDeserialized.PropInt);
            Assert.NotNull(strDeserialized.PropNested);
            Assert.AreEqual(obj.PropNested.PropString, strDeserialized.PropNested.PropString);
            
            _deserializer.StringToDeserialize = strWithType;
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithNestedType>();
            
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
            
            _serializer.ObjectToSerialize = obj;
            var str = _serializer.Serialize();
            var strWithType = _serializer.Serialize(new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropString\":\"Nested\"}", str);   
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithNestedNestedType+NestedClass+NestedNestedClass, SFJsonTest\",\"PropString\":\"Nested\"}", strWithType);
            
            _deserializer.StringToDeserialize = str;
            var strDeserialized = _deserializer.Deserialize<ObjectWithNestedNestedType.NestedClass.NestedNestedClass>();
            
            Assert.NotNull(strDeserialized);
            Assert.AreEqual(obj.PropString, strDeserialized.PropString);
            
            _deserializer.StringToDeserialize = strWithType;
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithNestedNestedType.NestedClass.NestedNestedClass>();
            
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
            
            _serializer.ObjectToSerialize = obj;
            var str = _serializer.Serialize();
            var strWithType = _serializer.Serialize(new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropNested\":{\"PropNested\":{\"PropString\":\"Nested\"}}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithNestedNestedType, SFJsonTest\",\"PropNested\":{\"$type\":\"SFJsonTest.ObjectWithNestedNestedType+NestedClass, SFJsonTest\",\"PropNested\":{\"$type\":\"SFJsonTest.ObjectWithNestedNestedType+NestedClass+NestedNestedClass, SFJsonTest\",\"PropString\":\"Nested\"}}}", strWithType);
            
            _deserializer.StringToDeserialize = str;
            var strDeserialized = _deserializer.Deserialize<ObjectWithNestedNestedType>();
            
            Assert.NotNull(strDeserialized);
            Assert.NotNull(strDeserialized.PropNested);
            Assert.NotNull(strDeserialized.PropNested.PropNested);
            Assert.AreEqual(obj.PropNested.PropNested.PropString, strDeserialized.PropNested.PropNested.PropString);
            
            _deserializer.StringToDeserialize = strWithType;
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithNestedNestedType>();
            
            Assert.NotNull(strWithTypeDeserialized);
            Assert.NotNull(strWithTypeDeserialized.PropNested);
            Assert.NotNull(strWithTypeDeserialized.PropNested.PropNested);
            Assert.AreEqual(obj.PropNested.PropNested.PropString, strWithTypeDeserialized.PropNested.PropNested.PropString);
        }
    }
}