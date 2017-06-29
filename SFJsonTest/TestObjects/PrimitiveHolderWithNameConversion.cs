using Newtonsoft.Json;
using SFJson.Attributes;

namespace SFJsonTest
{
    public class PrimitiveHolderWithNameConversion
    {
        [JsonValueName("b")]
        [JsonProperty("b")]
        public bool PropBool { get; set; }
        [JsonValueName("d")]
        [JsonProperty("d")]
        public double PropDouble { get; set; }
        [JsonValueName("f")]
        [JsonProperty("f")]
        public float PropFloat { get; set; }
        [JsonValueName("i")]
        [JsonProperty("i")]
        public int PropInt { get; set; }
        [JsonValueName("s")]
        [JsonProperty("s")]
        public string PropString { get; set; }
    }
}