using System;
using System.Collections.Generic;
using NUnit.Framework;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Exceptions;
using SFJson.Utils;

namespace SFJsonTest
{
    public class BaseTestClass
    {
        public bool ThrowsException(Type exceptionType, TestDelegate test)
        {
            if(exceptionType != null)
            {
                Assert.Throws(exceptionType, test);
                return true;
            }

            return false;
        }
    }
    
    [TestFixture]
    public class AbstractTests : BaseTestClass
    {
        [TestCase(false, SerializationTypeHandle.None, typeof(DeserializationException), "{\"ObjectImplementingAbstractClass\":{\"AbstractPropInt\":50,\"PropInt\":100}}")]
        [TestCase(false, SerializationTypeHandle.Collections, typeof(DeserializationException), "{\"ObjectImplementingAbstractClass\":{\"AbstractPropInt\":50,\"PropInt\":100}}")]
        [TestCase(false, SerializationTypeHandle.Objects, null, "{\"$type\":\"SFJsonTest.ObjectWithAbstractClass, SFJsonTest\",\"ObjectImplementingAbstractClass\":{\"$type\":\"SFJsonTest.ObjectImplementingAbstractClass, SFJsonTest\",\"AbstractPropInt\":50,\"PropInt\":100}}")]
        [TestCase(false, SerializationTypeHandle.All, null, "{\"$type\":\"SFJsonTest.ObjectWithAbstractClass, SFJsonTest\",\"ObjectImplementingAbstractClass\":{\"$type\":\"SFJsonTest.ObjectImplementingAbstractClass, SFJsonTest\",\"AbstractPropInt\":50,\"PropInt\":100}}")]
        [TestCase(true, SerializationTypeHandle.None, typeof(DeserializationException), "{\n\t\"ObjectImplementingAbstractClass\" : {\n\t\t\"AbstractPropInt\" : 50,\n\t\t\"PropInt\" : 100\n\t}\n}")]
        [TestCase(true, SerializationTypeHandle.Collections, typeof(DeserializationException), "{\n\t\"ObjectImplementingAbstractClass\" : {\n\t\t\"AbstractPropInt\" : 50,\n\t\t\"PropInt\" : 100\n\t}\n}")]
        [TestCase(true, SerializationTypeHandle.Objects, null, "{\n\t\"$type\" : \"SFJsonTest.ObjectWithAbstractClass, SFJsonTest\",\n\t\"ObjectImplementingAbstractClass\" : {\n\t\t\"$type\" : \"SFJsonTest.ObjectImplementingAbstractClass, SFJsonTest\",\n\t\t\"AbstractPropInt\" : 50,\n\t\t\"PropInt\" : 100\n\t}\n}")]
        [TestCase(true, SerializationTypeHandle.All, null, "{\n\t\"$type\" : \"SFJsonTest.ObjectWithAbstractClass, SFJsonTest\",\n\t\"ObjectImplementingAbstractClass\" : {\n\t\t\"$type\" : \"SFJsonTest.ObjectImplementingAbstractClass, SFJsonTest\",\n\t\t\"AbstractPropInt\" : 50,\n\t\t\"PropInt\" : 100\n\t}\n}")]
        public void CanConvertObjectWithAbstractType(bool formatOutput, SerializationTypeHandle serializationTypeHandle, Type exceptionType, string serializedForm)
        {
            var obj = new ObjectWithAbstractClass
            {
                ObjectImplementingAbstractClass = new ObjectImplementingAbstractClass()
                {
                    AbstractPropInt = 50,
                    PropInt = 100
                }
            };
            
            var str = Converter.Serialize(obj, new SerializerSettings { FormattedString = formatOutput, SerializationTypeHandle = serializationTypeHandle });
            Console.WriteLine(str);
            if(!formatOutput)
            {
                Assert.AreEqual(serializedForm, str);
            }

            if(!ThrowsException(exceptionType, () => { Converter.Deserialize<ObjectWithAbstractClass>(str); }))
            {
                var strDeserialized = Converter.Deserialize<ObjectWithAbstractClass>(str);
                Assert.NotNull(strDeserialized);
                Assert.IsInstanceOf<ObjectWithAbstractClass>(strDeserialized);
                Assert.IsInstanceOf<ObjectImplementingAbstractClass>(strDeserialized.ObjectImplementingAbstractClass);
                Assert.AreEqual(((ObjectImplementingAbstractClass)obj.ObjectImplementingAbstractClass).PropInt, ((ObjectImplementingAbstractClass)strDeserialized.ObjectImplementingAbstractClass).PropInt);
                Assert.AreEqual(((ObjectImplementingAbstractClass)obj.ObjectImplementingAbstractClass).AbstractPropInt, ((ObjectImplementingAbstractClass)strDeserialized.ObjectImplementingAbstractClass).AbstractPropInt);
            }
        }
    }
}