using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SFJson;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Utils;

namespace SFJsonTest
{
    [TestFixture]
    public class DictionaryTests
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
        public void CanDeserializeEmptyObjectIntoEmptyDictionary()
        {
            var str = "{\"Dictionary\":{}}";
            var strWithType = "{\"$type\":\"SFJsonTest.ObjectWithDictionary, SFJsonTest\",\"Dictionary\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int32, mscorlib],[System.Int32, mscorlib]], mscorlib\"}}";
            
            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithDictionary>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithDictionary>(strDeserialized);
            Assert.NotNull(strDeserialized.Dictionary);
            Assert.AreEqual(0, strDeserialized.Dictionary.Count);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithDictionary>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithDictionary>(strWithTypeDeserialized);
            Assert.NotNull(strWithTypeDeserialized.Dictionary);
            Assert.AreEqual(0, strWithTypeDeserialized.Dictionary.Count);
        }
        
        [Test]
        public void CanDeserializeEmptyArrayIntoEmptyDictionary()
        {
            var str = "{\"Dictionary\":[]}";
            var strWithType = "{\"$type\":\"SFJsonTest.ObjectWithDictionary, SFJsonTest\",\"Dictionary\":[\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int32, mscorlib],[System.Int32, mscorlib]], mscorlib\"]}";
            
            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithDictionary>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithDictionary>(strDeserialized);
            Assert.NotNull(strDeserialized.Dictionary);
            Assert.AreEqual(0, strDeserialized.Dictionary.Count);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithDictionary>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithDictionary>(strWithTypeDeserialized);
            Assert.NotNull(strWithTypeDeserialized.Dictionary);
            Assert.AreEqual(0, strWithTypeDeserialized.Dictionary.Count);
        }
        
        [Test]
        public void CanConvertObjectWithDicitonary()
        {
            var obj = new ObjectWithDictionary
            {
                Dictionary = new Dictionary<int, int>
                {
                    {1, 2},
                    {3, 4},
                    {5, 6}
                }
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"Dictionary\":{\"1\":2,\"3\":4,\"5\":6}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithDictionary, SFJsonTest\",\"Dictionary\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.Int32, mscorlib],[System.Int32, mscorlib]], mscorlib\",\"1\":2,\"3\":4,\"5\":6}}", strWithType);

            var strDeserialized = _deserializer.Deserialize<ObjectWithDictionary>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithDictionary>(strDeserialized);
            Assert.AreEqual(obj.Dictionary.Count, strDeserialized.Dictionary.Count);
            var objKeys = ((IDictionary<int,int>)obj.Dictionary).Keys.ToArray();
            var strDeserializedKeys = ((IDictionary<int,int>)strDeserialized.Dictionary).Keys.ToArray();
            for(int i = 0; i < objKeys.Length; i++)
            {
                Assert.AreEqual(objKeys[i], strDeserializedKeys[i]);
                Assert.AreEqual(((IDictionary<int,int>)obj.Dictionary)[objKeys[i]], ((IDictionary<int,int>)strDeserialized.Dictionary)[strDeserializedKeys[i]]);
            }
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithDictionary>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithDictionary>(strWithTypeDeserialized);
            Assert.AreEqual(obj.Dictionary.Count, strWithTypeDeserialized.Dictionary.Count);
            var strWithTypeDeserializedKeys = ((IDictionary<int,int>)strDeserialized.Dictionary).Keys.ToArray();
            for(int i = 0; i < objKeys.Length; i++)
            {
                Assert.AreEqual(objKeys[i], strWithTypeDeserializedKeys[i]);
                Assert.AreEqual(((IDictionary<int,int>)obj.Dictionary)[objKeys[i]], ((IDictionary<int,int>)strWithTypeDeserialized.Dictionary)[strWithTypeDeserializedKeys[i]]);
            }
        }

        [Test]
        public void CanConvertObjectWithObjectDicitonary()
        {
            var obj = new ObjectWithObjectDictionary()
            {
                Dictionary = new Dictionary<SimpleTestObject, SimpleTestObject>
                {
                    {new SimpleTestObject(), new SimpleTestObject()},
                    {new SimpleTestObject(), new SimpleTestObject()},
                    {new SimpleTestObject(), new SimpleTestObject()}
                }
            };

            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() {SerializationTypeHandle = SerializationTypeHandle.All});

            Console.WriteLine(str);
            Console.WriteLine(strWithType);

