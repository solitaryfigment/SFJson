using System;
using System.Collections.Generic;
using System.Linq;
using SFJson.Attributes;
using SFJson.Conversion;
using SFJson.Tokenization.Tokens;
using UnityEngine;

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

    public class MyCustomConverter : CustomConverter
    {
        public override object Convert()
        {
            Console.WriteLine(_token.JsonTokenType);
            var id = GetValueOfChild<int>(typeof(ISimpleBaseObject), "BaseFieldInt");
            
            switch(id)
            {
                case 2:
                    return _token.GetValue<SimpleInheritedObject2>();
                case 3:
                    return _token.GetValue<SimpleInheritedObject3>();
                default:
                    return _token.GetValue(_defaultType);
            }
        }
    }
    
    public class SimpleObjectWrapper
    {
        [CustomConverter(typeof(MyCustomConverter))]
        public SimpleBaseObject2 Obj1;
        
        [CustomConverter(typeof(MyCustomConverter))]
        public SimpleBaseObject2 Obj2 { get; set; }
    }
    
    public class SimpleBaseObject2 : ISimpleBaseObject
    {
        public int BaseFieldInt;
        public int BasePropInt { get; set; }
    }
    
    public class SimpleInheritedObject3 : SimpleInheritedObject2
    {
        [CustomConverter(typeof(EnumerableElementConverter<MyCustomConverter, List<ISimpleBaseObject>>))]
        public List<ISimpleBaseObject> List;
    }

    public class SimpleInheritedObject2 : SimpleBaseObject2
    {
        public string FieldString;
        public string PropString { get; set; }
    }

    public class ObjectWithMembersOfInheritedClasses
    {
        public SimpleBaseObject SimpleBaseObject { get; set; }
    }
}