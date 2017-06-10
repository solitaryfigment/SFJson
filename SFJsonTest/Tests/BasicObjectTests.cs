using System;
using NUnit.Framework;
using SFJson;

namespace SFJsonTest
{
    [TestFixture]
    public class BasicObjectTests
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
        public void CanConvertSimpleObjectType()
        {
            _serializer.ObjectToSerialize = new SimpleTestObject();
            
            var str = _serializer.Serialize();
            var strWithType = _serializer.Serialize(new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"}", strWithType);

            _deserializer.StringToDeserialize = str;
            var strDeserialized = _deserializer.Deserialize<SimpleTestObject>();
            Assert.IsInstanceOf<SimpleTestObject>(strDeserialized);
            
            _deserializer.StringToDeserialize = strWithType;
            var strWithTypeDeserialized = _deserializer.Deserialize<SimpleTestObject>();
            Assert.IsInstanceOf<SimpleTestObject>(strWithTypeDeserialized);
        }

        [Test]
        public void CanConvertPrimitiveProperties()
        {
            var obj = new PrimitiveHolder()
            {
                PropBool = true,
                PropDouble = 100.1,
                PropFloat = 1.1f,
                PropInt = 25,
                PropString = "String"
            };
            
            _serializer.ObjectToSerialize = obj;
            var str = _serializer.Serialize();
            var strWithType = _serializer.Serialize(new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropBool\":True,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":True,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String\"}", strWithType);
            
            _deserializer.StringToDeserialize = str;
            var strDeserialized = _deserializer.Deserialize<PrimitiveHolder>();
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<PrimitiveHolder>(strDeserialized);
            Assert.AreEqual(obj.PropBool, strDeserialized.PropBool);
            Assert.AreEqual(obj.PropDouble, strDeserialized.PropDouble);
            Assert.AreEqual(obj.PropFloat, strDeserialized.PropFloat);
            Assert.AreEqual(obj.PropInt, strDeserialized.PropInt);
            Assert.AreEqual(obj.PropString, strDeserialized.PropString);
            
            _deserializer.StringToDeserialize = strWithType;
            var strWithTypeDeserialized = _deserializer.Deserialize<PrimitiveHolder>();
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<PrimitiveHolder>(strWithTypeDeserialized);
            Assert.AreEqual(obj.PropBool, strWithTypeDeserialized.PropBool);
            Assert.AreEqual(obj.PropDouble, strWithTypeDeserialized.PropDouble);
            Assert.AreEqual(obj.PropFloat, strWithTypeDeserialized.PropFloat);
            Assert.AreEqual(obj.PropInt, strWithTypeDeserialized.PropInt);
            Assert.AreEqual(obj.PropString, strWithTypeDeserialized.PropString);
        }

        [Test]
        public void CanConvertSimpleObjectWithProperties()
        {
            var obj = new SimpleTestObjectWithProperties
            {
                TestInt = 10
            };

            _serializer.ObjectToSerialize = obj;
            var str = _serializer.Serialize();
            var strWithType = _serializer.Serialize(new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"TestInt\":10}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.SimpleTestObjectWithProperties, SFJsonTest\",\"TestInt\":10}", strWithType);
            
            _deserializer.StringToDeserialize = str;
            var strDeserialized = _deserializer.Deserialize<SimpleTestObjectWithProperties>();
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<SimpleTestObjectWithProperties>(strDeserialized);
            Assert.AreEqual(obj.TestInt, strDeserialized.TestInt);
            
            _deserializer.StringToDeserialize = strWithType;
            var strWithTypeDeserialized = _deserializer.Deserialize<SimpleTestObjectWithProperties>();
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<SimpleTestObjectWithProperties>(strWithTypeDeserialized);
            Assert.AreEqual(obj.TestInt, strWithTypeDeserialized.TestInt);
        }
        
        [Test]
        public void CanConvertEnum()
        {
            var obj = new ObjectWithEnum
            {
                Enums = Enums.Test2
            };

            _serializer.ObjectToSerialize = obj;
            var str = _serializer.Serialize();
            var strWithType = _serializer.Serialize(new SerializerSettings() { TypeHandler = TypeHandler.All });
            
            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"Enums\":Test2}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithEnum, SFJsonTest\",\"Enums\":Test2}", strWithType);
            
            _deserializer.StringToDeserialize = str;
            var strDeserialized = _deserializer.Deserialize<ObjectWithEnum>();
            
            Assert.IsTrue(strDeserialized != null);
            Assert.IsInstanceOf<Enums>(strDeserialized.Enums);
            Assert.AreEqual(obj.Enums, strDeserialized.Enums);
            
            _deserializer.StringToDeserialize = str;
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithEnum>();
            
            Assert.IsTrue(strWithTypeDeserialized != null);
            Assert.IsInstanceOf<Enums>(strWithTypeDeserialized.Enums);
            Assert.AreEqual(obj.Enums, strWithTypeDeserialized.Enums);
        }

        [Test]
        public void CanConvertSelfReferencedSimpleObjectType()
        {
            var obj = new SelfReferencedSimpleObject
            {
                Inner = new SelfReferencedSimpleObject()
            };
            
            _serializer.ObjectToSerialize = obj;
            var str = _serializer.Serialize();
            var strWithType = _serializer.Serialize(new SerializerSettings { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"Inner\":{\"Inner\":null}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":{\"$type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":null}}", strWithType);
            
            _deserializer.StringToDeserialize = str;
            var strDeserialized = _deserializer.Deserialize<SelfReferencedSimpleObject>();
            
            Assert.IsTrue(strDeserialized != null);
            Assert.NotNull(strDeserialized.Inner);
            Assert.Null(strDeserialized.Inner.Inner);
            
            _deserializer.StringToDeserialize = str;
            var strWithTypeDeserialized = _deserializer.Deserialize<SelfReferencedSimpleObject>();
            
            Assert.IsTrue(strWithTypeDeserialized != null);
            Assert.NotNull(strWithTypeDeserialized.Inner);
            Assert.Null(strWithTypeDeserialized.Inner.Inner);
        }

    }
}