            Assert.AreEqual("{\"Dictionary\":{\"{}\":{},\"{}\":{},\"{}\":{}}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithObjectDictionary, SFJsonTest\",\"Dictionary\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[SFJsonTest.SimpleTestObject, SFJsonTest],[SFJsonTest.SimpleTestObject, SFJsonTest]], mscorlib\",\"{\\\"$type\\\":\\\"SFJsonTest.SimpleTestObject, SFJsonTest\\\"}\":{\"$type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"},\"{\\\"$type\\\":\\\"SFJsonTest.SimpleTestObject, SFJsonTest\\\"}\":{\"$type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"},\"{\\\"$type\\\":\\\"SFJsonTest.SimpleTestObject, SFJsonTest\\\"}\":{\"$type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"}}}", strWithType);

            var strDeserialized = _deserializer.Deserialize<ObjectWithObjectDictionary>(str);
            foreach(var kvp in strDeserialized.Dictionary)
            {
                Assert.IsInstanceOf<SimpleTestObject>(kvp.Key);
                Assert.IsInstanceOf<SimpleTestObject>(kvp.Value);
            }
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithObjectDictionary>(str);
            foreach(var kvp in strWithTypeDeserialized.Dictionary)
            {
                Assert.IsInstanceOf<SimpleTestObject>(kvp.Key);
                Assert.IsInstanceOf<SimpleTestObject>(kvp.Value);
            }
        }

        [Test]
        public void CanConvertObjectWithComplexObjectDicitonary()
        {
            var obj = new ObjectWithComplexObjectDictionary()
            {
                Dictionary = new Dictionary<ComplexObject, ComplexObject>
                {
                    { 
                        new ComplexObject ()
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
                                PropString = "First"
                            }
                        },
                        new ComplexObject ()
                        {
                            Inner = new SelfReferencedSimpleObject()
                            {
                                Inner = new SelfReferencedSimpleObject()
                            },
                            SimpleTestObject = new SimpleTestObject(),
                            SimpleTestObjectWithProperties = new SimpleTestObjectWithProperties()
                            {
                                FieldInt = 30,
                                TestInt = 5
                            },
                            PrimitiveHolder = new PrimitiveHolder
                            {
                                PropBool = false,
                                PropDouble = 2.1,
                                PropFloat = 5.1f,
                                PropInt = 45,
                                PropString = "Second"
                            }
                        }
                    },
                    { 
                        new ComplexObject ()
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
                                PropString = "First"
                            }
                        },
                        new ComplexObject ()
                        {
                            Inner = new SelfReferencedSimpleObject()
                            {
                                Inner = new SelfReferencedSimpleObject()
                            },
                            SimpleTestObject = new SimpleTestObject(),
                            SimpleTestObjectWithProperties = new SimpleTestObjectWithProperties()
                            {
                                FieldInt = 30,
                                TestInt = 5
                            },
                            PrimitiveHolder = new PrimitiveHolder
                            {
                                PropBool = false,
                                PropDouble = 2.1,
                                PropFloat = 5.1f,
                                PropInt = 45,
                                PropString = "Second"
                            }
                        }
                    }
                }
            };

            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() {SerializationTypeHandle = SerializationTypeHandle.All});

            Console.WriteLine(str);
            Console.WriteLine(strWithType);

            Assert.AreEqual("{\"Dictionary\":{\"{\\\"Inner\\\":{\\\"Inner\\\":{\\\"Inner\\\":null}},\\\"SimpleTestObject\\\":{},\\\"SimpleTestObjectWithProperties\\\":{\\\"FieldInt\\\":20,\\\"TestInt\\\":10},\\\"PrimitiveHolder\\\":{\\\"PropBool\\\":false,\\\"PropDouble\\\":100.1,\\\"PropFloat\\\":1.1,\\\"PropInt\\\":25,\\\"PropString\\\":\\\"First\\\"}}\":{\"Inner\":{\"Inner\":{\"Inner\":null}},\"SimpleTestObject\":{},\"SimpleTestObjectWithProperties\":{\"FieldInt\":30,\"TestInt\":5},\"PrimitiveHolder\":{\"PropBool\":false,\"PropDouble\":2.1,\"PropFloat\":5.1,\"PropInt\":45,\"PropString\":\"Second\"}},\"{\\\"Inner\\\":{\\\"Inner\\\":{\\\"Inner\\\":null}},\\\"SimpleTestObject\\\":{},\\\"SimpleTestObjectWithProperties\\\":{\\\"FieldInt\\\":20,\\\"TestInt\\\":10},\\\"PrimitiveHolder\\\":{\\\"PropBool\\\":false,\\\"PropDouble\\\":100.1,\\\"PropFloat\\\":1.1,\\\"PropInt\\\":25,\\\"PropString\\\":\\\"First\\\"}}\":{\"Inner\":{\"Inner\":{\"Inner\":null}},\"SimpleTestObject\":{},\"SimpleTestObjectWithProperties\":{\"FieldInt\":30,\"TestInt\":5},\"PrimitiveHolder\":{\"PropBool\":false,\"PropDouble\":2.1,\"PropFloat\":5.1,\"PropInt\":45,\"PropString\":\"Second\"}}}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithComplexObjectDictionary, SFJsonTest\",\"Dictionary\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[SFJsonTest.ComplexObject, SFJsonTest],[SFJsonTest.ComplexObject, SFJsonTest]], mscorlib\",\"{\\\"$type\\\":\\\"SFJsonTest.ComplexObject, SFJsonTest\\\",\\\"Inner\\\":{\\\"$type\\\":\\\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\\\",\\\"Inner\\\":{\\\"$type\\\":\\\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\\\",\\\"Inner\\\":null}},\\\"SimpleTestObject\\\":{\\\"$type\\\":\\\"SFJsonTest.SimpleTestObject, SFJsonTest\\\"},\\\"SimpleTestObjectWithProperties\\\":{\\\"$type\\\":\\\"SFJsonTest.SimpleTestObjectWithProperties, SFJsonTest\\\",\\\"FieldInt\\\":20,\\\"TestInt\\\":10},\\\"PrimitiveHolder\\\":{\\\"$type\\\":\\\"SFJsonTest.PrimitiveHolder, SFJsonTest\\\",\\\"PropBool\\\":false,\\\"PropDouble\\\":100.1,\\\"PropFloat\\\":1.1,\\\"PropInt\\\":25,\\\"PropString\\\":\\\"First\\\"}}\":{\"$type\":\"SFJsonTest.ComplexObject, SFJsonTest\",\"Inner\":{\"$type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":{\"$type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":null}},\"SimpleTestObject\":{\"$type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"},\"SimpleTestObjectWithProperties\":{\"$type\":\"SFJsonTest.SimpleTestObjectWithProperties, SFJsonTest\",\"FieldInt\":30,\"TestInt\":5},\"PrimitiveHolder\":{\"$type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":false,\"PropDouble\":2.1,\"PropFloat\":5.1,\"PropInt\":45,\"PropString\":\"Second\"}},\"{\\\"$type\\\":\\\"SFJsonTest.ComplexObject, SFJsonTest\\\",\\\"Inner\\\":{\\\"$type\\\":\\\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\\\",\\\"Inner\\\":{\\\"$type\\\":\\\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\\\",\\\"Inner\\\":null}},\\\"SimpleTestObject\\\":{\\\"$type\\\":\\\"SFJsonTest.SimpleTestObject, SFJsonTest\\\"},\\\"SimpleTestObjectWithProperties\\\":{\\\"$type\\\":\\\"SFJsonTest.SimpleTestObjectWithProperties, SFJsonTest\\\",\\\"FieldInt\\\":20,\\\"TestInt\\\":10},\\\"PrimitiveHolder\\\":{\\\"$type\\\":\\\"SFJsonTest.PrimitiveHolder, SFJsonTest\\\",\\\"PropBool\\\":false,\\\"PropDouble\\\":100.1,\\\"PropFloat\\\":1.1,\\\"PropInt\\\":25,\\\"PropString\\\":\\\"First\\\"}}\":{\"$type\":\"SFJsonTest.ComplexObject, SFJsonTest\",\"Inner\":{\"$type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":{\"$type\":\"SFJsonTest.SelfReferencedSimpleObject, SFJsonTest\",\"Inner\":null}},\"SimpleTestObject\":{\"$type\":\"SFJsonTest.SimpleTestObject, SFJsonTest\"},\"SimpleTestObjectWithProperties\":{\"$type\":\"SFJsonTest.SimpleTestObjectWithProperties, SFJsonTest\",\"FieldInt\":30,\"TestInt\":5},\"PrimitiveHolder\":{\"$type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":false,\"PropDouble\":2.1,\"PropFloat\":5.1,\"PropInt\":45,\"PropString\":\"Second\"}}}}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithComplexObjectDictionary>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithComplexObjectDictionary>(strDeserialized);
            Assert.AreEqual(obj.Dictionary.Count, strDeserialized.Dictionary.Count);
            var objKeys = obj.Dictionary.Keys.ToArray();
            var strDeserializedKeys = strDeserialized.Dictionary.Keys.ToArray();
            for(int i = 0; i < objKeys.Length; i++)
            {
                Assert.IsTrue(objKeys[i].PrimitiveHolder != null);
                Assert.AreEqual(objKeys[i].PrimitiveHolder.PropBool, strDeserializedKeys[i].PrimitiveHolder.PropBool);
                Assert.AreEqual(objKeys[i].PrimitiveHolder.PropDouble, strDeserializedKeys[i].PrimitiveHolder.PropDouble);
                Assert.AreEqual(objKeys[i].PrimitiveHolder.PropFloat, strDeserializedKeys[i].PrimitiveHolder.PropFloat);
                Assert.AreEqual(objKeys[i].PrimitiveHolder.PropInt, strDeserializedKeys[i].PrimitiveHolder.PropInt);
                Assert.AreEqual(objKeys[i].PrimitiveHolder.PropString, strDeserializedKeys[i].PrimitiveHolder.PropString);
                        
                Assert.IsTrue(strDeserializedKeys[i].Inner != null);
                Assert.IsTrue(strDeserializedKeys[i].Inner.Inner != null);
                Assert.IsTrue(strDeserializedKeys[i].Inner.Inner.Inner == null);
            
                Assert.IsTrue(strDeserializedKeys[i].SimpleTestObject != null);
            
                Assert.IsTrue(strDeserializedKeys[i].SimpleTestObjectWithProperties != null);
                Assert.AreEqual(objKeys[i].SimpleTestObjectWithProperties.TestInt, strDeserializedKeys[i].SimpleTestObjectWithProperties.TestInt);
                Assert.AreEqual(objKeys[i].SimpleTestObjectWithProperties.FieldInt, strDeserializedKeys[i].SimpleTestObjectWithProperties.FieldInt);
            }
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithComplexObjectDictionary>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithComplexObjectDictionary>(strWithTypeDeserialized);
            Assert.AreEqual(obj.Dictionary.Count, strWithTypeDeserialized.Dictionary.Count);
            var strWithTypeDeserializedKeys = strWithTypeDeserialized.Dictionary.Keys.ToArray();
            for(int i = 0; i < objKeys.Length; i++)
            {
                Assert.IsTrue(objKeys[i].PrimitiveHolder != null);
                Assert.AreEqual(objKeys[i].PrimitiveHolder.PropBool, strWithTypeDeserializedKeys[i].PrimitiveHolder.PropBool);
                Assert.AreEqual(objKeys[i].PrimitiveHolder.PropDouble, strWithTypeDeserializedKeys[i].PrimitiveHolder.PropDouble);
                Assert.AreEqual(objKeys[i].PrimitiveHolder.PropFloat, strWithTypeDeserializedKeys[i].PrimitiveHolder.PropFloat);
                Assert.AreEqual(objKeys[i].PrimitiveHolder.PropInt, strWithTypeDeserializedKeys[i].PrimitiveHolder.PropInt);
                Assert.AreEqual(objKeys[i].PrimitiveHolder.PropString, strWithTypeDeserializedKeys[i].PrimitiveHolder.PropString);
                        
                Assert.IsTrue(strWithTypeDeserializedKeys[i].Inner != null);
                Assert.IsTrue(strWithTypeDeserializedKeys[i].Inner.Inner != null);
                Assert.IsTrue(strWithTypeDeserializedKeys[i].Inner.Inner.Inner == null);
            
                Assert.IsTrue(strWithTypeDeserializedKeys[i].SimpleTestObject != null);
            
                Assert.IsTrue(strWithTypeDeserializedKeys[i].SimpleTestObjectWithProperties != null);
                Assert.AreEqual(objKeys[i].SimpleTestObjectWithProperties.TestInt, strWithTypeDeserializedKeys[i].SimpleTestObjectWithProperties.TestInt);
                Assert.AreEqual(objKeys[i].SimpleTestObjectWithProperties.FieldInt, strWithTypeDeserializedKeys[i].SimpleTestObjectWithProperties.FieldInt);
            }
        }
    }
}