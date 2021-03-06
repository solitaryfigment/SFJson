using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using NUnit.Framework;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Tokenization.Tokens;
using SFJson.Utils;

namespace SFJsonTests
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
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropType\":\"SFJson.Tokenization.Tokens.JsonCollection, SFJson\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTests.TypeHolder, SFJsonTests\",\"PropType\":\"SFJson.Tokenization.Tokens.JsonCollection, SFJson\"}", strWithType);

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
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropType\":\"SFJsonTests.SimpleBaseObject, SFJsonTests\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTests.TypeHolder, SFJsonTests\",\"PropType\":\"SFJsonTests.SimpleBaseObject, SFJsonTests\"}", strWithType);

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
                PropType = typeof(Dictionary<Dictionary<JsonTokenType,int[]>,List<string[]>>)
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropType\":\"System.Collections.Generic.Dictionary`2[[System.Collections.Generic.Dictionary`2[[SFJson.Tokenization.Tokens.JsonTokenType, SFJson],[System.Int32[], System.Private.CoreLib]], System.Private.CoreLib],[System.Collections.Generic.List`1[[System.String[], System.Private.CoreLib]], System.Private.CoreLib]], System.Private.CoreLib\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTests.TypeHolder, SFJsonTests\",\"PropType\":\"System.Collections.Generic.Dictionary`2[[System.Collections.Generic.Dictionary`2[[SFJson.Tokenization.Tokens.JsonTokenType, SFJson],[System.Int32[], System.Private.CoreLib]], System.Private.CoreLib],[System.Collections.Generic.List`1[[System.String[], System.Private.CoreLib]], System.Private.CoreLib]], System.Private.CoreLib\"}", strWithType);

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
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropType\":null}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTests.TypeHolder, SFJsonTests\",\"PropType\":null}", strWithType);

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
            var str = "{\"PropType\":\"TypeThatIsNotThere, System.Private.CoreLib\"}";
            var strWithType = "{\"$type\":\"SFJsonTests.TypeHolder, SFJsonTests\",\"PropType\":\"TypeThatIsNotThere, System.Private.CoreLib\"}";

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