using Newtonsoft.Json;
using SFJson.Attributes;

namespace SFJsonTest
{
    public class PrimitiveHolderWithNameConversion
    {
        [JsonNamedValue("b")]
        [JsonProperty("b")]
        public bool PropBool { get; set; }
        [JsonNamedValue("d")]
        [JsonProperty("d")]
        public double PropDouble { get; set; }
        [JsonNamedValue("f")]
        [JsonProperty("f")]
        public float PropFloat { get; set; }
        [JsonNamedValue("i")]
        [JsonProperty("i")]
        public int PropInt { get; set; }
        [JsonNamedValue("s")]
        [JsonProperty("s")]
        public string PropString { get; set; }
    }
}