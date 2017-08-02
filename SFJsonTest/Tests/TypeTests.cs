using System;
using System.Collections.Generic;
using NUnit.Framework;
using SFJson.Conversion;
using SFJson.Tokenization.Tokens;
using SFJson.Utils;

namespace SFJsonTest
{
    [TestFixture]
    public class TypeTests
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
        public void CanConvertObjectWithTypePropertyInDifferentAssembly()
        {
            var obj = new TypeHolder()
            {
                PropType = typeof(JsonCollection)
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropType\":\"SFJson.Tokenization.Tokens.JsonCollection, SFJson\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.TypeHolder, SFJsonTest\",\"PropType\":\"SFJson.Tokenization.Tokens.JsonCollection, SFJson\"}", strWithType);

            var strDeserialized = _deserializer.Deserialize<TypeHolder>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<TypeHolder>(strDeserialized);
            Assert.AreEqual(obj.PropType, strDeserialized.PropType);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<TypeHolder>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<TypeHolder>(strWithTypeDeserialized);
            Assert.AreEqual(obj.PropType, strWithTypeDeserialized.PropType);
        }
        
        [Test]
        public void CanConvertObjectWithTypePropertyInSameAssembly()
        {
            var obj = new TypeHolder()
            {
                PropType = typeof(SimpleBaseObject)
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropType\":\"SFJsonTest.SimpleBaseObject, SFJsonTest\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.TypeHolder, SFJsonTest\",\"PropType\":\"SFJsonTest.SimpleBaseObject, SFJsonTest\"}", strWithType);

            var strDeserialized = _deserializer.Deserialize<TypeHolder>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<TypeHolder>(strDeserialized);
            Assert.AreEqual(obj.PropType, strDeserialized.PropType);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<TypeHolder>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<TypeHolder>(strWithTypeDeserialized);
            Assert.AreEqual(obj.PropType, strWithTypeDeserialized.PropType);
        }
        
        [Test]
        public void CanConvertObjectWithComplexTypeProperty()
        {
            var obj = new TypeHolder()
            {
                PropType = typeof(Dictionary<Dictionary<JsonType,int[]>,List<string[]>>)
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropType\":\"System.Collections.Generic.Dictionary`2[[System.Collections.Generic.Dictionary`2[[SFJson.Tokenization.Tokens.JsonType, SFJson],[System.Int32[], mscorlib]], mscorlib],[System.Collections.Generic.List`1[[System.String[], mscorlib]], mscorlib]], mscorlib\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.TypeHolder, SFJsonTest\",\"PropType\":\"System.Collections.Generic.Dictionary`2[[System.Collections.Generic.Dictionary`2[[SFJson.Tokenization.Tokens.JsonType, SFJson],[System.Int32[], mscorlib]], mscorlib],[System.Collections.Generic.List`1[[System.String[], mscorlib]], mscorlib]], mscorlib\"}", strWithType);

            var strDeserialized = _deserializer.Deserialize<TypeHolder>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<TypeHolder>(strDeserialized);
            Assert.AreEqual(obj.PropType, strDeserialized.PropType);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<TypeHolder>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<TypeHolder>(strWithTypeDeserialized);
            Assert.AreEqual(obj.PropType, strWithTypeDeserialized.PropType);
        }
        
        [Test]
        public void ConvertsNullType()
        {
            var obj = new TypeHolder()
            {
                PropType = null
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropType\":null}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.TypeHolder, SFJsonTest\",\"PropType\":null}", strWithType);

            var strDeserialized = _deserializer.Deserialize<TypeHolder>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<TypeHolder>(strDeserialized);
            Assert.IsNull(strDeserialized.PropType);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<TypeHolder>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<TypeHolder>(strWithTypeDeserialized);
            Assert.IsNull(strWithTypeDeserialized.PropType);
        }
        
        [Test]
        public void ConvertsTypeWhichDoesNotExistToNull()
        {
            var str = "{\"PropType\":\"TypeThatIsNotThere, mscorlib\"}";
            var strWithType = "{\"$type\":\"SFJsonTest.TypeHolder, SFJsonTest\",\"PropType\":\"TypeThatIsNotThere, mscorlib\"}";

            var strDeserialized = _deserializer.Deserialize<TypeHolder>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<TypeHolder>(strDeserialized);
            Assert.IsNull(strDeserialized.PropType);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<TypeHolder>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<TypeHolder>(strWithTypeDeserialized);
            Assert.IsNull(strWithTypeDeserialized.PropType);
        }
    }
}