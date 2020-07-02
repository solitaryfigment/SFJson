using System;
using System.Collections.Generic;
using System.Linq;
using SFJson.Attributes;
using SFJson.Conversion;
using UnityEngine;

namespace SFJsonTests
{
    public class DragTargetOffset
    {
        public readonly Vector3 Offset;

        public DragTargetOffset()
        {
            Offset = new Vector3(0, 0, 0);
        }
        public DragTargetOffset(float x = 0, float y = 0, float z = 0)
        {
            Offset = new Vector3(x, y, z);
        }
    }

    [Serializable]
    public class LayerToDragTargetOffsetMap
    {
        public Vector3 Vector;
    }
    
    [Serializable]
    public class DragEntityBehaviorProfile : EntityBehaviorProfile
    {
        public string[] TargetLayers;
    }
    
    [Serializable]
    public class WorldDragEntityBehaviorProfile : DragEntityBehaviorProfile
    {
        public Dictionary<int, DragTargetOffset> OffsetMap;
        public int Test;
    }
    
    [Serializable]
    public class UIDragEntityBehaviorProfile : DragEntityBehaviorProfile
    {
    }
    
    public static class EntityBehaviorTypes
    {
        public static readonly EntityBehaviorType WORLD_HOVER = new EntityBehaviorType(1, "World", "Permanent", "Interaction");
        public static readonly EntityBehaviorType WORLD_DRAG = new EntityBehaviorType(2, "World", "Permanent", "Interaction");
        public static readonly EntityBehaviorType UI_HOVER = new EntityBehaviorType(3, "UI", "Permanent", "Interaction");
        public static readonly EntityBehaviorType UI_DRAG = new EntityBehaviorType(4, "UI", "Permanent", "Interaction");
        public static readonly EntityBehaviorType INTERACT = new EntityBehaviorType(5, "Permanent", "Interaction");
        public static readonly EntityBehaviorType CONTEXT = new EntityBehaviorType(6, "Permanent", "Interaction");
        public static readonly EntityBehaviorType CLICK = new EntityBehaviorType(7, "Permanent", "Interaction");
    }
    
    public class EntityBehaviorProfileConverter : CustomConverter
    {
        public override object Deserialize()
        {
            var type = GetValueOfChild<EntityBehaviorType>(typeof(EntityBehaviorProfile), "Type");
            
            Console.WriteLine(type);
            // TODO: Move to Dictionary of types
            if(type != null && type == EntityBehaviorTypes.WORLD_DRAG)
            {
                return _token.GetValue<WorldDragEntityBehaviorProfile>();
            }
            if(type != null && type == EntityBehaviorTypes.UI_DRAG)
            {
                return _token.GetValue<UIDragEntityBehaviorProfile>();
            }

            return _token.GetValue(_defaultType);
        }

        public override string Serialize(object obj)
        {
            return Converter.Serialize(obj);
        }
    }

    public class EntityBehaviorTypeConverter : CustomConverter
    {
        public override object Deserialize()
        {
            var id = _token.Children.FirstOrDefault(c => c.Name == "Id")?.GetValue<int>();
            
            Console.WriteLine("ID: " + id);
            if(id == EntityBehaviorTypes.WORLD_DRAG)
            {
                return EntityBehaviorTypes.WORLD_DRAG;
            }
            if(id == EntityBehaviorTypes.UI_DRAG)
            {
                return EntityBehaviorTypes.UI_DRAG;
            }
            
            return EntityBehaviorTypes.UI_HOVER;
        }

        public override string Serialize(object obj)
        {
            return Converter.Serialize(obj);
        }
    }

    [Serializable]
    public struct EntityBehaviorTypeData
    {
        public readonly int Id;
        public int[] Values;
    }

    [Serializable]
    public struct EntityBehaviorType
    {
        public readonly int Id;
        public string[] Categories;
        public EntityBehaviorTypeData Data;

        public EntityBehaviorType(int id, params string[] categories)
        {
            Id = id;
            Categories = categories;
            Data = new EntityBehaviorTypeData();
        }

        public static implicit operator int(EntityBehaviorType entityBehaviorType)
        {
            return entityBehaviorType.Id;
        }
        
        public static implicit operator EntityBehaviorType(int intVal)
        {
            return new EntityBehaviorType(intVal);
        }

        public override bool Equals(object obj)
        {
            if(obj is EntityBehaviorType)
            {
                return Equals((EntityBehaviorType)obj);
            }

            return false;
        }

        public bool Equals(EntityBehaviorType other)
        {
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
    
    [Serializable]
    public class EntityBehaviorProfile
    {
        [CustomConverter(typeof(EntityBehaviorTypeConverter))]
        public EntityBehaviorType Type;

        // public static EntityBehaviorProfile Create(EntityBehaviorType type)
        // {
        //     EntityBehaviorProfile entityBehaviorProfile;
        //     switch(type)
        //     {
        //         case EntityBehaviorTypes.UI_DRAG:
        //             break;
        //         default:
        //             entityBehaviorProfile = new EntityBehaviorProfile();
        //             break;
        //     }
        //
        //     entityBehaviorProfile.Type = type;
        //     return entityBehaviorProfile;
        // }
    }
    
    [Serializable]
    public class EntityProfile : ISerializationCallbackReceiver
    {
        [HideInInspector][SerializeField] protected string _serialized;
        public long Id;
        
        [NonSerialized]
        [CustomConverter(typeof(EnumerableElementConverter<EntityBehaviorProfileConverter, List<EntityBehaviorProfile>>))]
        public List<EntityBehaviorProfile> EntityBehaviorProfiles = new List<EntityBehaviorProfile>();
        
        public virtual void OnBeforeSerialize()
        {
            try
            {
                _serialized = Converter.Serialize(this);
            }
            catch
            {
                // Do Nothing
            }
        }
        
        public virtual void OnAfterDeserialize()
        {
            if(!string.IsNullOrEmpty(_serialized))
            {
                var profile = Converter.DeserializeOnToInstance<EntityProfile>(this, _serialized);
                EntityBehaviorProfiles = profile.EntityBehaviorProfiles;
            }
        }
    }

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
        public override object Deserialize()
        {
            Console.WriteLine(_token.JsonTokenType);
            var id = GetValueOfChild(typeof(ISimpleBaseObject), typeof(int), "BaseFieldInt");

            int i = id != null ? (int)id : default;
            
            switch(i)
            {
                case 2:
                    Console.WriteLine(2);
                    return _token.GetValue<SimpleInheritedObject2>();
                case 3:
                    Console.WriteLine(3);
                    return _token.GetValue<SimpleInheritedObject3>();
                default:
                    Console.WriteLine("D: " + i);
                    return _token.GetValue(_defaultType);
            }
        }

        public override string Serialize(object obj)
        {
            return Converter.Serialize(obj);
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