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
        public void CanDeserializeStringWithExtraProperties()
        {
            var obj = new SimpleTestObject();
            var str = "{\"Extra\":12}";
            var strWithType = "{\"$type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\",\"Extra\":12}";

            var strDeserialized = _deserializer.Deserialize<SimpleTestObject>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<SimpleTestObject>(strDeserialized);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<SimpleTestObject>(strWithType);
            Assert.IsInstanceOf<SimpleTestObject>(strWithTypeDeserialized);
            Assert.NotNull(strWithTypeDeserialized);
        }

        [Test]
        public void CanConvertSimpleObjectType()
        {
            var obj = new SimpleTestObject();
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"}", strWithType);

            var strDeserialized = _deserializer.Deserialize<SimpleTestObject>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<SimpleTestObject>(strDeserialized);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<SimpleTestObject>(strWithType);
            Assert.IsInstanceOf<SimpleTestObject>(strWithTypeDeserialized);
            Assert.NotNull(strWithTypeDeserialized);
        }

        [Test]
        public void CanConvertComplexString()
        {
            var obj = new StringObject();
            obj.String = "{[This:\"is \"\" a\",string]}";
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"String\":\"{[This:\\\"is \\\"\\\" a\\\",string]}\"}", str);
//            Assert.AreEqual("{\"$type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"}", strWithType);

            var strDeserialized = _deserializer.Deserialize<StringObject>(str);
            Assert.IsInstanceOf<StringObject>(strDeserialized);
            Console.WriteLine(strDeserialized.String);
            Assert.AreEqual("{[This:\"is \"\" a\",string]}", strDeserialized.String);
            
//            var strWithTypeDeserialized = _deserializer.Deserialize<SimpleTestObject>(strWithType);
//            Assert.IsInstanceOf<SimpleTestObject>(strWithTypeDeserialized);
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
                PropString = "1"
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropBool\":True,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"1\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":True,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"1\"}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<PrimitiveHolder>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<PrimitiveHolder>(strDeserialized);
            Assert.AreEqual(obj.PropBool, strDeserialized.PropBool);
            Assert.AreEqual(obj.PropDouble, strDeserialized.PropDouble);
            Assert.AreEqual(obj.PropFloat, strDeserialized.PropFloat);
            Assert.AreEqual(obj.PropInt, strDeserialized.PropInt);
            Assert.AreEqual(obj.PropString, strDeserialized.PropString);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<PrimitiveHolder>(strWithType);
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
                FieldInt = 20,
                TestInt = 10
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"FieldInt\":20,\"TestInt\":10}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.SimpleTestObjectWithProperties, SFJsonTest\",\"FieldInt\":20,\"TestInt\":10}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<SimpleTestObjectWithProperties>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<SimpleTestObjectWithProperties>(strDeserialized);
            Assert.AreEqual(obj.TestInt, strDeserialized.TestInt);
            Assert.AreEqual(obj.FieldInt, strDeserialized.FieldInt);

            
            var strWithTypeDeserialized = _deserializer.Deserialize<SimpleTestObjectWithProperties>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<SimpleTestObjectWithProperties>(strWithTypeDeserialized);
            Assert.AreEqual(obj.TestInt, strWithTypeDeserialized.TestInt);
            Assert.AreEqual(obj.FieldInt, strWithTypeDeserialized.FieldInt);
        }
        
        [Test]
        public void CanConvertEnum()
        {
            var obj = new ObjectWithEnum
            {
                Enums = Enums.Test2
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });
            
            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"Enums\":Test2}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithEnum, SFJsonTest\",\"Enums\":Test2}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithEnum>(str);
            
            Assert.IsTrue(strDeserialized != null);
            Assert.IsInstanceOf<Enums>(strDeserialized.Enums);
            Assert.AreEqual(obj.Enums, strDeserialized.Enums);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithEnum>(strWithType);
            
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
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"Inner\":{\"Inner\":null}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":{\"$type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":null}}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<SelfReferencedSimpleObject>(str);
            
            Assert.IsTrue(strDeserialized != null);
            Assert.NotNull(strDeserialized.Inner);
            Assert.Null(strDeserialized.Inner.Inner);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<SelfReferencedSimpleObject>(strWithType);
            
            Assert.IsTrue(strWithTypeDeserialized != null);
            Assert.NotNull(strWithTypeDeserialized.Inner);
            Assert.Null(strWithTypeDeserialized.Inner.Inner);
        }
    }
}