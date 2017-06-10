using System;
using NUnit.Framework;
using SFJson;

namespace SFJsonTest
{
    [TestFixture]
    public class InterfaceTests
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
        public void CanConvertObjectWithInterface()
        {
            var obj = new ObjectWithInterface()
            {
                ObjectImplementingInterface = new ObjectImplementingInterface()
                {
                    InterfacePropInt = 50,
                    PropInt = 100
                }
            };

            _serializer.ObjectToSerialize = obj;
            var str = _serializer.Serialize();
            var strWithType = _serializer.Serialize(new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"ObjectImplementingInterface\":{\"InterfacePropInt\":50,\"PropInt\":100}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithInterface, SFJsonTest\",\"ObjectImplementingInterface\":{\"$type\":\"SFJsonTest.ObjectImplementingInterface, SFJsonTest\",\"InterfacePropInt\":50,\"PropInt\":100}}", strWithType);

            _deserializer.StringToDeserialize = str;
            Assert.Throws<MissingMethodException>(() =>
            {
                _deserializer.Deserialize<ObjectWithInterface>();
            });
            
            _deserializer.StringToDeserialize = strWithType;
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithInterface>();
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithInterface>(strWithTypeDeserialized);
            Assert.AreEqual(obj.ObjectImplementingInterface.InterfacePropInt, strWithTypeDeserialized.ObjectImplementingInterface.InterfacePropInt);
            Assert.IsInstanceOf<ObjectImplementingInterface>(strWithTypeDeserialized.ObjectImplementingInterface);
            Assert.AreEqual(((ObjectImplementingInterface)obj.ObjectImplementingInterface).PropInt, ((ObjectImplementingInterface)strWithTypeDeserialized.ObjectImplementingInterface).PropInt);
        }
        
        [Test]
        public void CanConvertInterface()
        {
            var obj = new ObjectImplementingInterface
            {
                InterfacePropInt = 50,
                PropInt = 100
            };

            _serializer.ObjectToSerialize = obj;
            var str = _serializer.Serialize();
            var strWithType = _serializer.Serialize(new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"InterfacePropInt\":50,\"PropInt\":100}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectImplementingInterface, SFJsonTest\",\"InterfacePropInt\":50,\"PropInt\":100}", strWithType);

            _deserializer.StringToDeserialize = str;
            var strDeserialized = _deserializer.Deserialize<ObjectImplementingInterface>();
            Assert.NotNull(strDeserialized);
            Assert.IsInstanceOf<ObjectImplementingInterface>(strDeserialized);
            Assert.AreEqual(obj.InterfacePropInt, strDeserialized.InterfacePropInt);
            Assert.AreEqual(obj.PropInt, strDeserialized.PropInt);
            
            _deserializer.StringToDeserialize = strWithType;
            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectImplementingInterface>();
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectImplementingInterface>(strWithTypeDeserialized);
            Assert.AreEqual(obj.InterfacePropInt, strWithTypeDeserialized.InterfacePropInt);
            Assert.AreEqual(obj.PropInt, strWithTypeDeserialized.PropInt);
        }
    }
}