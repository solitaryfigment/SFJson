using System;
using NUnit.Framework;
using SFJson;

namespace SFJsonTest
{
    [TestFixture]
    public class InheritedTests
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
        public void CanConvertInheritedObjects()
        {
            var obj = new SimpleInheritedObject
            {
                BaseFieldInt = 1,
                BasePropInt = 2,
                FieldString = "String",
                PropString = "String2"
            };

            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings { TypeHandler = TypeHandler.All });
            
            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"FieldString\":\"String\",\"BaseFieldInt\":1,\"PropString\":\"String2\",\"BasePropInt\":2}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.SimpleInheritedObject, SFJsonTest\",\"FieldString\":\"String\",\"BaseFieldInt\":1,\"PropString\":\"String2\",\"BasePropInt\":2}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<SimpleInheritedObject>(str);
            
            Assert.IsTrue(strDeserialized != null);
            Assert.AreEqual(obj.BaseFieldInt, strDeserialized.BaseFieldInt);
            Assert.AreEqual(obj.FieldString, strDeserialized.FieldString);
            Assert.AreEqual(obj.BasePropInt, strDeserialized.BasePropInt);
            Assert.AreEqual(obj.PropString, strDeserialized.PropString);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<SimpleInheritedObject>(strWithType);
            
            Assert.IsTrue(strWithTypeDeserialized != null);
            Assert.AreEqual(obj.BaseFieldInt, strWithTypeDeserialized.BaseFieldInt);
            Assert.AreEqual(obj.FieldString, strWithTypeDeserialized.FieldString);
            Assert.AreEqual(obj.BasePropInt, strWithTypeDeserialized.BasePropInt);
            Assert.AreEqual(obj.PropString, strWithTypeDeserialized.PropString);
        }

        [Test]
        public void OnlyConvertsBaseObjectsWhenAssignedToBaseClass()
        {
            var obj = new ObjectWithMembersOfInheritedClasses
            {
                SimpleBaseObject = new SimpleInheritedObject
                {
                    BaseFieldInt = 1,
                    BasePropInt = 2,
                    FieldString = "String",
                    PropString = "String2"
                }
            };

            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings { TypeHandler = TypeHandler.All });
            
            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"SimpleBaseObject\":{\"FieldString\":\"String\",\"BaseFieldInt\":1,\"PropString\":\"String2\",\"BasePropInt\":2}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithMembersOfInheritedClasses, SFJsonTest\",\"SimpleBaseObject\":{\"$type\":\"SFJsonTest.SimpleInheritedObject, SFJsonTest\",\"FieldString\":\"String\",\"BaseFieldInt\":1,\"PropString\":\"String2\",\"BasePropInt\":2}}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithMembersOfInheritedClasses>(str);
            
            Assert.IsTrue(strDeserialized != null);
            Assert.NotNull(obj.SimpleBaseObject);
            Assert.AreEqual(obj.SimpleBaseObject.BaseFieldInt, strDeserialized.SimpleBaseObject.BaseFieldInt);
            Assert.AreEqual(obj.SimpleBaseObject.BasePropInt, strDeserialized.SimpleBaseObject.BasePropInt);
            Assert.IsNotInstanceOf<SimpleInheritedObject>(strDeserialized.SimpleBaseObject);

            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithMembersOfInheritedClasses>(strWithType);
            
            Assert.IsTrue(strWithTypeDeserialized != null);
            Assert.AreEqual(obj.SimpleBaseObject.BaseFieldInt, strWithTypeDeserialized.SimpleBaseObject.BaseFieldInt);
            Assert.AreEqual(obj.SimpleBaseObject.BasePropInt, strWithTypeDeserialized.SimpleBaseObject.BasePropInt);
            Assert.IsInstanceOf<SimpleInheritedObject>(strWithTypeDeserialized.SimpleBaseObject);
            Assert.AreEqual(((SimpleInheritedObject)obj.SimpleBaseObject).FieldString, ((SimpleInheritedObject)strWithTypeDeserialized.SimpleBaseObject).FieldString);
            Assert.AreEqual(((SimpleInheritedObject)obj.SimpleBaseObject).PropString, ((SimpleInheritedObject)strWithTypeDeserialized.SimpleBaseObject).PropString);
        }
    }
}