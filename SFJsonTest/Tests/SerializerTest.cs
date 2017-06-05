using System;
using NUnit.Framework;
using SFJson;

namespace SFJsonTest
{
    [TestFixture]
    public class SerializerTest
    {
        private Serializer _serializer;

        [OneTimeSetUp]
        public void Init()
        {
            _serializer = new Serializer();
        }

        [Test]
        public void CanSerializeSimpleObjectType()
        {
            _serializer.ObjectToSerialize = new SimpleTestObject();
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"}", str);
        }

        [Test]
        public void CanSerializeSelfReferencedSimpleObjectType()
        {
            var obj = new SelfReferencedSimpleObject();
            obj.Inner = new SelfReferencedSimpleObject();
            _serializer.ObjectToSerialize = obj;
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":{\"$Type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":null}}", str);
        }

        [Test]
        public void CanSerializeSimpleObjectWithProperties()
        {
            _serializer.ObjectToSerialize = new SimpleTestObjectWithProperties
            {
                TestInt = 10
            };
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.SimpleTestObjectWithProperties, SFJsonTest\",\"TestInt\":10}", str);
        }

        [Test]
        public void CanSerializePrimitiveProperties()
        {
            _serializer.ObjectToSerialize = new PrimitiveHolder()
            {
                PropBool = false,
                PropDouble = 100.1,
                PropFloat = 1.1f,
                PropInt = 25,
                PropString = "String"
            };
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":False,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String\"}", str);
        }

        [Test]
        public void CanSerializeComplexObjects()
        {
            _serializer.ObjectToSerialize = new ComplexObject()
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
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.ComplexObject, SFJsonTest\",\"Inner\":{\"$Type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":{\"$Type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":null}},\"SimpleTestObject\":{\"$Type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"},\"SimpleTestObjectWithProperties\":{\"$Type\":\"SFJsonTest.SimpleTestObjectWithProperties, SFJsonTest\",\"TestInt\":10},\"PrimitiveHolder\":{\"$Type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":False,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String\"}}", str);
        }

        [Test]
        public void CanSerializeObjectWithAbstractType()
        {
            _serializer.ObjectToSerialize = new ObjectWithAbstractClass
            {
                ObjectImplementingAbstractClass = new ObjectImplementingAbstractClass()
                {
                    AbstractPropInt = 50,
                    PropInt = 100
                }
            };
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual(
                "{\"$Type\":\"SFJsonTest.ObjectWithAbstractClass, SFJsonTest\",\"ObjectImplementingAbstractClass\":{\"$Type\":\"SFJsonTest.ObjectImplementingAbstractClass, SFJsonTest\",\"AbstractPropInt\":50,\"PropInt\":100}}",
                str);
        }

        [Test]
        public void CanSerializeObjectWithInterface()
        {
            _serializer.ObjectToSerialize = new ObjectWithInterface()
            {
                ObjectImplementingInterface = new ObjectImplementingInterface()
                {
                    InterfacePropInt = 50,
                    PropInt = 100
                }
            };
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.ObjectWithInterface, SFJsonTest\",\"ObjectImplementingInterface\":{\"$Type\":\"SFJsonTest.ObjectImplementingInterface, SFJsonTest\",\"InterfacePropInt\":50,\"PropInt\":100}}", str);
        }

        [Test]
        public void CanSerializeWithGenericString()
        {
            _serializer.ObjectToSerialize = new GenericObject<string>()
            {
                GenericProp = "String"
            };
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.GenericObject`1[[System.String, mscorlib]], SFJsonTest\",\"GenericProp\":\"String\"}", str);
        }

        [Test]
        public void CanSerializeWithGenericInt()
        {
            _serializer.ObjectToSerialize = new GenericObject<int>()
            {
                GenericProp = 100
            };
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.GenericObject`1[[System.Int32, mscorlib]], SFJsonTest\",\"GenericProp\":100}", str);
        }

        [Test]
        public void CanSerializeEnum()
        {
            _serializer.ObjectToSerialize = new ObjectWithEnum()
            {
                Enums = Enums.Test2
            };
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.ObjectWithEnum, SFJsonTest\",\"Enums\":Test2}", str);
        
        }
    }
}