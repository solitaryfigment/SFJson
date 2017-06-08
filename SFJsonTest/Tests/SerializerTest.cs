using System;
using System.Collections.Generic;
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

        [Test]
        public void CanSerializeNestedObjects()
        {
            _serializer.ObjectToSerialize = new ObjectWithNestedType.NestedClass()
            {
                PropString = "Nested"
            };
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.ObjectWithNestedType+NestedClass, SFJsonTest\",\"PropString\":\"Nested\"}", str);   
        }

        [Test]
        public void CanSerializeReferenceNestedObjects()
        {
            _serializer.ObjectToSerialize = new ObjectWithNestedType()
            {
                PropInt = 100,
                PropNested = new ObjectWithNestedType.NestedClass()
                {
                    PropString = "Nested"
                }
            };
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.ObjectWithNestedType, SFJsonTest\",\"PropNested\":{\"$Type\":\"SFJsonTest.ObjectWithNestedType+NestedClass, SFJsonTest\",\"PropString\":\"Nested\"},\"PropInt\":100}", str);
        }

        [Test]
        public void CanSerializeNestedNestedObjects()
        {
            _serializer.ObjectToSerialize = new ObjectWithNestedNestedType.NestedClass.NestedNestedClass()
            {
                PropString = "Nested"
            };
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.ObjectWithNestedNestedType+NestedClass+NestedNestedClass, SFJsonTest\",\"PropString\":\"Nested\"}", str);   
        }

        [Test]
        public void CanSerializeReferenceNestedNestedObjects()
        {
            _serializer.ObjectToSerialize = new ObjectWithNestedNestedType()
            {
                PropNested = new ObjectWithNestedNestedType.NestedClass()
                {
                    PropNested = new ObjectWithNestedNestedType.NestedClass.NestedNestedClass()
                    {
                        PropString = "Nested"
                    }
                }
            };
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.ObjectWithNestedNestedType, SFJsonTest\",\"PropNested\":{\"$Type\":\"SFJsonTest.ObjectWithNestedNestedType+NestedClass, SFJsonTest\",\"PropNested\":{\"$Type\":\"SFJsonTest.ObjectWithNestedNestedType+NestedClass+NestedNestedClass, SFJsonTest\",\"PropString\":\"Nested\"}}}", str);
        }

        [Test]
        public void CanSerializeObjectWithList()
        {
            _serializer.ObjectToSerialize = new ObjectWithList()
            {
                List = new List<int>()
                {
                    1, 2, 3, 4, 5
                }
            };
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.ObjectWithList, SFJsonTest\",\"List\":[\"$Type\":\"System.Collections.Generic.List`1[[System.Int32, mscorlib]], mscorlib\",\"$Values\":[1,2,3,4,5]]}", str);
        }

        [Test]
        public void CanSerializeObjectWithListOfObjects()
        {
            _serializer.ObjectToSerialize = new ObjectWithListOfObjects()
            {
                List = new List<PrimitiveHolder>()
                {
                    new PrimitiveHolder
                    {
                        PropBool = true,
                        PropDouble = 100.2,
                        PropFloat = 1.2f,
                        PropInt = 26,
                        PropString = "String"
                    },
                    new PrimitiveHolder
                    {
                        PropBool = false,
                        PropDouble = 100.1,
                        PropFloat = 1.1f,
                        PropInt = 25,
                        PropString = "String2"
                    }
                }
            };
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.ObjectWithListOfObjects, SFJsonTest\",\"List\":[\"$Type\":\"System.Collections.Generic.List`1[[SFJsonTest.PrimitiveHolder, SFJsonTest]], mscorlib\",\"$Values\":[{\"$Type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":True,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"$Type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":False,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]]}", str);
        }

        [Test]
        public void CanSerializeObjectWithArray()
        {
            _serializer.ObjectToSerialize = new ObjectWithArray()
            {
                Array = new int[]
                {
                    1, 2, 3, 4, 5
                }
            };
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.ObjectWithArray, SFJsonTest\",\"Array\":[\"$Type\":\"System.Int32[], mscorlib\",\"$Values\":[1,2,3,4,5]]}", str);
        }

        [Test]
        public void CanSerializeObjectWithArrayOfObjects()
        {
            _serializer.ObjectToSerialize = new ObjectWithArrayOfObjects()
            {
                Array = new PrimitiveHolder[]
                {
                    new PrimitiveHolder
                    {
                        PropBool = true,
                        PropDouble = 100.2,
                        PropFloat = 1.2f,
                        PropInt = 26,
                        PropString = "String"
                    },
                    new PrimitiveHolder
                    {
                        PropBool = false,
                        PropDouble = 100.1,
                        PropFloat = 1.1f,
                        PropInt = 25,
                        PropString = "String2"
                    }
                }
            };
            
            var str = _serializer.Serialize();

            Console.WriteLine(str);
            Assert.AreEqual("{\"$Type\":\"SFJsonTest.ObjectWithArrayOfObjects, SFJsonTest\",\"Array\":[\"$Type\":\"SFJsonTest.PrimitiveHolder[], SFJsonTest\",\"$Values\":[{\"$Type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":True,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"$Type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":False,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]]}", str);
        }
    }
}