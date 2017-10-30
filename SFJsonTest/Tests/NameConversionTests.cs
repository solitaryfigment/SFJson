using System;
using NUnit.Framework;
using SFJson;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Utils;

namespace SFJsonTest
{
    [TestFixture]
    public class NameConversionTests
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
        public void CanConvertWithNameConversion()
        {
            var obj = new PrimitiveHolderWithNameConversion()
            {
                PropBool = true,
                PropDouble = 100.1,
                PropFloat = 1.1f,
                PropInt = 25,
                PropString = "1"
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationType = SerializationType.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"b\":true,\"d\":100.1,\"f\":1.1,\"i\":25,\"s\":\"1\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.PrimitiveHolderWithNameConversion, SFJsonTest\",\"b\":true,\"d\":100.1,\"f\":1.1,\"i\":25,\"s\":\"1\"}", strWithType);
            
            
            var strDeserialized = _deserializer.Deserialize<PrimitiveHolderWithNameConversion>(str);
            var afterstr = _serializer.Serialize(strDeserialized);
            Console.WriteLine(afterstr);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<PrimitiveHolderWithNameConversion>(strDeserialized);
            Assert.AreEqual(obj.PropBool, strDeserialized.PropBool);
            Assert.AreEqual(obj.PropDouble, strDeserialized.PropDouble);
            Assert.AreEqual(obj.PropFloat, strDeserialized.PropFloat);
            Assert.AreEqual(obj.PropInt, strDeserialized.PropInt);
            Assert.AreEqual(obj.PropString, strDeserialized.PropString);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<PrimitiveHolderWithNameConversion>(strWithType);
            var afterstrWithType = _serializer.Serialize(strWithTypeDeserialized, new SerializerSettings() { SerializationType = SerializationType.All });
            Console.WriteLine(afterstrWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<PrimitiveHolderWithNameConversion>(strWithTypeDeserialized);
            Assert.AreEqual(obj.PropBool, strWithTypeDeserialized.PropBool);
            Assert.AreEqual(obj.PropDouble, strWithTypeDeserialized.PropDouble);
            Assert.AreEqual(obj.PropFloat, strWithTypeDeserialized.PropFloat);
            Assert.AreEqual(obj.PropInt, strWithTypeDeserialized.PropInt);
            Assert.AreEqual(obj.PropString, strWithTypeDeserialized.PropString);
        }
    }
}
