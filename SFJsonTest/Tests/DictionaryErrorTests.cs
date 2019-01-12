using NUnit.Framework;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Exceptions;

namespace SFJsonTest
{
    [TestFixture]
    public class DictionaryErrorTests
    {
        private Deserializer _deserializer;

        [OneTimeSetUp]
        public void Init()
        {
            _deserializer = new Deserializer();
        }
        
        [Test]
        public void NullKeyInDictionaryThrowsExpection()
        {
            var str = "{\"Dictionary\":{null:2,\"3\":4,\"5\":6}}";
            var strWithType = "{\"$type\":\"SFJsonTest.ObjectWithDictionary, SFJsonTest\",\"Dictionary\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int32, mscorlib],[System.Int32, mscorlib]], mscorlib\",null:2,\"3\":4,\"5\":6}}";

            Assert.Throws<DeserializationException>(() =>
            {
                _deserializer.Deserialize<ObjectWithDictionary>(str);
            });
            
            Assert.Throws<DeserializationException>(() =>
            {
                _deserializer.Deserialize<ObjectWithDictionary>(strWithType);
            });
         }
        
        [Test]
        public void SkipNullKeyInDictionaryWithSettings()
        {
            var str = "{\"Dictionary\":{null:2,\"3\":4,\"5\":6}}";
            var strWithType = "{\"$type\":\"SFJsonTest.ObjectWithDictionary, SFJsonTest\",\"Dictionary\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.String, mscorlib],[System.Int32, mscorlib]], mscorlib\",null:2,\"3\":4,\"5\":6}}";
            var deserializerSettings = new DeserializerSettings()
            {
                SkipNullKeysInKeyValuedCollections = true
            };
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithDictionary>(str, deserializerSettings);
            Assert.NotNull(strDeserialized);
            Assert.NotNull(strDeserialized.Dictionary);
//            Assert.AreEqual(2, strDeserialized.Dictionary.Count);
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithDictionary>(strWithType, deserializerSettings);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.NotNull(strWithTypeDeserialized.Dictionary);
//            Assert.AreEqual(2, strWithTypeDeserialized.Dictionary.Count);
        }
    }
}