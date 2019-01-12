using SFJson.Attributes;

namespace SFJsonTest
{
    public class PrimitiveHolderWithNameConversion
    {
        [JsonNamedValue("b")]
        public bool PropBool { get; set; }
        [JsonNamedValue("d")]
        public double PropDouble { get; set; }
        [JsonNamedValue("f")]
        public float PropFloat { get; set; }
        [JsonNamedValue("i")]
        public int PropInt { get; set; }
        [JsonNamedValue("s")]
        public string PropString { get; set; }
    }
}