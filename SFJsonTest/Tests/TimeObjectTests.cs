using System;
using NUnit.Framework;
using SFJson;

namespace SFJsonTest
{
    [TestFixture]
    public class TimeObjectTests
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
        public void CanConvertDateTimeObjectType()
        {
            var obj = new DateTimeObject()
            {
                DateTimeOffset = new DateTimeOffset(new DateTime(2010, 10, 9), new TimeSpan(1, 1, 0)),
                DateTime = new DateTime(2010, 10, 9)
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            
            Assert.AreEqual("{\"DateTime\":\"10/9/2010 12:00:00 AM\",\"DateTimeOffset\":\"10/9/2010 12:00:00 AM +01:01\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.DateTimeObject, SFJsonTest\",\"DateTime\":\"10/9/2010 12:00:00 AM\",\"DateTimeOffset\":\"10/9/2010 12:00:00 AM +01:01\"}", strWithType);

            var strDeserialized = _deserializer.Deserialize<DateTimeObject>(str);
            Assert.IsInstanceOf<DateTimeObject>(strDeserialized);
            Assert.AreEqual(obj.DateTime, strDeserialized.DateTime);
            Assert.AreEqual(obj.DateTimeOffset, strDeserialized.DateTimeOffset);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<DateTimeObject>(strWithType);
            Assert.IsInstanceOf<DateTimeObject>(strWithTypeDeserialized);
            Assert.AreEqual(obj.DateTime, strWithTypeDeserialized.DateTime);
            Assert.AreEqual(obj.DateTimeOffset, strWithTypeDeserialized.DateTimeOffset);
        }

        [Test]
        public void CanConvertTimeSpanObjectType()
        {
            var obj = new TimeSpanObject()
            {
                TimeSpan = (new DateTime(2010, 10, 9) - new DateTime(2008, 9 , 9))
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            
            Assert.AreEqual("{\"TimeSpan\":\"760.00:00:00\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.TimeSpanObject, SFJsonTest\",\"TimeSpan\":\"760.00:00:00\"}", strWithType);

            var strDeserialized = _deserializer.Deserialize<TimeSpanObject>(str);
            Assert.IsInstanceOf<TimeSpanObject>(strDeserialized);
            Assert.AreEqual(obj.TimeSpan, strDeserialized.TimeSpan);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<TimeSpanObject>(strWithType);
            Assert.IsInstanceOf<TimeSpanObject>(strWithTypeDeserialized);
            Assert.AreEqual(obj.TimeSpan, strWithTypeDeserialized.TimeSpan);
        }
    }
}