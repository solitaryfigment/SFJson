using System;
using NUnit.Framework;
using SFJson;

namespace SFJsonTest
{
    public class DeserializeTest
    {
        private Deserializer _deserializer;

        [OneTimeSetUp]
        public void Init()
        {
            _deserializer = new Deserializer();
        }
        
        [Test]
        public void CanDeserializeSimpleObjectTypeWithProperties()
        {
            _deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.SimpleTestObjectWithProperties, SFJsonTest\",\"TestInt\":10}";

            var obj = _deserializer.Deserialize<SimpleTestObjectWithProperties>();

            Assert.IsTrue(obj != null);
            Assert.AreEqual(10, obj.TestInt);
        }
        
        [Test]
        public void CanDeserializeSimpleObjectType()
        {
            _deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"}";
            
            var obj = _deserializer.Deserialize<SimpleTestObject>();

            Assert.IsTrue(obj != null);
        }
        
        [Test] 
        public void CanDeserializeSelfReferencedSimpleObjectType()
        {
            _deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":{\"$Type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":null}}";

            var obj = _deserializer.Deserialize<SelfReferencedSimpleObject>();
            
            Assert.IsTrue(obj != null);
            Assert.IsTrue(obj.Inner != null);
        }

        [Test]
        public void CanDeserializePrimitives()
        {
            _deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":True,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String\"}";

            var obj = _deserializer.Deserialize<PrimitiveHolder>();
            
            Assert.IsTrue(obj != null);
            Assert.IsTrue(obj.PropBool);
            Assert.AreEqual(100.1d, obj.PropDouble);
            Assert.AreEqual(1.1f, obj.PropFloat);
            Assert.AreEqual(25, obj.PropInt);
            Assert.AreEqual("String", obj.PropString);
        }

        [Test]
        public void CanDeserializeComplexObjects()
        {
            _deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.ComplexObject, SFJsonTest\",\"Inner\":{\"$Type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":{\"$Type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":null}},\"SimpleTestObject\":{\"$Type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"},\"SimpleTestObjectWithProperties\":{\"$Type\":\"SFJsonTest.SimpleTestObjectWithProperties, SFJsonTest\",\"TestInt\":10},\"PrimitiveHolder\":{\"$Type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":True,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String\"}}";

            var obj = _deserializer.Deserialize<ComplexObject>();
            
            Assert.IsTrue(obj != null);
            
            Assert.IsTrue(obj.PrimitiveHolder != null);
            Assert.IsTrue(obj.PrimitiveHolder.PropBool);
            Assert.AreEqual(100.1d, obj.PrimitiveHolder.PropDouble);
            Assert.AreEqual(1.1f, obj.PrimitiveHolder.PropFloat);
            Assert.AreEqual(25, obj.PrimitiveHolder.PropInt);
            Assert.AreEqual("String", obj.PrimitiveHolder.PropString);
                        
            Assert.IsTrue(obj.Inner != null);
            Assert.IsTrue(obj.Inner.Inner != null);
            Assert.IsTrue(obj.Inner.Inner.Inner == null);
            
            Assert.IsTrue(obj.SimpleTestObject != null);
            
            Assert.IsTrue(obj.SimpleTestObjectWithProperties != null);
            Assert.AreEqual(10, obj.SimpleTestObjectWithProperties.TestInt);
        }
        
        [Test]
        public void CanDeserializeObjectWithAbstractType()
        {
            _deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.ObjectWithAbstractClass, SFJsonTest\",\"ObjectImplementingAbstractClass\":{\"$Type\":\"SFJsonTest.ObjectImplementingAbstractClass, SFJsonTest\",\"AbstractPropInt\":50,\"PropInt\":100}}";

            var obj = _deserializer.Deserialize<ObjectWithAbstractClass>();

            Assert.IsTrue(obj != null);
            Assert.IsInstanceOf<ObjectImplementingAbstractClass>(obj.ObjectImplementingAbstractClass);
            Assert.AreEqual(50, ((ObjectImplementingAbstractClass)obj.ObjectImplementingAbstractClass).AbstractPropInt);
            Assert.AreEqual(100, ((ObjectImplementingAbstractClass)obj.ObjectImplementingAbstractClass).PropInt);
        }
        
        [Test]
        public void CanDeserializeObjectWithInterface()
        {
            _deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.ObjectWithInterface, SFJsonTest\",\"ObjectImplementingInterface\":{\"$Type\":\"SFJsonTest.ObjectImplementingInterface, SFJsonTest\",\"InterfacePropInt\":50,\"PropInt\":100}}";

            var obj = _deserializer.Deserialize<ObjectWithInterface>();
            
            Assert.IsTrue(obj != null);
            Assert.IsInstanceOf<ObjectImplementingInterface>(obj.ObjectImplementingInterface);
            Assert.AreEqual(50, ((ObjectImplementingInterface)obj.ObjectImplementingInterface).InterfacePropInt);
            Assert.AreEqual(100, ((ObjectImplementingInterface)obj.ObjectImplementingInterface).PropInt);
        }

        [Test]
        public void CanDeserializeGenericString()
        {
            _deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.GenericObject`1[[System.String, mscorlib]], SFJsonTest\",\"GenericProp\":\"String\"}";
            
            var obj = _deserializer.Deserialize<GenericObject<string>>();
            
            Assert.IsTrue(obj != null);
            Assert.IsInstanceOf<GenericObject<string>>(obj);
            Assert.IsInstanceOf<string>(obj.GenericProp);
            Assert.AreEqual("String", obj.GenericProp);
        }

        [Test]
        public void CanDeserializeGenericInt()
        {
            _deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.GenericObject`1[[System.Int32, mscorlib]], SFJsonTest\",\"GenericProp\":100}";
            
            var objInt = _deserializer.Deserialize<GenericObject<int>>();
            
            Assert.IsTrue(objInt != null);
            Assert.IsInstanceOf<GenericObject<int>>(objInt);
            Assert.IsInstanceOf<int>(objInt.GenericProp);
            Assert.AreEqual(100, objInt.GenericProp);
        }

        [Test]
        public void CanDeserializeEnum()
        {
            _deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.ObjectWithEnum, SFJsonTest\",\"Enums\":Test2}";

            var obj = _deserializer.Deserialize<ObjectWithEnum>();
            
            Assert.IsTrue(obj != null);
            Assert.IsInstanceOf<Enums>(obj.Enums);
            Assert.AreEqual(Enums.Test2, obj.Enums);
        }
    }
}