using System;
using NUnit.Framework;
using SFJson.Conversion;
using SFJson.Exceptions;

namespace SFJsonTests
{
    [TestFixture]
    public class ErrorTests
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
        public void InvalidJsonThrowsTokenizationException()
        {
            var str = "{\"Thingy\":12]";

            Assert.Throws<TokenizationException>(() =>
            {
                _deserializer.Deserialize<SimpleObject>(str);
            });
        }

        [Test]
        public void InvalidObjectThrowsDeserializationException()
        {
            var str = "{\"String\":{\"String\":\"string\"}}";
            
            Assert.Throws<DeserializationException>(() =>
            {
                try
                {
                    _deserializer.Deserialize<SimpleObject>(str);
                }
                catch (DeserializationException e)
                {
                    Console.WriteLine(e.Message);
                    Console.WriteLine(e.InnerException);
                    Assert.AreEqual(1, e.Token.Children.Count);
                    Assert.AreEqual("string", e.Token.Children[0].Children[0].GetValue<string>());
                    throw;
                }
            });
        }
    }
}