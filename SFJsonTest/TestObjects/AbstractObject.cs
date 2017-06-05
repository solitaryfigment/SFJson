using System;

namespace SFJsonTest
{
    public abstract class SimpleObject
    {
        int AbstractPropInt { get; set; }
    }

    public class ObjectImplementingAbstractClass : SimpleObject
    {
        public int AbstractPropInt { get; set; }
        public int PropInt { get; set; }
    }
    
    public class ObjectWithAbstractClass
    {
        public SimpleObject ObjectImplementingAbstractClass { get; set; }
    }
}