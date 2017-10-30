using System;
using NUnit.Framework;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Utils;
using UnityEngine;

namespace SFJsonTest
{
    [TestFixture]
    public class StructTests
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
        public void CanConvertSimpleStructs()
        {
            var obj = new StructObject()
            {
                Vector = new Vector3(1,2,3)
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings { SerializationType = SerializationType.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            
            var strDeserialized = _deserializer.Deserialize<StructObject>(str);
            
            Assert.NotNull(strDeserialized);
            Assert.AreEqual(obj.Vector, strDeserialized.Vector);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<StructObject>(strWithType);
            
            Assert.NotNull(strWithTypeDeserialized);
            Assert.AreEqual(obj.Vector, strWithTypeDeserialized.Vector);
        }
        
        [Test]
        public void CanConvertRect()
        {
            var obj = new Rect(5, 6, 10, 12);
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings { SerializationType = SerializationType.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            
            var strDeserialized = _deserializer.Deserialize<Rect>(str);
            
            Assert.NotNull(strDeserialized);
            Assert.AreEqual(obj, strDeserialized);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<Rect>(strWithType);
            
            Assert.NotNull(strWithTypeDeserialized);
            Assert.AreEqual(obj, strWithTypeDeserialized);
        }
    }
}