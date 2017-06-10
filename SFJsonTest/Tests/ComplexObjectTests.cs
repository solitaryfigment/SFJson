using System;
using NUnit.Framework;
using SFJson;

namespace SFJsonTest
{
    [TestFixture]
    public class ComplexObjectTests
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
        public void CanConvertComplexObjects()
        {
            var obj = new ComplexObject()
            {
                Inner = new SelfReferencedSimpleObject()
                {
                    Inner = new SelfReferencedSimpleObject()
                },
                SimpleTestObject = new SimpleTestObject(),
                SimpleTestObjectWithProperties = new SimpleTestObjectWithProperties()
                {
                    TestInt = 10
                },
                PrimitiveHolder = new PrimitiveHolder
                {
                    PropBool = false,
                    PropDouble = 100.1,
                    PropFloat = 1.1f,
                    PropInt = 25,
                    PropString = "String"
                },
            };

            _serializer.ObjectToSerialize = obj;
            var str = _serializer.Serialize();
            var strWithType = _serializer.Serialize(new SerializerSettings { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"Inner\":{\"Inner\":{\"Inner\":null}},\"SimpleTestObject\":{},\"SimpleTestObjectWithProperties\":{\"TestInt\":10},\"PrimitiveHolder\":{\"PropBool\":False,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String\"}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ComplexObject, SFJsonTest\",\"Inner\":{\"$type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":{\"$type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":null}},\"SimpleTestObject\":{\"$type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"},\"SimpleTestObjectWithProperties\":{\"$type\":\"SFJsonTest.SimpleTestObjectWithProperties, SFJsonTest\",\"TestInt\":10},\"PrimitiveHolder\":{\"$type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":False,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String\"}}", strWithType);

            _deserializer.StringToDeserialize = str;
            var strDeserialized = _deserializer.Deserialize<ComplexObject>();
            
            Assert.IsTrue(strDeserialized != null);
            
            Assert.IsTrue(strDeserialized.PrimitiveHolder != null);
            Assert.AreEqual(obj.PrimitiveHolder.PropBool, strDeserialized.PrimitiveHolder.PropBool);
            Assert.AreEqual(obj.PrimitiveHolder.PropDouble, strDeserialized.PrimitiveHolder.PropDouble);
            Assert.AreEqual(obj.PrimitiveHolder.PropFloat, strDeserialized.PrimitiveHolder.PropFloat);
            Assert.AreEqual(obj.PrimitiveHolder.PropInt, strDeserialized.PrimitiveHolder.PropInt);
            Assert.AreEqual(obj.PrimitiveHolder.PropString, strDeserialized.PrimitiveHolder.PropString);
                        
            Assert.IsTrue(strDeserialized.Inner != null);
            Assert.IsTrue(strDeserialized.Inner.Inner != null);
            Assert.IsTrue(strDeserialized.Inner.Inner.Inner == null);
            
            Assert.IsTrue(strDeserialized.SimpleTestObject != null);
            
            Assert.IsTrue(strDeserialized.SimpleTestObjectWithProperties != null);
            Assert.AreEqual(obj.SimpleTestObjectWithProperties.TestInt, strDeserialized.SimpleTestObjectWithProperties.TestInt);
            
            _deserializer.StringToDeserialize = str;
            var strWithTypeDeserialized = _deserializer.Deserialize<ComplexObject>();
            
            Assert.IsTrue(strWithTypeDeserialized != null);
            
            Assert.IsTrue(strWithTypeDeserialized.PrimitiveHolder != null);
            Assert.AreEqual(obj.PrimitiveHolder.PropBool, strWithTypeDeserialized.PrimitiveHolder.PropBool);
            Assert.AreEqual(obj.PrimitiveHolder.PropDouble, strWithTypeDeserialized.PrimitiveHolder.PropDouble);
            Assert.AreEqual(obj.PrimitiveHolder.PropFloat, strWithTypeDeserialized.PrimitiveHolder.PropFloat);
            Assert.AreEqual(obj.PrimitiveHolder.PropInt, strWithTypeDeserialized.PrimitiveHolder.PropInt);
            Assert.AreEqual(obj.PrimitiveHolder.PropString, strWithTypeDeserialized.PrimitiveHolder.PropString);
                        
            Assert.IsTrue(strWithTypeDeserialized.Inner != null);
            Assert.IsTrue(strWithTypeDeserialized.Inner.Inner != null);
            Assert.IsTrue(strWithTypeDeserialized.Inner.Inner.Inner == null);
            
            Assert.IsTrue(strWithTypeDeserialized.SimpleTestObject != null);
            
            Assert.IsTrue(strWithTypeDeserialized.SimpleTestObjectWithProperties != null);
            Assert.AreEqual(obj.SimpleTestObjectWithProperties.TestInt, strWithTypeDeserialized.SimpleTestObjectWithProperties.TestInt);
        }

    }
}