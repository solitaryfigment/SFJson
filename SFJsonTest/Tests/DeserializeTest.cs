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

        [Test]
        public void CanDeserializeNestedObject()
        {
            _deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.ObjectWithNestedType+NestedClass, SFJsonTest\",\"PropString\":\"Nested\"}";

            var obj = _deserializer.Deserialize<ObjectWithNestedType.NestedClass>();
            
            Assert.IsTrue(obj != null);
            Assert.AreEqual("Nested", obj.PropString);
        }

        [Test]
        public void CanDeserializeReferencedNestedObject()
        {
            _deserializer.StringToDeserialize =
                "{\"$Type\":\"SFJsonTest.ObjectWithNestedType, SFJsonTest\",\"PropNested\":{\"$Type\":\"SFJsonTest.ObjectWithNestedType+NestedClass, SFJsonTest\",\"PropString\":\"Nested\"},\"PropInt\":100}";

            var obj = _deserializer.Deserialize<ObjectWithNestedType>();
            
            Assert.IsTrue(obj != null);
            Assert.AreEqual(100, obj.PropInt);
            Assert.IsTrue(obj.PropNested != null);
            Assert.AreEqual("Nested", obj.PropNested.PropString);
        }

        [Test]
        public void CanDeserializeNestedNestedObject()
        {
            _deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.ObjectWithNestedNestedType+NestedClass+NestedNestedClass, SFJsonTest\",\"PropString\":\"Nested\"}";

            var obj = _deserializer.Deserialize<ObjectWithNestedNestedType.NestedClass.NestedNestedClass>();
            
            Assert.IsTrue(obj != null);
            Assert.AreEqual("Nested", obj.PropString);
        }

        [Test]
        public void CanDeserializeReferencedNestedNestedObject()
        {
            _deserializer.StringToDeserialize =
                "{\"$Type\":\"SFJsonTest.ObjectWithNestedNestedType, SFJsonTest\",\"PropNested\":{\"$Type\":\"SFJsonTest.ObjectWithNestedNestedType+NestedClass, SFJsonTest\",\"PropNested\":{\"$Type\":\"SFJsonTest.ObjectWithNestedNestedType+NestedClass+NestedNestedClass, SFJsonTest\",\"PropString\":\"Nested\"}}}";

            var obj = _deserializer.Deserialize<ObjectWithNestedNestedType>();
            
            Assert.IsTrue(obj != null);
            Assert.IsTrue(obj.PropNested != null);
            Assert.IsTrue(obj.PropNested.PropNested != null);
            Assert.AreEqual("Nested", obj.PropNested.PropNested.PropString);
        }

        [Test]
        public void CanDeserializeObjectWithList()
        {
            _deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.ObjectWithList, SFJsonTest\",\"List\":[\"$Type\":\"System.Collections.Generic.List`1[[System.Int32, mscorlib]], mscorlib\",\"$Values\":[1,2,3,4,5]]}";

            var obj = _deserializer.Deserialize<ObjectWithList>();

            Assert.IsTrue(obj != null);
            Assert.IsTrue(obj.List != null);
            Assert.AreEqual(5, obj.List.Count);
            for(int i = 0; i < obj.List.Count; i++)
            {
                Assert.AreEqual(i + 1, obj.List[i]);
            }
        }
        
        [Test]
        public void CanDeserializeObjectWithListOfObjects()
        {
            _deserializer.StringToDeserialize =
                "{\"$Type\":\"SFJsonTest.ObjectWithListOfObjects, SFJsonTest\",\"List\":[\"$Type\":\"System.Collections.Generic.List`1[[SFJsonTest.PrimitiveHolder, SFJsonTest]], mscorlib\",\"$Values\":[{\"$Type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":True,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"$Type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":False,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]]}";
            
            var obj = _deserializer.Deserialize<ObjectWithListOfObjects>();
            
            Assert.IsTrue(obj != null);
            Assert.IsTrue(obj.List != null);
            Assert.AreEqual(2, obj.List.Count);
            
            Assert.IsTrue(obj.List[0] != null);
            Assert.IsTrue(obj.List[0].PropBool);
            Assert.AreEqual(100.2d, obj.List[0].PropDouble);
            Assert.AreEqual(1.2f, obj.List[0].PropFloat);
            Assert.AreEqual(26, obj.List[0].PropInt);
            Assert.AreEqual("String", obj.List[0].PropString);
            
            Assert.IsTrue(obj.List[1] != null);
            Assert.IsFalse(obj.List[1].PropBool);
            Assert.AreEqual(100.1d, obj.List[1].PropDouble);
            Assert.AreEqual(1.1f, obj.List[1].PropFloat);
            Assert.AreEqual(25, obj.List[1].PropInt);
            Assert.AreEqual("String2", obj.List[1].PropString);
        }

        [Test]
        public void CanDeserializeObjectWithArray()
        {
            _deserializer.StringToDeserialize = "{\"$Type\":\"SFJsonTest.ObjectWithArray, SFJsonTest\",\"Array\":[\"$Type\":\"System.Int32[], mscorlib\",\"$Values\":[1,2,3,4,5]]}";

            var obj = _deserializer.Deserialize<ObjectWithArray>();

            Assert.IsTrue(obj != null);
            Assert.IsTrue(obj.Array != null);
            Assert.AreEqual(5, obj.Array.Length);
            for(int i = 0; i < obj.Array.Length; i++)
            {
                Assert.AreEqual(i + 1, obj.Array[i]);
            }
        }
        
        [Test]
        public void CanDeserializeObjectWithArrayOfObjects()
        {
            _deserializer.StringToDeserialize =
                "{\"$Type\":\"SFJsonTest.ObjectWithArrayOfObjects, SFJsonTest\",\"Array\":[\"$Type\":\"SFJsonTest.PrimitiveHolder[], SFJsonTest\",\"$Values\":[{\"$Type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":True,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"$Type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":False,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]]}";
            
            var obj = _deserializer.Deserialize<ObjectWithArrayOfObjects>();
            
            Assert.IsTrue(obj != null);
            Assert.IsTrue(obj.Array != null);
            Assert.AreEqual(2, obj.Array.Length);
            
            Assert.IsTrue(obj.Array[0] != null);
            Assert.IsTrue(obj.Array[0].PropBool);
            Assert.AreEqual(100.2d, obj.Array[0].PropDouble);
            Assert.AreEqual(1.2f, obj.Array[0].PropFloat);
            Assert.AreEqual(26, obj.Array[0].PropInt);
            Assert.AreEqual("String", obj.Array[0].PropString);
            
            Assert.IsTrue(obj.Array[1] != null);
            Assert.IsFalse(obj.Array[1].PropBool);
            Assert.AreEqual(100.1d, obj.Array[1].PropDouble);
            Assert.AreEqual(1.1f, obj.Array[1].PropFloat);
            Assert.AreEqual(25, obj.Array[1].PropInt);
            Assert.AreEqual("String2", obj.Array[1].PropString);
        }
    }
}