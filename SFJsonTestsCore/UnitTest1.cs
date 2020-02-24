using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using NUnit.Framework;
using SFJson.Conversion;
using SFJson.Utils;

namespace SFJsonTestsCore
{
    public class ASPNetCoreTests
    {
        private IHeaderDictionary _headerDictionary;
        
        [SetUp]
        public void Setup()
        {
            _headerDictionary = new HeaderDictionary();
            _headerDictionary["first"] = new StringValues("One-String");
            _headerDictionary["second"] = new StringValues(new string[]{"More","Than","One","String"});
        }

        [TestCase(@"{""first"":[""One-String""],""second"":[""More"",""Than"",""One"",""String""]}")]
        public void CanSerializeASPNetCoreContextDictionary(string serialized)
        {
            Assert.AreEqual(serialized,Converter.Serialize(_headerDictionary));
        }

        [TestCase(@"{""first"":[""One-String""],""second"":[""More"",""Than"",""One"",""String""]}")]
        public void CanDeserializeASPNetCoreContextDictionary(string serialized)
        {
            Console.WriteLine(serialized);
            IHeaderDictionary header = Converter.Deserialize<HeaderDictionary>(serialized);
            Assert.AreEqual(serialized,Converter.Serialize(header));
        }
    }
}
