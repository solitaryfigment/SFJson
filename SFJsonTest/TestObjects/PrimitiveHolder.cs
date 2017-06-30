using System;

namespace SFJsonTest
{
    public class PrimitiveHolder
    {
        public bool PropBool { get; set; }
        public double PropDouble { get; set; }
        public float PropFloat { get; set; }
        public int PropInt { get; set; }
        public string PropString { get; set; }
    }
    
    public class PrimitiveHolder2
    {
        public decimal PropDecimal { get; set; }
        public byte PropByte { get; set; }
        public sbyte PropSByte { get; set; }
        public uint PropUInt { get; set; }
        public UInt16 PropUInt16 { get; set; }
        public Int16 PropInt16 { get; set; }
        public UInt32 PropUInt32 { get; set; }
        public Int32 PropInt32 { get; set; }
        public UInt64 PropUInt64 { get; set; }
        public Int64 PropInt64 { get; set; }
//        public Type Type { get; set; }
//        public Guid Guid { get; set; }
    }
}