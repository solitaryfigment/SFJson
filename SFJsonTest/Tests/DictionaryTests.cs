using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SFJson;

namespace SFJsonTest
{
    [TestFixture]
    public class DictionaryTests
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
        public void CanConvertObjectWithDicitonary()
        {
            var obj = new ObjectWithDictionary()
            {
                Dictionary = new Dictionary<int, int>
                {
                    {1, 2},
                    {3, 4},
                    {5, 6}
                }
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"Dictionary\":[{\"1\":2},{\"3\":4},{\"5\":6}]}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithDictionary, SFJsonTest\",\"Dictionary\":[\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int32, mscorlib],[System.Int32, mscorlib]], mscorlib\",\"$values\":[{\"1\":2},{\"3\":4},{\"5\":6}]]}", strWithType);

            var strDeserialized = _deserializer.Deserialize<ObjectWithDictionary>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithDictionary>(strDeserialized);
            Assert.AreEqual(obj.Dictionary.Count, strDeserialized.Dictionary.Count);
            var objKeys = obj.Dictionary.Keys.ToArray();
            var strDeserializedKeys = obj.Dictionary.Keys.ToArray();
            for(int i = 0; i < objKeys.Length; i++)
            {
                Assert.AreEqual(objKeys[i], strDeserializedKeys[i]);
                Assert.AreEqual(obj.Dictionary[objKeys[i]], strDeserialized.Dictionary[strDeserializedKeys[i]]);
            }
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithDictionary>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithDictionary>(strWithTypeDeserialized);
            Assert.AreEqual(obj.Dictionary.Count, strWithTypeDeserialized.Dictionary.Count);
            var strWithTypeDeserializedKeys = obj.Dictionary.Keys.ToArray();
            for(int i = 0; i < objKeys.Length; i++)
            {
                Assert.AreEqual(objKeys[i], strWithTypeDeserializedKeys[i]);
                Assert.AreEqual(obj.Dictionary[objKeys[i]], strWithTypeDeserialized.Dictionary[strWithTypeDeserializedKeys[i]]);
            }
        }
    }
}