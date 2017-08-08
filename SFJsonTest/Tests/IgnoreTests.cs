using System;
using NUnit.Framework;
using SFJson.Conversion;
using SFJson.Utils;

namespace SFJsonTest
{
    [TestFixture]
    public class IgnoreTests
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
        public void WillProperlyHandleSerializingIgnoredMembers()
        {
            var obj = new ObjectWithIgnoredMembers()
            {
                PropInt = 12,
                PropIgnoredInt = 32,
                FieldInt = 13,
                FieldIgnoredInt = 33
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"FieldInt\":13,\"PropInt\":12}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithIgnoredMembers, SFJsonTest\",\"FieldInt\":13,\"PropInt\":12}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithIgnoredMembers>(str);
            var afterstr = _serializer.Serialize(strDeserialized);
            Console.WriteLine(afterstr);
            
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithIgnoredMembers>(strDeserialized);
            Assert.AreEqual(obj.PropInt, strDeserialized.PropInt);
            Assert.AreEqual(obj.FieldInt, strDeserialized.FieldInt);
            Assert.AreEqual(0, strDeserialized.PropIgnoredInt);
            Assert.AreEqual(0, strDeserialized.FieldIgnoredInt);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithIgnoredMembers>(strWithType);
            var afterstrWithType = _serializer.Serialize(strWithTypeDeserialized, new SerializerSettings() { TypeHandler = TypeHandler.All });
            Console.WriteLine(afterstrWithType);
            
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithIgnoredMembers>(strWithTypeDeserialized);
            Assert.AreEqual(obj.PropInt, strWithTypeDeserialized.PropInt);
            Assert.AreEqual(obj.FieldInt, strWithTypeDeserialized.FieldInt);
            Assert.AreEqual(0, strWithTypeDeserialized.PropIgnoredInt);
            Assert.AreEqual(0, strWithTypeDeserialized.FieldIgnoredInt);
        }
        
        [Test]
        public void WillProperlyHandleDeserializingIgnoredMembers()
        {
            var str = "{\"FieldInt\":13,\"PropInt\":12,\"PropIgnoredInt\":32,\"FieldIgnoredInt\":33}";
            var strWithType = "{\"$type\":\"SFJsonTest.ObjectWithIgnoredMembers, SFJsonTest\",\"FieldInt\":13,\"PropInt\":12,\"PropIgnoredInt\":32,\"FieldIgnoredInt\":33}";
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithIgnoredMembers>(str);
            var afterstr = _serializer.Serialize(strDeserialized);
            Console.WriteLine(afterstr);
            
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithIgnoredMembers>(strDeserialized);
            Assert.AreEqual(12, strDeserialized.PropInt);
            Assert.AreEqual(13, strDeserialized.FieldInt);
            Assert.AreEqual(0, strDeserialized.PropIgnoredInt);
            Assert.AreEqual(0, strDeserialized.FieldIgnoredInt);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithIgnoredMembers>(strWithType);
            var afterstrWithType = _serializer.Serialize(strWithTypeDeserialized, new SerializerSettings() { TypeHandler = TypeHandler.All });
            Console.WriteLine(afterstrWithType);
            
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithIgnoredMembers>(strWithTypeDeserialized);
            Assert.AreEqual(12, strWithTypeDeserialized.PropInt);
            Assert.AreEqual(13, strWithTypeDeserialized.FieldInt);
            Assert.AreEqual(0, strWithTypeDeserialized.PropIgnoredInt);
            Assert.AreEqual(0, strWithTypeDeserialized.FieldIgnoredInt);
        }

        [Test]
        public void WillProperlyHandleSerializingIgnoredObjectMembers()
        {
            var obj = new ObjectWithIgnoredObjectMembers()
            {
                PropIgnoredObject = new ObjectWithIgnoredMembers
                {
                    PropInt = 2,
                    PropIgnoredInt = 22,
                    FieldInt = 3,
                    FieldIgnoredInt = 23
                },
                PropObject = new ObjectWithIgnoredMembers
                {
                    PropInt = 12,
                    PropIgnoredInt = 32,
                    FieldInt = 13,
                    FieldIgnoredInt = 33
                },
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"PropObject\":{\"FieldInt\":13,\"PropInt\":12}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithIgnoredObjectMembers, SFJsonTest\",\"PropObject\":{\"$type\":\"SFJsonTest.ObjectWithIgnoredMembers, SFJsonTest\",\"FieldInt\":13,\"PropInt\":12}}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithIgnoredObjectMembers>(str);
            var afterstr = _serializer.Serialize(strDeserialized);
            Console.WriteLine("After: " + afterstr);
            
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithIgnoredObjectMembers>(strDeserialized);
            Assert.NotNull(strDeserialized.PropObject);
            Assert.IsNull(strDeserialized.PropIgnoredObject);
            Assert.IsInstanceOf<ObjectWithIgnoredMembers>(strDeserialized.PropObject);
            Assert.AreEqual(obj.PropObject.PropInt, strDeserialized.PropObject.PropInt);
            Assert.AreEqual(obj.PropObject.FieldInt, strDeserialized.PropObject.FieldInt);
            Assert.AreEqual(0, strDeserialized.PropObject.FieldIgnoredInt);
            Assert.AreEqual(0, strDeserialized.PropObject.PropIgnoredInt);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithIgnoredObjectMembers>(strWithType);
            var afterstrWithType = _serializer.Serialize(strWithTypeDeserialized, new SerializerSettings() { TypeHandler = TypeHandler.All });
            Console.WriteLine(afterstrWithType);
            
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithIgnoredObjectMembers>(strWithTypeDeserialized);
            Assert.NotNull(strWithTypeDeserialized.PropObject);
            Assert.IsNull(strWithTypeDeserialized.PropIgnoredObject);
            Assert.IsInstanceOf<ObjectWithIgnoredMembers>(strWithTypeDeserialized.PropObject);
            Assert.AreEqual(obj.PropObject.PropInt, strWithTypeDeserialized.PropObject.PropInt);
            Assert.AreEqual(obj.PropObject.FieldInt, strWithTypeDeserialized.PropObject.FieldInt);
            Assert.AreEqual(0, strWithTypeDeserialized.PropObject.FieldIgnoredInt);
            Assert.AreEqual(0, strWithTypeDeserialized.PropObject.PropIgnoredInt);
        }
        
        [Test]
        public void WillProperlyHandleDeserializingIgnoredObjectMembers()
        {
            var str = "{\"PropObject\":{\"FieldInt\":13,\"PropInt\":12}}";
            var strWithType = "{\"$type\":\"SFJsonTest.ObjectWithIgnoredObjectMembers, SFJsonTest\",\"PropObject\":{\"$type\":\"SFJsonTest.ObjectWithIgnoredMembers, SFJsonTest\",\"FieldInt\":13,\"PropInt\":12}}";
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithIgnoredObjectMembers>(str);
            var afterstr = _serializer.Serialize(strDeserialized);
            Console.WriteLine(afterstr);
            
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectWithIgnoredObjectMembers>(strDeserialized);
            Assert.NotNull(strDeserialized.PropObject);
            Assert.IsNull(strDeserialized.PropIgnoredObject);
            Assert.IsInstanceOf<ObjectWithIgnoredMembers>(strDeserialized.PropObject);
            Assert.AreEqual(12, strDeserialized.PropObject.PropInt);
            Assert.AreEqual(13, strDeserialized.PropObject.FieldInt);
            Assert.AreEqual(0, strDeserialized.PropObject.FieldIgnoredInt);
            Assert.AreEqual(0, strDeserialized.PropObject.PropIgnoredInt);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithIgnoredObjectMembers>(strWithType);
            var afterstrWithType = _serializer.Serialize(strWithTypeDeserialized, new SerializerSettings() { TypeHandler = TypeHandler.All });
            Console.WriteLine(afterstrWithType);
            
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithIgnoredObjectMembers>(strWithTypeDeserialized);
            Assert.NotNull(strWithTypeDeserialized.PropObject);
            Assert.IsNull(strWithTypeDeserialized.PropIgnoredObject);
            Assert.IsInstanceOf<ObjectWithIgnoredMembers>(strWithTypeDeserialized.PropObject);
            Assert.AreEqual(12, strWithTypeDeserialized.PropObject.PropInt);
            Assert.AreEqual(13, strWithTypeDeserialized.PropObject.FieldInt);
            Assert.AreEqual(0, strWithTypeDeserialized.PropObject.FieldIgnoredInt);
            Assert.AreEqual(0, strWithTypeDeserialized.PropObject.PropIgnoredInt);
        }
    }
}