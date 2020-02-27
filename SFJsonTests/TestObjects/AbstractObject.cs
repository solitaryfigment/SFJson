using System;

namespace SFJsonTests
{
    public abstract class SimpleObject
    {
        public abstract int AbstractPropInt { get; set; }
    }

    public class ObjectImplementingAbstractClass : SimpleObject
    {
        public override int AbstractPropInt { get; set; }
        public int PropInt { get; set; }
    }
    
    public class ObjectWithAbstractClass
    {
        public SimpleObject ObjectImplementingAbstractClass { get; set; }
    }

    public interface ISimpleBaseObject
    {
        int BasePropInt { get; set; }
    }
    public class SimpleBaseObject : ISimpleBaseObject
    {
        public int BaseFieldInt;
        public int BasePropInt { get; set; }
    }

    public class SimpleInheritedObject : SimpleBaseObject
    {
        public string FieldString;
        public string PropString { get; set; }
    }

    public class ObjectWithMembersOfInheritedClasses
    {
        public SimpleBaseObject SimpleBaseObject { get; set; }
    }
}