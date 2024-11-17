using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Utils;

namespace SFJsonTests
{
    [TestFixture]
    public class StackTests
    {
        private Serializer _serializer;
        private Deserializer _deserializer;

        [OneTimeSetUp]
        public void Init()
        {
            _deserializer = new Deserializer();
            _serializer = new Serializer();
        }

        [TestCase(false)]
        [TestCase(true)]
        public void Queue(bool formatOutput)
        {
            var obj = new Queue<int>();
            obj.Enqueue(1);
            obj.Enqueue(2);
            obj.Enqueue(3);
            obj.Enqueue(4);
            obj.Enqueue(5);

            var str = _serializer.Serialize(obj, new SerializerSettings() { FormattedString = formatOutput});
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { FormattedString = formatOutput, SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);

            if(!formatOutput)
            {
                Assert.AreEqual("[1,2,3,4,5]", str);
                Assert.AreEqual("{\"$type\":\"System.Collections.Generic.Queue`1[[System.Int32, System.Private.CoreLib]], System.Private.CoreLib\",\"$values\":[1,2,3,4,5]}", strWithType);
            }

            var strDeserialized = _deserializer.Deserialize<Queue<int>>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<Queue<int>>(strDeserialized);
            Assert.AreEqual(obj.Count, strDeserialized.Count);
            for(int i = 0; i < strDeserialized.Count; i++)
            {
                Assert.AreEqual(obj.ElementAt(i), strDeserialized.ElementAt(i));
            }
            
            var strWithTypeDeserialized = _deserializer.Deserialize<Queue<int>>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<Queue<int>>(strWithTypeDeserialized);
            Assert.AreEqual(obj.Count, strWithTypeDeserialized.Count);
            for(int i = 0; i < strWithTypeDeserialized.Count; i++)
            {
                Assert.AreEqual(obj.ElementAt(i), strWithTypeDeserialized.ElementAt(i));
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void CanDeserializeStackOfLists(bool formatOutput)
        {
            var obj = new Stack<List<int>>();
            obj.Push(new List<int>{1,2,3});
            obj.Push(new List<int>{1,2,3});
            obj.Push(new List<int>{1,2,3});
            obj.Push(new List<int>{1,2,3});
            obj.Push(new List<int>{1,2,3});

            var str = _serializer.Serialize(obj, new SerializerSettings() { FormattedString = formatOutput});
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { FormattedString = formatOutput, SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);

            var strDeserialized = _deserializer.Deserialize<Stack<List<int>>>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<Stack<List<int>>>(strDeserialized);
            Assert.AreEqual(obj.Count, strDeserialized.Count);
            for(int i = 0; i < strDeserialized.Count; i++)
            {
                Assert.AreEqual(obj.ElementAt(i).Count, strDeserialized.ElementAt(i).Count);
                for(int j = 0; j < obj.ElementAt(j).Count; j++)
                {
                    Assert.AreEqual(obj.ElementAt(i)[j], strDeserialized.ElementAt(i)[j]);
                }
            }
            
            var strWithTypeDeserialized = _deserializer.Deserialize<Stack<List<int>>>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<Stack<List<int>>>(strWithTypeDeserialized);
            Assert.AreEqual(obj.Count, strWithTypeDeserialized.Count);
            for(int i = 0; i < strWithTypeDeserialized.Count; i++)
            {
                Assert.AreEqual(obj.ElementAt(i).Count, strWithTypeDeserialized.ElementAt(i).Count);
                for(int j = 0; j < obj.ElementAt(j).Count; j++)
                {
                    Assert.AreEqual(obj.ElementAt(i)[j], strWithTypeDeserialized.ElementAt(i)[j]);
                }
            }
        }

        [TestCase(false)]
        [TestCase(true)]
        public void CanDeserializeEmptyObjectIntoEmptyArray(bool formatOutput)
        {
            var obj = new Stack<int>();
            obj.Push(1);
            obj.Push(2);
            obj.Push(3);
            obj.Push(4);
            obj.Push(5);

            var str = _serializer.Serialize(obj, new SerializerSettings() { FormattedString = formatOutput});
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { FormattedString = formatOutput, SerializationTypeHandle = SerializationTypeHandle.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);

            if(!formatOutput)
            {
                Assert.AreEqual("[5,4,3,2,1]", str);
                Assert.AreEqual("{\"$type\":\"System.Collections.Generic.Stack`1[[System.Int32, System.Private.CoreLib]], System.Collections\",\"$values\":[5,4,3,2,1]}", strWithType);
            }

            var strDeserialized = _deserializer.Deserialize<Stack<int>>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<Stack<int>>(strDeserialized);
            Assert.AreEqual(obj.Count, strDeserialized.Count);
            for(int i = 0; i < strDeserialized.Count; i++)
            {
                Assert.AreEqual(obj.ElementAt(i), strDeserialized.ElementAt(i));
            }
            
            var strWithTypeDeserialized = _deserializer.Deserialize<Stack<int>>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<Stack<int>>(strWithTypeDeserialized);
            Assert.AreEqual(obj.Count, strWithTypeDeserialized.Count);
            for(int i = 0; i < strWithTypeDeserialized.Count; i++)
            {
                Assert.AreEqual(obj.ElementAt(i), strWithTypeDeserialized.ElementAt(i));
            }
        }
    }

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
        [TestCase("{\"Array\":{}}")]
        [TestCase("{\"Array\":{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":{}}}")]
        [TestCase("{\"$type\":\"SFJsonTests.ObjectWithArray, SFJsonTests\",\"Array\":{}}")]
        [TestCase("{\"$type\":\"SFJsonTests.ObjectWithArray, SFJsonTests\",\"Array\":{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":{}}}")]
        // TODO: format output 
        [TestCase("{\n\t\"Array\" : {\n\t}\n}")]
        [TestCase("{\n\t\"Array\" : {\n\t\t\"$type\" : \"System.Int32[], System.Private.CoreLib\",\n\t\t\"$values\":{\n\t\t}\n\t}\n}")]
        [TestCase("{\n\t\"$type\" : \"SFJsonTests.ObjectWithArray, SFJsonTests\",\n\t\"Array\" : {\n\t}\n}")]
        [TestCase("{\n\t\"$type\" : \"SFJsonTests.ObjectWithArray, SFJsonTests\",\n\t\"Array\":{\n\t\t\"$type\" : \"System.Int32[], System.Private.CoreLib\",\n\t\t\"$values\":{\n\t\t}\n\t}\n}")]
        
        public void CanDeserializeEmptyObjectIntoEmptyArray(string serializedForm)
        {
            Console.WriteLine(serializedForm);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithArray>(serializedForm);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithArray>(strDeserialized);
            Assert.NotNull(strDeserialized.Array);
            Assert.AreEqual(0, strDeserialized.Array.Length);
        }
        
        [TestCase("{\"Array\":[]}")]
        [TestCase("{\"Array\":{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":[]}}")]
        [TestCase("{\"$type\":\"SFJsonTests.ObjectWithArray, SFJsonTests\",\"Array\":[]}")]
        [TestCase("{\"$type\":\"SFJsonTests.ObjectWithArray, SFJsonTests\",\"Array\":{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":[]}}")]
        // TODO: format output 
        [TestCase("{\n\t\"Array\" : [\n\t]\n}")]
        [TestCase("{\n\t\"Array\" : {\n\t\t\"$type\" : \"System.Int32[], System.Private.CoreLib\",\n\t\t\"$values\":[\n\t\t]\n\t}\n}")]
        [TestCase("{\n\t\"$type\" : \"SFJsonTests.ObjectWithArray, SFJsonTests\",\n\t\"Array\" : [\n\t]\n}")]
        [TestCase("{\n\t\"$type\" : \"SFJsonTests.ObjectWithArray, SFJsonTests\",\n\t\"Array\":{\n\t\t\"$type\" : \"System.Int32[], System.Private.CoreLib\",\n\t\t\"$values\":[\n\t\t]\n\t}\n}")]
        public void CanDeserializeEmptyArrayIntoEmptyArray(string serializedForm)
        {
            Console.WriteLine(serializedForm);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithArray>(serializedForm);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithArray>(strDeserialized);
            Assert.NotNull(strDeserialized.Array);
            Assert.AreEqual(0, strDeserialized.Array.Length);
        }
        
        [TestCase(false, SerializationTypeHandle.None, "{\"Array\":[1,2,3,4,5]}")]
        [TestCase(false, SerializationTypeHandle.Objects, "{\"$type\":\"SFJsonTests.ObjectWithArray, SFJsonTests\",\"Array\":[1,2,3,4,5]}")]
        [TestCase(false, SerializationTypeHandle.Collections, "{\"Array\":{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":[1,2,3,4,5]}}")]
        [TestCase(false, SerializationTypeHandle.All, "{\"$type\":\"SFJsonTests.ObjectWithArray, SFJsonTests\",\"Array\":{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":[1,2,3,4,5]}}")]
        // TODO: format output 
        [TestCase(true, SerializationTypeHandle.None, "{\"Array\":[1,2,3,4,5]}")]
        [TestCase(true, SerializationTypeHandle.Objects, "{\"$type\":\"SFJsonTests.ObjectWithArray, SFJsonTests\",\"Array\":[1,2,3,4,5]}")]
        [TestCase(true, SerializationTypeHandle.Collections, "{\"Array\":{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":[1,2,3,4,5]}}")]
        [TestCase(true, SerializationTypeHandle.All, "{\"$type\":\"SFJsonTests.ObjectWithArray, SFJsonTests\",\"Array\":{\"$type\":\"System.Int32[], System.Private.CoreLib\",\"$values\":[1,2,3,4,5]}}")]
        public void CanConvertObjectWithArray(bool formatOutput, SerializationTypeHandle serializationTypeHandle, string serializedForm)
        {
            var obj = new ObjectWithArray()
            {
                Array = new int[]
                {
                    1, 2, 3, 4, 5
                }
            };
            
            var str = _serializer.Serialize(obj, new SerializerSettings() { FormattedString = formatOutput, SerializationTypeHandle = serializationTypeHandle });

            Console.WriteLine(str);

            if(!formatOutput)
            {
                Assert.AreEqual(serializedForm, str);
            }

            var strDeserialized = _deserializer.Deserialize<ObjectWithArray>(str);
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithArray>(strDeserialized);
            Assert.AreEqual(obj.Array.Length, strDeserialized.Array.Length);
            for(int i = 0; i < strDeserialized.Array.Length; i++)
            {
                Assert.AreEqual(obj.Array[i], strDeserialized.Array[i]);
            }
        }

        [TestCase(false, SerializationTypeHandle.None, "{\"Array\":[{\"PropBool\":true,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]}")]
        [TestCase(false, SerializationTypeHandle.Objects, "{\"$type\":\"SFJsonTests.ObjectWithArrayOfObjects, SFJsonTests\",\"Array\":[{\"$type\":\"SFJsonTests.PrimitiveHolder, SFJsonTests\",\"PropBool\":true,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"$type\":\"SFJsonTests.PrimitiveHolder, SFJsonTests\",\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]}")]
        [TestCase(false, SerializationTypeHandle.Collections, "{\"Array\":{\"$type\":\"SFJsonTests.PrimitiveHolder[], SFJsonTests\",\"$values\":[{\"PropBool\":true,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]}}")]
        [TestCase(false, SerializationTypeHandle.All, "{\"$type\":\"SFJsonTests.ObjectWithArrayOfObjects, SFJsonTests\",\"Array\":{\"$type\":\"SFJsonTests.PrimitiveHolder[], SFJsonTests\",\"$values\":[{\"$type\":\"SFJsonTests.PrimitiveHolder, SFJsonTests\",\"PropBool\":true,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"$type\":\"SFJsonTests.PrimitiveHolder, SFJsonTests\",\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]}}")]
        // TODO: format output 
        [TestCase(true, SerializationTypeHandle.None, "{\"Array\":[{\"PropBool\":true,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]}")]
        [TestCase(true, SerializationTypeHandle.Objects, "{\"$type\":\"SFJsonTests.ObjectWithArrayOfObjects, SFJsonTests\",\"Array\":[{\"$type\":\"SFJsonTests.PrimitiveHolder, SFJsonTests\",\"PropBool\":true,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"$type\":\"SFJsonTests.PrimitiveHolder, SFJsonTests\",\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]}")]
        [TestCase(true, SerializationTypeHandle.Collections, "{\"Array\":{\"$type\":\"SFJsonTests.PrimitiveHolder[], SFJsonTests\",\"$values\":[{\"PropBool\":true,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]}}")]
        [TestCase(true, SerializationTypeHandle.All, "{\"$type\":\"SFJsonTests.ObjectWithArrayOfObjects, SFJsonTests\",\"Array\":{\"$type\":\"SFJsonTests.PrimitiveHolder[], SFJsonTests\",\"$values\":[{\"$type\":\"SFJsonTests.PrimitiveHolder, SFJsonTests\",\"PropBool\":true,\"PropDouble\":100.2,\"PropFloat\":1.2,\"PropInt\":26,\"PropString\":\"String\"},{\"$type\":\"SFJsonTests.PrimitiveHolder, SFJsonTests\",\"PropBool\":false,\"PropDouble\":100.1,\"PropFloat\":1.1,\"PropInt\":25,\"PropString\":\"String2\"}]}}")]
        public void CanConvertObjectWithArrayOfObjects(bool formatOutput, SerializationTypeHandle serializationTypeHandle, string serializedForm)
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
            
            var str = _serializer.Serialize(obj, new SerializerSettings() { FormattedString = formatOutput, SerializationTypeHandle = serializationTypeHandle });

            Console.WriteLine(str);

            if(!formatOutput)
            {
                Assert.AreEqual(serializedForm, str);
            }

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
        }
    }
}