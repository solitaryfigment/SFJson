using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Utils;
using UnityEngine;

namespace SFJsonTests
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

        public class DictWrapper
        {
            public WorldDragEntityBehaviorProfile Profile;
        }

        [Test]
        public void CanConvertDictionaryIntKey()
        {
            var str =
                "{\"Id\":0,\"EntityBehaviorProfiles\":[{\"OffsetMap\":{8:{\"Offset\":{\"x\":0.5,\"y\":0.5,\"z\":0.5}}},\"Test\":5,\"TargetLayers\":[\"Ground\"],\"Type\":{\"Id\":2,\"Categories\":[\"World\",\"Permanent\",\"Interaction\"],\"Data\":{\"Id\":0,\"Values\":null}}},{\"TargetLayers\":[\"Grid\"],\"Type\":{\"Id\":4,\"Categories\":[\"UI\",\"Permanent\",\"Interaction\"],\"Data\":{\"Id\":0,\"Values\":null}}}]}";
            var strDeserialized = Converter.Deserialize<EntityProfile>(str);
            
            // Assert.NotNull(strDeserialized);
            // Assert.NotNull(strDeserialized);
            // Assert.AreEqual(1, strDeserialized.Dict.Count);
            //
            // strDeserialized = _deserializer.Deserialize<DictWrapper>(serialized);
            // Assert.NotNull(strDeserialized);
            // Assert.NotNull(strDeserialized);
            // Assert.AreEqual(1, strDeserialized.Dict.Count);
            
            // var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithDictionary>(strWithType);
            // Assert.NotNull(strDeserialized);
            // Assert.IsInstanceOf<ObjectWithDictionary>(strDeserialized);
            // Assert.NotNull(strDeserialized);
            // Assert.AreEqual(3, strDeserialized.Count);
        }
        
        [Test]
        public void CanDeserializeEmptyObjectIntoEmptyDictionary()
        {
            var str = "{\"Dictionary\":{}}";
            var strWithType = "{\"$type\":\"SFJsonTests.ObjectWithDictionary, SFJsonTests\",\"Dictionary\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.String, mscorlib],[System.Int32, mscorlib]], mscorlib\"}}";
            
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
            var strWithType = "{\"$type\":\"SFJsonTests.ObjectWithDictionary, SFJsonTests\",\"Dictionary\":[\"$type\":\"System.Collections.Generic.Dictionary`2[[System.String, mscorlib],[System.Int32, mscorlib]], mscorlib\"]}";
            
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
        public void CanConvertObjectWithDictionary()
        {
            var obj = new ObjectWithDictionary
            {
                Dictionary = new Dictionary<string, int>
                {
                    {"1", 2},
                    {"3", 4},
                    {"5", 6}
                }
            };
            
            var str = _serializer.Serialize(obj, new SerializerSettings() {FormattedString = false});
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All, FormattedString = false });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"Dictionary\":{\"1\":2,\"3\":4,\"5\":6}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTests.ObjectWithDictionary, SFJsonTests\",\"Dictionary\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[System.String, mscorlib],[System.Int32, mscorlib]], mscorlib\",\"1\":2,\"3\":4,\"5\":6}}", strWithType);

            var strDeserialized = _deserializer.Deserialize<ObjectWithDictionary>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithDictionary>(strDeserialized);
            Assert.AreEqual(obj.Dictionary.Count, strDeserialized.Dictionary.Count);
            var objKeys = obj.Dictionary.Keys.ToArray();
            var strDeserializedKeys = strDeserialized.Dictionary.Keys.ToArray();
            for(int i = 0; i < objKeys.Length; i++)
            {
                Assert.AreEqual(objKeys[i], strDeserializedKeys[i]);
                Assert.AreEqual(obj.Dictionary[objKeys[i]], strDeserialized.Dictionary[strDeserializedKeys[i]]);
            }
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithDictionary>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithDictionary>(strWithTypeDeserialized);
            Assert.AreEqual(obj.Dictionary.Count, strWithTypeDeserialized.Dictionary.Count);
            var strWithTypeDeserializedKeys = strDeserialized.Dictionary.Keys.ToArray();
            for(int i = 0; i < objKeys.Length; i++)
            {
                Assert.AreEqual(objKeys[i], strWithTypeDeserializedKeys[i]);
                Assert.AreEqual(obj.Dictionary[objKeys[i]], strWithTypeDeserialized.Dictionary[strWithTypeDeserializedKeys[i]]);
            }
        }

        private class BoolList : List<bool>
        {
            
        }
        private class ADictionary : Dictionary<string, bool[]>
        {
            
        }
        
        [Test]
        public void CanConvertDictionary()
        {
            var dictionary = new Dictionary<string, bool>();
            dictionary.Add("First", true);
            dictionary.Add("Second", false);
            dictionary.Add("Third", true);
            var str = _serializer.Serialize(dictionary);
            var deserialized = _deserializer.Deserialize<Dictionary<string, bool>>(str);
            
            Assert.AreEqual(dictionary, deserialized);
        }
        
        [Test]
        public void CanConvertPlainDictionary()
        {
            var dictionary = new ADictionary();
            dictionary.Add("First", new bool[]{true});
            dictionary.Add("Second", new bool[]{true, false});
            dictionary.Add("Third", new bool[]{false, false});
            var str = _serializer.Serialize(dictionary);
            
            Console.WriteLine(dictionary.GetType());
            var inters  = dictionary.GetType().GetInterfaces();
            
            Console.WriteLine((typeof(IDictionary).IsAssignableFrom(dictionary.GetType())));

            foreach(var type in inters)
            {
                if(type.IsGenericType)
                {
                    Console.WriteLine(type.GetGenericTypeDefinition());
                    Console.WriteLine((typeof(IDictionary<,>)) == type.GetGenericTypeDefinition());
                }

            }
            
            var deserialized = _deserializer.Deserialize<ADictionary>(str);
            
            Assert.AreEqual(dictionary, deserialized);
        }

        [Test]
        public void CanConvertObjectWithObjectDictionary()
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

            Assert.AreEqual("{\"Dictionary\":{{}:{},{}:{},{}:{}}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTests.ObjectWithObjectDictionary, SFJsonTests\",\"Dictionary\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[SFJsonTests.SimpleTestObject, SFJsonTests],[SFJsonTests.SimpleTestObject, SFJsonTests]], mscorlib\",{\"$type\":\"SFJsonTests.SimpleTestObject, SFJsonTests\"}:{\"$type\":\"SFJsonTests.SimpleTestObject, SFJsonTests\"},{\"$type\":\"SFJsonTests.SimpleTestObject, SFJsonTests\"}:{\"$type\":\"SFJsonTests.SimpleTestObject, SFJsonTests\"},{\"$type\":\"SFJsonTests.SimpleTestObject, SFJsonTests\"}:{\"$type\":\"SFJsonTests.SimpleTestObject, SFJsonTests\"}}}", strWithType);

            var strDeserialized = _deserializer.Deserialize<ObjectWithObjectDictionary>(str);
            Assert.AreEqual(obj.Dictionary.Count, strDeserialized.Dictionary.Count);
            foreach(var kvp in strDeserialized.Dictionary)
            {
                Assert.IsInstanceOf<SimpleTestObject>(kvp.Key);
                Assert.IsInstanceOf<SimpleTestObject>(kvp.Value);
            }
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithObjectDictionary>(str);
            Assert.AreEqual(obj.Dictionary.Count, strWithTypeDeserialized.Dictionary.Count);
            foreach(var kvp in strWithTypeDeserialized.Dictionary)
            {
                Assert.IsInstanceOf<SimpleTestObject>(kvp.Key);
                Assert.IsInstanceOf<SimpleTestObject>(kvp.Value);
            }
        }

        [TestCase(true)]
        [TestCase(false)]
        public void CanConvertObjectWithComplexObjectDictionary(bool formattedOutput)
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

            var str = _serializer.Serialize(obj, new SerializerSettings() {FormattedString = formattedOutput});
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() {SerializationTypeHandle = SerializationTypeHandle.All,FormattedString = formattedOutput});

            Console.WriteLine(str);
            Console.WriteLine(strWithType);

            if(!formattedOutput)
            {
                Assert.AreEqual("{\"Dictionary\":{{\"Inner\":{\"Inner\":{\"Inner\":null}},\"SimpleTestObject\":{},\"SimpleTestObjectWithProperties\":{\"FieldInt\":20,\"TestInt\":10},\"PrimitiveHolder\":{\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"First\"}}:{\"Inner\":{\"Inner\":{\"Inner\":null}},\"SimpleTestObject\":{},\"SimpleTestObjectWithProperties\":{\"FieldInt\":30,\"TestInt\":5},\"PrimitiveHolder\":{\"PropBool\":false,\"PropDouble\":2.1,\"PropFloat\":5.1,\"PropInt\":45,\"PropString\":\"Second\"}},{\"Inner\":{\"Inner\":{\"Inner\":null}},\"SimpleTestObject\":{},\"SimpleTestObjectWithProperties\":{\"FieldInt\":20,\"TestInt\":10},\"PrimitiveHolder\":{\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"First\"}}:{\"Inner\":{\"Inner\":{\"Inner\":null}},\"SimpleTestObject\":{},\"SimpleTestObjectWithProperties\":{\"FieldInt\":30,\"TestInt\":5},\"PrimitiveHolder\":{\"PropBool\":false,\"PropDouble\":2.1,\"PropFloat\":5.1,\"PropInt\":45,\"PropString\":\"Second\"}}}}",str);
                Assert.AreEqual("{\"$type\":\"SFJsonTests.ObjectWithComplexObjectDictionary, SFJsonTests\",\"Dictionary\":{\"$type\":\"System.Collections.Generic.Dictionary`2[[SFJsonTests.ComplexObject, SFJsonTests],[SFJsonTests.ComplexObject, SFJsonTests]], mscorlib\",{\"$type\":\"SFJsonTests.ComplexObject, SFJsonTests\",\"Inner\":{\"$type\":\"SFJsonTests.SelfReferencedSimpleObject, SFJsonTests\",\"Inner\":{\"$type\":\"SFJsonTests.SelfReferencedSimpleObject, SFJsonTests\",\"Inner\":null}},\"SimpleTestObject\":{\"$type\":\"SFJsonTests.SimpleTestObject, SFJsonTests\"},\"SimpleTestObjectWithProperties\":{\"$type\":\"SFJsonTests.SimpleTestObjectWithProperties, SFJsonTests\",\"FieldInt\":20,\"TestInt\":10},\"PrimitiveHolder\":{\"$type\":\"SFJsonTests.PrimitiveHolder, SFJsonTests\",\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"First\"}}:{\"$type\":\"SFJsonTests.ComplexObject, SFJsonTests\",\"Inner\":{\"$type\":\"SFJsonTests.SelfReferencedSimpleObject, SFJsonTests\",\"Inner\":{\"$type\":\"SFJsonTests.SelfReferencedSimpleObject, SFJsonTests\",\"Inner\":null}},\"SimpleTestObject\":{\"$type\":\"SFJsonTests.SimpleTestObject, SFJsonTests\"},\"SimpleTestObjectWithProperties\":{\"$type\":\"SFJsonTests.SimpleTestObjectWithProperties, SFJsonTests\",\"FieldInt\":30,\"TestInt\":5},\"PrimitiveHolder\":{\"$type\":\"SFJsonTests.PrimitiveHolder, SFJsonTests\",\"PropBool\":false,\"PropDouble\":2.1,\"PropFloat\":5.1,\"PropInt\":45,\"PropString\":\"Second\"}},{\"$type\":\"SFJsonTests.ComplexObject, SFJsonTests\",\"Inner\":{\"$type\":\"SFJsonTests.SelfReferencedSimpleObject, SFJsonTests\",\"Inner\":{\"$type\":\"SFJsonTests.SelfReferencedSimpleObject, SFJsonTests\",\"Inner\":null}},\"SimpleTestObject\":{\"$type\":\"SFJsonTests.SimpleTestObject, SFJsonTests\"},\"SimpleTestObjectWithProperties\":{\"$type\":\"SFJsonTests.SimpleTestObjectWithProperties, SFJsonTests\",\"FieldInt\":20,\"TestInt\":10},\"PrimitiveHolder\":{\"$type\":\"SFJsonTests.PrimitiveHolder, SFJsonTests\",\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"First\"}}:{\"$type\":\"SFJsonTests.ComplexObject, SFJsonTests\",\"Inner\":{\"$type\":\"SFJsonTests.SelfReferencedSimpleObject, SFJsonTests\",\"Inner\":{\"$type\":\"SFJsonTests.SelfReferencedSimpleObject, SFJsonTests\",\"Inner\":null}},\"SimpleTestObject\":{\"$type\":\"SFJsonTests.SimpleTestObject, SFJsonTests\"},\"SimpleTestObjectWithProperties\":{\"$type\":\"SFJsonTests.SimpleTestObjectWithProperties, SFJsonTests\",\"FieldInt\":30,\"TestInt\":5},\"PrimitiveHolder\":{\"$type\":\"SFJsonTests.PrimitiveHolder, SFJsonTests\",\"PropBool\":false,\"PropDouble\":2.1,\"PropFloat\":5.1,\"PropInt\":45,\"PropString\":\"Second\"}}}}",strWithType);
            }

            var strDeserialized = _deserializer.Deserialize<ObjectWithComplexObjectDictionary>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithComplexObjectDictionary>(strDeserialized);
            Assert.NotNull(strDeserialized.Dictionary);
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