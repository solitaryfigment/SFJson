using System;
using NUnit.Framework;

namespace SFJsonTest
{
    public class SimpleTestObject 
    {
    }
    
    public class SelfReferencedSimpleObject
    {
        public SelfReferencedSimpleObject Inner { get; set; }
    }
    
    public class SimpleTestObjectWithProperties
    {
        public int TestInt { get; set; }
    }

    [TestFixture]
    public class SerializerTest
    {
        [Test] 
        public void CanSerializeSimpleObjectType() 
        {
            var serializer = new SFJson.Serializer();
            serializer.ObjectToSerialize = new SimpleTestObject();
            var str = serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"}", str);
        }
        
        [Test] 
        public void CanSerializeSelfReferencedSimpleObjectType() 
        {
            var serializer = new SFJson.Serializer();
            var obj = new SelfReferencedSimpleObject();
            obj.Inner = new SelfReferencedSimpleObject();
            serializer.ObjectToSerialize = obj;
            var str = serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":{\"$Type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":null}}", str);
        }

        [Test]
        public void CanSerializeSimpleObjectWithProperties()
        {
            var serializer = new SFJson.Serializer();
            serializer.ObjectToSerialize = new SimpleTestObjectWithProperties
            {
                TestInt = 10
            };
            var str = serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.SimpleTestObjectWithProperties, SFJsonTest\",\"TestInt\":10}", str);
        }
    }
}