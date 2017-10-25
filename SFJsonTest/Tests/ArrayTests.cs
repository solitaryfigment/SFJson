using System;
using NUnit.Framework;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Utils;

namespace SFJsonTest
{
    [TestFixture]
    public class ArrayTests
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
        public void CanDeserializeEmptyObjectIntoEmptyArray()
        {
            var str = "{\"Array\":{}}";
            var strWithType = "{\"$type\":\"SFJsonTest.ObjectWithArray, SFJsonTest\",\"Array\":{\"$type\":\"System.Int32[], mscorlib\",\"$values\":[]}}";
            
            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithArray>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithArray>(strDeserialized);
            Assert.NotNull(strDeserialized.Array);
            Assert.AreEqual(0, strDeserialized.Array.Length);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithArray>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithArray>(strWithTypeDeserialized);
            Assert.NotNull(strWithTypeDeserialized.Array);
            Assert.AreEqual(0, strWithTypeDeserialized.Array.Length);
        }
        
        [Test]
        public void CanDeserializeEmptyArrayIntoEmptyArray()
        {
            var str = "{\"Array\":[]}";
            var strWithType = "{\"$type\":\"SFJsonTest.ObjectWithArray, SFJsonTest\",\"Array\":{\"$type\":\"System.Int32[], mscorlib\",\"$values\":[]}}";
            
            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithArray>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithArray>(strDeserialized);
            Assert.NotNull(strDeserialized.Array);
            Assert.AreEqual(0, strDeserialized.Array.Length);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithArray>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithArray>(strWithTypeDeserialized);
            Assert.NotNull(strWithTypeDeserialized.Array);
            Assert.AreEqual(0, strWithTypeDeserialized.Array.Length);
        }
        
        [Test]
        public void CanConvertObjectWithArray()
        {
            var obj = new ObjectWithArray()
            {
                Array = new int[]
                {
                    1, 2, 3, 4, 5
                }
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"Array\":[1,2,3,4,5]}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithArray, SFJsonTest\",\"Array\":{\"$type\":\"System.Int32[], mscorlib\",\"$values\":[1,2,3,4,5]}}", strWithType);

            var strDeserialized = _deserializer.Deserialize<ObjectWithArray>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithArray>(strDeserialized);
            Assert.AreEqual(obj.Array.Length, strDeserialized.Array.Length);
            for(int i = 0; i < strDeserialized.Array.Length; i++)
            {
                Assert.AreEqual(obj.Array[i], strDeserialized.Array[i]);
            }
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithArray>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithArray>(strWithTypeDeserialized);
            for(int i = 0; i < strWithTypeDeserialized.Array.Length; i++)
            {
                Assert.AreEqual(obj.Array[i], strWithTypeDeserialized.Array[i]);
            }
        }

        [Test]
        public void CanConvertObjectWithArrayOfObjects()
        {
            var obj = new ObjectWithArrayOfObjects()
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
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"Array\":[{\"PropBool\":true,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithArrayOfObjects, SFJsonTest\",\"Array\":{\"$type\":\"SFJsonTest.PrimitiveHolder[], SFJsonTest\",\"$values\":[{\"$type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":true,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"$type\":\"SFJsonTest.PrimitiveHolder, SFJsonTest\",\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]}}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithArrayOfObjects>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithArrayOfObjects>(strDeserialized);
            Assert.AreEqual(obj.Array.Length, strDeserialized.Array.Length);
            for(int i = 0; i < strDeserialized.Array.Length; i++)
            {
                Assert.AreEqual(obj.Array[i].PropBool, strDeserialized.Array[i].PropBool);
                Assert.AreEqual(obj.Array[i].PropDouble, strDeserialized.Array[i].PropDouble);
                Assert.AreEqual(obj.Array[i].PropFloat, strDeserialized.Array[i].PropFloat);
                Assert.AreEqual(obj.Array[i].PropInt, strDeserialized.Array[i].PropInt);
                Assert.AreEqual(obj.Array[i].PropString, strDeserialized.Array[i].PropString);
            }
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithArrayOfObjects>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithArrayOfObjects>(strWithTypeDeserialized);
            Assert.AreEqual(obj.Array.Length, strWithTypeDeserialized.Array.Length);
            for(int i = 0; i < strWithTypeDeserialized.Array.Length; i++)
            {
                Assert.AreEqual(obj.Array[i].PropBool, strWithTypeDeserialized.Array[i].PropBool);
                Assert.AreEqual(obj.Array[i].PropDouble, strWithTypeDeserialized.Array[i].PropDouble);
                Assert.AreEqual(obj.Array[i].PropFloat, strWithTypeDeserialized.Array[i].PropFloat);
                Assert.AreEqual(obj.Array[i].PropInt, strWithTypeDeserialized.Array[i].PropInt);
                Assert.AreEqual(obj.Array[i].PropString, strWithTypeDeserialized.Array[i].PropString);
            }
        }
    }
}