using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using SFJson.Conversion;
using SFJson.Exceptions;

namespace SFJsonTest
{
    [TestFixture]
    public class CompatabilityTests
    {
        private Serializer _serializer;
        private Deserializer _deserializer;

        [OneTimeSetUp]
        public void Init()
        {
            _deserializer = new Deserializer();
            _serializer = new Serializer();
        }

        // TODO: More tests: 
        //    IList
        //    IEnumerable
        //    Random interface type
        [Test]
        public void IDictionaryTest()
        {
            IDictionary obj = new Dictionary<int, SimpleBaseObject>
            {
                {1, new SimpleBaseObject { BaseFieldInt = 1, BasePropInt = 3 }},
                {2, new SimpleBaseObject { BaseFieldInt = 2, BasePropInt = 4 }}
            };
            
            var str = _serializer.Serialize(obj);
            
            Console.WriteLine(str);

            IDictionary strDeserialized = _deserializer.Deserialize<IDictionary>(str);

            var reserialized = _serializer.Serialize(strDeserialized);
            Console.WriteLine(reserialized);
            Assert.AreEqual("{\"1\":{\"BaseFieldInt\":\"1\",\"BasePropInt\":\"3\"},\"2\":{\"BaseFieldInt\":\"2\",\"BasePropInt\":\"4\"}}", reserialized);
        }
        
        [Test]
        public void IListTest()
        {
            IList obj = new List<SimpleBaseObject>
            {
                new SimpleBaseObject { BaseFieldInt = 1, BasePropInt = 3 },
                new SimpleBaseObject { BaseFieldInt = 2, BasePropInt = 4 }
            };
            
            var str = _serializer.Serialize(obj);
            
            Console.WriteLine(str);

            IList strDeserialized = _deserializer.Deserialize<IList>(str);

            var reserialized = _serializer.Serialize(strDeserialized);
            Console.WriteLine(reserialized);
            Assert.AreEqual("[{\"BaseFieldInt\":\"1\",\"BasePropInt\":\"3\"},{\"BaseFieldInt\":\"2\",\"BasePropInt\":\"4\"}]", reserialized);
        }
        
        [Test]
        public void LoneInterfaceFailsTest()
        {
            var obj = new SimpleBaseObject {BaseFieldInt = 1, BasePropInt = 3};
            
            var str = _serializer.Serialize(obj);
            
            Console.WriteLine(str);

            Assert.Throws<DeserializationException>(() =>
                {
                    ISimpleBaseObject strDeserialized = _deserializer.Deserialize<ISimpleBaseObject>(str);
                });
        }
        
        [Test]
        public void IEnumerableTest()
        {
            Stack<SimpleBaseObject> obj = new Stack<SimpleBaseObject>();
            obj.Push(new SimpleBaseObject {BaseFieldInt = 1, BasePropInt = 3});
            obj.Push(new SimpleBaseObject {BaseFieldInt = 2, BasePropInt = 4});
            
            var str = _serializer.Serialize(obj);
            
            Console.WriteLine(str);

            IEnumerable strDeserialized = _deserializer.Deserialize<IEnumerable>(str);

            var reserialized = _serializer.Serialize(strDeserialized);
            Console.WriteLine(reserialized);
            Assert.AreEqual("[{\"BaseFieldInt\":\"2\",\"BasePropInt\":\"4\"},{\"BaseFieldInt\":\"1\",\"BasePropInt\":\"3\"}]", reserialized);
        }
    }
}
