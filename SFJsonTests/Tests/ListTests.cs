using System;
using System.Linq;
using System.Collections.Generic;
using NUnit.Framework;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Utils;

namespace SFJsonTests
{
    [TestFixture]
    public class ListTests
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
        public void CanDeserializeEmptyObjectIntoEmptyList()
        {
            var str = "{\"List\":{}}";
            var strWithType = "{\"$type\":\"SFJsonTests.ObjectWithList, SFJsonTests\",\"List\":{\"$type\":\"System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\",\"$values\":{}}}";
            
            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithList>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithList>(strDeserialized);
            Assert.NotNull(strDeserialized.List);
            Assert.AreEqual(0, strDeserialized.List.Count);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithList>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithList>(strWithTypeDeserialized);
            Assert.NotNull(strWithTypeDeserialized.List);
            Assert.AreEqual(0, strWithTypeDeserialized.List.Count);
        }
        
        [Test]
        public void CanDeserializeEmptyArrayIntoEmptyList()
        {
            var str = "{\"List\":[]}";
            var strWithType = "{\"$type\":\"SFJsonTests.ObjectWithList, SFJsonTests\",\"List\":{\"$type\":\"System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\",\"$values\":[]}}";
            
            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithList>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithList>(strDeserialized);
            Assert.NotNull(strDeserialized.List);
            Assert.AreEqual(0, strDeserialized.List.Count);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithList>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithList>(strWithTypeDeserialized);
            Assert.NotNull(strWithTypeDeserialized.List);
            Assert.AreEqual(0, strWithTypeDeserialized.List.Count);
        }
        
        [Test]
        public void CanConvertList()
        {
            var list = new List<int>
            {
                1, 2, 3, 4, 5
            };
            
            var str = _serializer.Serialize(list);
            // var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            // Console.WriteLine(strWithType);
            Assert.AreEqual("[1,2,3,4,5]", str);
            // Assert.AreEqual("{\"$type\":\"SFJsonTests.ObjectWithList, SFJsonTests\",\"List\":{\"$type\":\"System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\",\"$values\":[1,2,3,4,5]}}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<List<int>>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<List<int>>(strDeserialized);
            Assert.AreEqual(list.Count, strDeserialized.Count);
            for(int i = 0; i < strDeserialized.Count(); i++)
            {
                Assert.AreEqual(list.ElementAt(i), strDeserialized.ElementAt(i));
            }
            
            // var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithList>(strWithType);
            // Assert.NotNull(strWithTypeDeserialized);
            // Assert.IsInstanceOf<ObjectWithList>(strWithTypeDeserialized);
            // Assert.AreEqual(list.Count, strDeserialized.Count);
            // for(int i = 0; i < strWithTypeDeserialized.List.Count(); i++)
            // {
            //     Assert.AreEqual(obj.List.ElementAt(i), strWithTypeDeserialized.List.ElementAt(i));
            // }
        }

        private class AList : List<int>
        {
            
        }
        
        [Test]
        public void CanConvertPlainList()
        {
            var list = new AList()
            {
                1, 2, 3, 4, 5
            };
            
            var str = _serializer.Serialize(list);
            // var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            // Console.WriteLine(strWithType);
            Assert.AreEqual("[1,2,3,4,5]", str);
            // Assert.AreEqual("{\"$type\":\"SFJsonTests.ObjectWithList, SFJsonTests\",\"List\":{\"$type\":\"System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\",\"$values\":[1,2,3,4,5]}}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<AList>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<AList>(strDeserialized);
            Assert.AreEqual(list.Count, strDeserialized.Count);
            for(int i = 0; i < strDeserialized.Count(); i++)
            {
                Assert.AreEqual(list.ElementAt(i), strDeserialized.ElementAt(i));
            }
            
            // var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithList>(strWithType);
            // Assert.NotNull(strWithTypeDeserialized);
            // Assert.IsInstanceOf<ObjectWithList>(strWithTypeDeserialized);
            // Assert.AreEqual(list.Count, strDeserialized.Count);
            // for(int i = 0; i < strWithTypeDeserialized.List.Count(); i++)
            // {
            //     Assert.AreEqual(obj.List.ElementAt(i), strWithTypeDeserialized.List.ElementAt(i));
            // }
        }

        [Test]
        public void CanConvertObjectWithList()
        {
            var obj = new ObjectWithList
            {
                List = new List<int>
                {
                    1, 2, 3, 4, 5
                }
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"List\":[1,2,3,4,5]}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTests.ObjectWithList, SFJsonTests\",\"List\":{\"$type\":\"System.Collections.Generic.List`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\",\"$values\":[1,2,3,4,5]}}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithList>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithList>(strDeserialized);
            Assert.AreEqual(obj.List.Count, strDeserialized.List.Count);
            for(int i = 0; i < strDeserialized.List.Count(); i++)
            {
                Assert.AreEqual(obj.List.ElementAt(i), strDeserialized.List.ElementAt(i));
            }
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithList>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithList>(strWithTypeDeserialized);
            Assert.AreEqual(obj.List.Count, strDeserialized.List.Count);
            for(int i = 0; i < strWithTypeDeserialized.List.Count(); i++)
            {
                Assert.AreEqual(obj.List.ElementAt(i), strWithTypeDeserialized.List.ElementAt(i));
            }
        }

        [Test]
        public void CanConvertObjectWithListOfObjects()
        {
            var obj = new ObjectWithListOfObjects()
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
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"List\":[{\"PropBool\":true,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTests.ObjectWithListOfObjects, SFJsonTests\",\"List\":{\"$type\":\"System.Collections.Generic.List`1[[SFJsonTests.PrimitiveHolder, SFJsonTests]], System.Private.CoreLib\",\"$values\":[{\"$type\":\"SFJsonTests.PrimitiveHolder, SFJsonTests\",\"PropBool\":true,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"$type\":\"SFJsonTests.PrimitiveHolder, SFJsonTests\",\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]}}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithListOfObjects>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithListOfObjects>(strDeserialized);
            Assert.AreEqual(obj.List.Count, strDeserialized.List.Count);
            for(int i = 0; i < strDeserialized.List.Count; i++)
            {
                Assert.AreEqual(obj.List[i].PropBool, strDeserialized.List[i].PropBool);
                Assert.AreEqual(obj.List[i].PropDouble, strDeserialized.List[i].PropDouble);
                Assert.AreEqual(obj.List[i].PropFloat, strDeserialized.List[i].PropFloat);
                Assert.AreEqual(obj.List[i].PropInt, strDeserialized.List[i].PropInt);
                Assert.AreEqual(obj.List[i].PropString, strDeserialized.List[i].PropString);
            }
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithListOfObjects>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithListOfObjects>(strWithTypeDeserialized);
            for(int i = 0; i < strWithTypeDeserialized.List.Count; i++)
            {
                Assert.AreEqual(obj.List[i].PropBool, strWithTypeDeserialized.List[i].PropBool);
                Assert.AreEqual(obj.List[i].PropDouble, strWithTypeDeserialized.List[i].PropDouble);
                Assert.AreEqual(obj.List[i].PropFloat, strWithTypeDeserialized.List[i].PropFloat);
                Assert.AreEqual(obj.List[i].PropInt, strWithTypeDeserialized.List[i].PropInt);
                Assert.AreEqual(obj.List[i].PropString, strWithTypeDeserialized.List[i].PropString);
            }
        }
    }
}