using System;
using System.Collections.Generic;
using NUnit.Framework;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Exceptions;
using SFJson.Utils;

namespace SFJsonTest
{
    [TestFixture]
    public class AbstractTests
    {
        [Test]
        public void CanConvertObjectWithAbstractType()
        {
            var obj = new ObjectWithAbstractClass
            {
                ObjectImplementingAbstractClass = new ObjectImplementingAbstractClass()
                {
                    AbstractPropInt = 50,
                    PropInt = 100
                }
            };
            
            var str = Converter.Serialize(obj, new SerializerSettings { FormattedString = true });
            var strWithType = Converter.Serialize(obj, new SerializerSettings { SerializationTypeHandle = SerializationTypeHandle.All, FormattedString = true });

            Console.WriteLine("without type: " + str);
            Console.WriteLine("with type: " + strWithType);
//            Assert.AreEqual("{\"ObjectImplementingAbstractClass\":{\"AbstractPropInt\":50,\"PropInt\":100}}", str);
//            Assert.AreEqual("{\"$type\":\"SFJsonTest.ObjectWithAbstractClass, SFJsonTest\",\"ObjectImplementingAbstractClass\":{\"$type\":\"SFJsonTest.ObjectImplementingAbstractClass, SFJsonTest\",\"AbstractPropInt\":50,\"PropInt\":100}}", strWithType);

            Assert.Throws<DeserializationException>(() =>
            {
                Converter.Deserialize<ObjectWithAbstractClass>(str);
            });
            
            var strWithTypeDeserialized = Converter.Deserialize<ObjectWithAbstractClass>(strWithType);
            Assert.NotNull(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectWithAbstractClass>(strWithTypeDeserialized);
            Assert.IsInstanceOf<ObjectImplementingAbstractClass>(strWithTypeDeserialized.ObjectImplementingAbstractClass);
            Assert.AreEqual(((ObjectImplementingAbstractClass)obj.ObjectImplementingAbstractClass).PropInt, ((ObjectImplementingAbstractClass)strWithTypeDeserialized.ObjectImplementingAbstractClass).PropInt);
            Assert.AreEqual(((ObjectImplementingAbstractClass)obj.ObjectImplementingAbstractClass).AbstractPropInt, ((ObjectImplementingAbstractClass)strWithTypeDeserialized.ObjectImplementingAbstractClass).AbstractPropInt);
        }
    }
}