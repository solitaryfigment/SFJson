using System;
using NUnit.Framework;

namespace SFJsonTest
{
    public class DeserializeTest
    {
        [Test]
        public void CanDeserializeSimpleObjectType()
        {
            var deserializer = new SFJson.Deserializer();
            deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"}";
            Console.WriteLine(deserializer.StringToDeserialize);
            var obj = deserializer.Deserialize<SimpleTestObject>();
            Console.WriteLine(obj);
            Assert.IsTrue(obj != null);
        }
        
        [Test] 
        public void CanDeserializeSelfReferencedSimpleObjectType()
        {
            var deserializer = new SFJson.Deserializer();
            deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":{\"$Type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":null}}";
            Console.WriteLine(deserializer.StringToDeserialize);
            var obj = deserializer.Deserialize<SelfReferencedSimpleObject>();
            Console.WriteLine("1: " + obj);
            Assert.IsTrue(obj != null);
            Console.WriteLine("2: " + obj.Inner);
            Assert.IsTrue(obj.Inner != null);
        }
    }
}