using System;
using NUnit.Framework;
using SFJson;
using SFJson.Conversion;
using SFJson.Utils;

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
                DateTimeOffset = new DateTimeOffset(new DateTime(2010, 10, 9, 1, 2, 3, 500), new TimeSpan(1, 1, 0)),
                DateTime = new DateTime(2010, 10, 9, 1, 2, 3, 500)
            };
            
            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            
            Assert.AreEqual("{\"DateTime\":\"2010-10-09T01:02:03.500\",\"DateTimeOffset\":\"2010-10-09T01:02:03.500 +01:01\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.DateTimeObject, SFJsonTest\",\"DateTime\":\"2010-10-09T01:02:03.500\",\"DateTimeOffset\":\"2010-10-09T01:02:03.500 +01:01\"}", strWithType);

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
        public void CanConvertDateTimeObjectTypeWithFormatting()
        {
            var obj = new DateTimeObject()
            {
                DateTimeOffset = new DateTimeOffset(new DateTime(2010, 10, 9, 1, 2, 3, 500), new TimeSpan(1, 1, 0)),
                DateTime = new DateTime(2010, 10, 9, 1, 2, 3, 500)
            };
            
            var str = _serializer.Serialize(obj, new SerializerSettings() { DateTimeFormat = "yyyy-MM-dd", DateTimeOffsetFormat = "yyyy-MM-dd zzz" });
            var strWithType = _serializer.Serialize(obj, new SerializerSettings() { TypeHandler = TypeHandler.All, DateTimeFormat = "yyyy-MM-dd", DateTimeOffsetFormat = "yyyy-MM-dd zzz" });

            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            
            Assert.AreEqual("{\"DateTime\":\"2010-10-09\",\"DateTimeOffset\":\"2010-10-09 +01:01\"}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTest.DateTimeObject, SFJsonTest\",\"DateTime\":\"2010-10-09\",\"DateTimeOffset\":\"2010-10-09 +01:01\"}", strWithType);

            var testDateTime = new DateTime(obj.DateTime.Year, obj.DateTime.Month, obj.DateTime.Day);
            var strDeserialized = _deserializer.Deserialize<DateTimeObject>(str);
            Assert.IsInstanceOf<DateTimeObject>(strDeserialized);
            Assert.AreNotEqual(obj.DateTime, strDeserialized.DateTime);
            Assert.AreNotEqual(obj.DateTimeOffset, strDeserialized.DateTimeOffset);
            Assert.AreEqual(testDateTime, strDeserialized.DateTime);
            Assert.AreEqual(new DateTimeOffset(testDateTime, obj.DateTimeOffset.Offset), strDeserialized.DateTimeOffset);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<DateTimeObject>(strWithType);
            Assert.IsInstanceOf<DateTimeObject>(strWithTypeDeserialized);
            Assert.AreNotEqual(obj.DateTime, strWithTypeDeserialized.DateTime);
            Assert.AreNotEqual(obj.DateTimeOffset.Offset, strWithTypeDeserialized.DateTimeOffset);
            Assert.AreEqual(testDateTime, strWithTypeDeserialized.DateTime);
            Assert.AreEqual(new DateTimeOffset(testDateTime, obj.DateTimeOffset.Offset), strWithTypeDeserialized.DateTimeOffset);
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