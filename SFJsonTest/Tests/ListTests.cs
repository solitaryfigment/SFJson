using System;
using System.Collections.Generic;
using NUnit.Framework;
using SFJson;
using SFJson.Conversion;
using SFJson.Utils;

namespace SFJsonTest
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
        public void CanConvertObjectWithList()
        {
            var obj = new ObjectWithList()
            {
                List = new List<int>()
                {
                    1, 2, 3, 4, 5
                }
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"List\":[1,2,3,4,5]}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithList, SFJsonTest\",\"List\":{\"$type\":\"System.Collections.Generic.List`1[[System.Int32, mscorlib]], mscorlib\",\"$values\":[1,2,3,4,5]}}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithList>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithList>(strDeserialized);
            Assert.AreEqual(obj.List.Count, strDeserialized.List.Count);
            for(int i = 0; i < strDeserialized.List.Count; i++)
            {
                Assert.AreEqual(obj.List[i], strDeserialized.List[i]);
            }
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithList>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithList>(strWithTypeDeserialized);
            for(int i = 0; i < strWithTypeDeserialized.List.Count; i++)
            {
                Assert.AreEqual(obj.List[i], strWithTypeDeserialized.List[i]);
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
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"List\":[{\"PropBool\":true,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithListOfObjects, SFJsonTest\",\"List\":{\"$type\":\"System.Collections.Generic.List`1[[SFJsonTest.PrimitiveHolder, SFJsonTest]], mscorlib\",\"$values\":[{\"$type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":true,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"$type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]}}", strWithType);
            
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