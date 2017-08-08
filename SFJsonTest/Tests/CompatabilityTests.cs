using System;
using Newtonsoft.Json;
using NUnit.Framework;
using SFJson;
using SFJson.Conversion;

namespace SFJsonTest
{
    [TestFixture]
    public class CompatabilityTests
    {
        private Serializer _serializer;
        private Deserializer _deserializer;
        private JsonSerializerSettings _newtonsoftSettings;

        [OneTimeSetUp]
        public void Init()
        {
            _deserializer = new Deserializer();
            _serializer = new Serializer();
            
            _newtonsoftSettings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All
            };
        }

        [Test]
        public void Primitives()
        {
            var obj = new PrimitiveHolder()
            {
                PropBool = true,
                PropDouble = 100.1,
                PropFloat = 1.1f,
                PropInt = 25,
                PropString = "1"
            };
            
//            var strNS = _serializer.Serialize(obj);
//            var strWithTypeNS = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });
            var strNS = JsonConvert.SerializeObject(obj);
            var strWithTypeNS = JsonConvert.SerializeObject(obj, _newtonsoftSettings);
            
            Console.WriteLine(strNS);
            Console.WriteLine(strWithTypeNS);
            
            var deserializedNSToSF = _deserializer.Deserialize<PrimitiveHolder>(strNS);
            Assert.NotNull(deserializedNSToSF);
            Assert.IsInstanceOf<PrimitiveHolder>(deserializedNSToSF);
            Assert.AreEqual(obj.PropBool, deserializedNSToSF.PropBool);
            Assert.AreEqual(obj.PropDouble, deserializedNSToSF.PropDouble);
            Assert.AreEqual(obj.PropFloat, deserializedNSToSF.PropFloat);
            Assert.AreEqual(obj.PropInt, deserializedNSToSF.PropInt);
            Assert.AreEqual(obj.PropString, deserializedNSToSF.PropString);
            
            var deserializedWithTypeNSToSF = _deserializer.Deserialize<PrimitiveHolder>(strWithTypeNS);
            Assert.NotNull(deserializedWithTypeNSToSF);
            Assert.IsInstanceOf<PrimitiveHolder>(deserializedWithTypeNSToSF);
            Assert.AreEqual(obj.PropBool, deserializedWithTypeNSToSF.PropBool);
            Assert.AreEqual(obj.PropDouble, deserializedWithTypeNSToSF.PropDouble);
            Assert.AreEqual(obj.PropFloat, deserializedWithTypeNSToSF.PropFloat);
            Assert.AreEqual(obj.PropInt, deserializedWithTypeNSToSF.PropInt);
            Assert.AreEqual(obj.PropString, deserializedWithTypeNSToSF.PropString);
        }

        [Test]
        public void NameConversionCompatability()
        {
            var obj = new PrimitiveHolderWithNameConversion()
            {
                PropBool = true,
                PropDouble = 100.1,
                PropFloat = 1.1f,
                PropInt = 25,
                PropString = "1"
            };
            
//            var strNS = _serializer.Serialize(obj);
//            var strWithTypeNS = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });
            var strNS = JsonConvert.SerializeObject(obj);
            var strWithTypeNS = JsonConvert.SerializeObject(obj, _newtonsoftSettings);
            
            Console.WriteLine(strNS);
            Console.WriteLine(strWithTypeNS);
            
            var deserializedNSToSF = _deserializer.Deserialize<PrimitiveHolderWithNameConversion>(strNS);
            Assert.NotNull(deserializedNSToSF);
            Assert.IsInstanceOf<PrimitiveHolderWithNameConversion>(deserializedNSToSF);
            Assert.AreEqual(obj.PropBool, deserializedNSToSF.PropBool);
            Assert.AreEqual(obj.PropDouble, deserializedNSToSF.PropDouble);
            Assert.AreEqual(obj.PropFloat, deserializedNSToSF.PropFloat);
            Assert.AreEqual(obj.PropInt, deserializedNSToSF.PropInt);
            Assert.AreEqual(obj.PropString, deserializedNSToSF.PropString);
            
            var deserializedWithTypeNSToSF = _deserializer.Deserialize<PrimitiveHolderWithNameConversion>(strWithTypeNS);
            Assert.NotNull(deserializedWithTypeNSToSF);
            Assert.IsInstanceOf<PrimitiveHolderWithNameConversion>(deserializedWithTypeNSToSF);
            Assert.AreEqual(obj.PropBool, deserializedWithTypeNSToSF.PropBool);
            Assert.AreEqual(obj.PropDouble, deserializedWithTypeNSToSF.PropDouble);
            Assert.AreEqual(obj.PropFloat, deserializedWithTypeNSToSF.PropFloat);
            Assert.AreEqual(obj.PropInt, deserializedWithTypeNSToSF.PropInt);
            Assert.AreEqual(obj.PropString, deserializedWithTypeNSToSF.PropString);
        }
    }
}
