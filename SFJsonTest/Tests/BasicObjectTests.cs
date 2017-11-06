using System;
using NUnit.Framework;
using SFJson;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Utils;

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
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });

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
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"String\":\"{[This:\\\"is \\\"\\\" a\\\",string]}\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.StringObject, SFJsonTest\",\"String\":\"{[This:\\\"is \\\"\\\" a\\\",string]}\"}", strWithType);

            var strDeserialized = _deserializer.Deserialize<StringObject>(str);
            Assert.IsInstanceOf<StringObject>(strDeserialized);
            Assert.AreEqual("{[This:\"is \"\" a\",string]}", strDeserialized.String);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<StringObject>(strWithType);
            Assert.IsInstanceOf<StringObject>(strWithTypeDeserialized);
            Assert.AreEqual("{[This:\"is \"\" a\",string]}", strWithTypeDeserialized.String);
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
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropBool\":true,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"1\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":true,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"1\"}", strWithType);
            
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
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });

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
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });
            
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
        public void CanConvertGuid()
        {
            var obj = new GuidHolder()
            {
                PropGuid = Guid.NewGuid()
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });
            
            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual(string.Format("{0}\"PropGuid\":\"{1}\"{2}",'{',obj.PropGuid,'}'), str);
            Assert.AreEqual(string.Format("{0}\"$type\":\"SFJsonTest.GuidHolder, SFJsonTest\",\"PropGuid\":\"{1}\"{2}",'{',obj.PropGuid,'}'), strWithType);
            
            var strDeserialized = _deserializer.Deserialize<GuidHolder>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<GuidHolder>(strDeserialized);
            Assert.AreEqual(obj.PropGuid, strDeserialized.PropGuid);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<GuidHolder>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<GuidHolder>(strWithTypeDeserialized);
            Assert.AreEqual(obj.PropGuid, strWithTypeDeserialized.PropGuid);
        }

        [Test]
        public void CanConvertSelfReferencedSimpleObjectType()
        {
            var obj = new SelfReferencedSimpleObject
            {
                Inner = new SelfReferencedSimpleObject()
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings { SerializationTypeHandle = SerializationTypeHandle.All });

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

        [Test]
        public void CanConvertNullString()
        {
            var obj = new StringHolder()
            {
                PropString = null
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings { SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropString\":null}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.StringHolder, SFJsonTest\",\"PropString\":null}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<StringHolder>(str);
            
            Assert.NotNull(strDeserialized);
            Assert.IsNull(strDeserialized.PropString);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<StringHolder>(strWithType);
            
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsNull(strWithTypeDeserialized.PropString);
        }

        [Test]
        public void CanConvertEmptyString()
        {
            var obj = new StringHolder()
            {
                PropString = string.Empty
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings { SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropString\":\"\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.StringHolder, SFJsonTest\",\"PropString\":\"\"}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<StringHolder>(str);
            
            Assert.NotNull(strDeserialized);
            Assert.AreEqual(string.Empty, strDeserialized.PropString);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<StringHolder>(strWithType);
            
            Assert.NotNull(strWithTypeDeserialized);
            Assert.AreEqual(string.Empty, strWithTypeDeserialized.PropString);
        }

        [Test]
        public void CanConvertNumberPrimitiveProperties()
        {
            var obj = new PrimitiveHolder2()
            {
                PropDecimal = 3.2m,
                PropByte = 255,
                PropSByte = 127,
                PropInt16 = 32767,
                PropInt32 = 2147483647,
                PropInt64 = 9223372036854775807,
                PropUInt = 4294967295,
                PropUInt16 = 65535,
                PropUInt32 = 4294967295,
                PropUInt64 = 18446744073709551615
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            
            Assert.AreEqual("{\"PropDecimal\":3.2,\"PropByte\":255,\"PropSByte\":127,\"PropUInt\":4294967295,\"PropUInt16\":65535,\"PropInt16\":32767,\"PropUInt32\":4294967295,\"PropInt32\":2147483647,\"PropUInt64\":18446744073709551615,\"PropInt64\":9223372036854775807}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.PrimitiveHolder2, SFJsonTest\",\"PropDecimal\":3.2,\"PropByte\":255,\"PropSByte\":127,\"PropUInt\":4294967295,\"PropUInt16\":65535,\"PropInt16\":32767,\"PropUInt32\":4294967295,\"PropInt32\":2147483647,\"PropUInt64\":18446744073709551615,\"PropInt64\":9223372036854775807}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<PrimitiveHolder2>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<PrimitiveHolder2>(strDeserialized);
            Assert.AreEqual(obj.PropDecimal, strDeserialized.PropDecimal);
            Assert.AreEqual(obj.PropByte, strDeserialized.PropByte);
            Assert.AreEqual(obj.PropSByte, strDeserialized.PropSByte);
            Assert.AreEqual(obj.PropInt16, strDeserialized.PropInt16);
            Assert.AreEqual(obj.PropInt32, strDeserialized.PropInt32);
            Assert.AreEqual(obj.PropInt64, strDeserialized.PropInt64);
            Assert.AreEqual(obj.PropUInt, strDeserialized.PropUInt);
            Assert.AreEqual(obj.PropUInt16, strDeserialized.PropUInt16);
            Assert.AreEqual(obj.PropUInt32, strDeserialized.PropUInt32);
            Assert.AreEqual(obj.PropUInt64, strDeserialized.PropUInt64);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<PrimitiveHolder2>(str);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<PrimitiveHolder2>(strWithTypeDeserialized);
            Assert.AreEqual(obj.PropDecimal, strWithTypeDeserialized.PropDecimal);
            Assert.AreEqual(obj.PropByte, strWithTypeDeserialized.PropByte);
            Assert.AreEqual(obj.PropSByte, strWithTypeDeserialized.PropSByte);
            Assert.AreEqual(obj.PropInt16, strWithTypeDeserialized.PropInt16);
            Assert.AreEqual(obj.PropInt32, strWithTypeDeserialized.PropInt32);
            Assert.AreEqual(obj.PropInt64, strWithTypeDeserialized.PropInt64);
            Assert.AreEqual(obj.PropUInt, strWithTypeDeserialized.PropUInt);
            Assert.AreEqual(obj.PropUInt16, strWithTypeDeserialized.PropUInt16);
            Assert.AreEqual(obj.PropUInt32, strWithTypeDeserialized.PropUInt32);
            Assert.AreEqual(obj.PropUInt64, strWithTypeDeserialized.PropUInt64);
        }
    }
}