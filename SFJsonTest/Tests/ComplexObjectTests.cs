using System;
using NUnit.Framework;
using SFJson;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Utils;

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
        public void CanConvertCompositeObjects()
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
                    FieldInt = 20,
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
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"Inner\":{\"Inner\":{\"Inner\":null}},\"SimpleTestObject\":{},\"SimpleTestObjectWithProperties\":{\"FieldInt\":20,\"TestInt\":10},\"PrimitiveHolder\":{\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String\"}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ComplexObject, SFJsonTest\",\"Inner\":{\"$type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":{\"$type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":null}},\"SimpleTestObject\":{\"$type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"},\"SimpleTestObjectWithProperties\":{\"$type\":\"SFJsonTest.SimpleTestObjectWithProperties, SFJsonTest\",\"FieldInt\":20,\"TestInt\":10},\"PrimitiveHolder\":{\"$type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String\"}}", strWithType);

            var strDeserialized = _deserializer.Deserialize<ComplexObject>(str);
            
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
            Assert.AreEqual(obj.SimpleTestObjectWithProperties.FieldInt, strDeserialized.SimpleTestObjectWithProperties.FieldInt);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ComplexObject>(strWithType);
            
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
            Assert.AreEqual(obj.SimpleTestObjectWithProperties.FieldInt, strWithTypeDeserialized.SimpleTestObjectWithProperties.FieldInt);

        }

    }
}