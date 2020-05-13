using System;
using System.Collections.Generic;
using NUnit.Framework;
using SFJson.Conversion;
using SFJson.Conversion.Settings;
using SFJson.Utils;

namespace SFJsonTests
{
    [TestFixture]
    public class InheritedTests
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
        public void CanConvertInheritedObjects()
        {
            var obj = new SimpleInheritedObject
            {
                BaseFieldInt = 1,
                BasePropInt = 2,
                FieldString = "String",
                PropString = "String2"
            };

            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings { SerializationTypeHandle = SerializationTypeHandle.All });
            
            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"FieldString\":\"String\",\"BaseFieldInt\":1,\"PropString\":\"String2\",\"BasePropInt\":2}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTests.SimpleInheritedObject, SFJsonTests\",\"FieldString\":\"String\",\"BaseFieldInt\":1,\"PropString\":\"String2\",\"BasePropInt\":2}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<SimpleInheritedObject>(str);
            
            Assert.IsTrue(strDeserialized != null);
            Assert.AreEqual(obj.BaseFieldInt, strDeserialized.BaseFieldInt);
            Assert.AreEqual(obj.FieldString, strDeserialized.FieldString);
            Assert.AreEqual(obj.BasePropInt, strDeserialized.BasePropInt);
            Assert.AreEqual(obj.PropString, strDeserialized.PropString);
            
            var strWithTypeDeserialized = _deserializer.Deserialize<SimpleInheritedObject>(strWithType);
            
            Assert.IsTrue(strWithTypeDeserialized != null);
            Assert.AreEqual(obj.BaseFieldInt, strWithTypeDeserialized.BaseFieldInt);
            Assert.AreEqual(obj.FieldString, strWithTypeDeserialized.FieldString);
            Assert.AreEqual(obj.BasePropInt, strWithTypeDeserialized.BasePropInt);
            Assert.AreEqual(obj.PropString, strWithTypeDeserialized.PropString);
        }

        [Test]
        public void ProfileTest()
        {
            var profile = new EntityProfile();
            profile.EntityBehaviorProfiles = new List<EntityBehaviorProfile>();
            profile.EntityBehaviorProfiles.Add(new WorldDragEntityBehaviorProfile
            {
                Type = EntityBehaviorTypes.WORLD_DRAG,
                TargetLayers = new[] {"Ground"},
                Test = 5,
                OffsetMap = new Dictionary<int, DragTargetOffset>()
            });
            profile.EntityBehaviorProfiles.Add(new UIDragEntityBehaviorProfile());
            ((WorldDragEntityBehaviorProfile)profile.EntityBehaviorProfiles[0]).OffsetMap = new Dictionary<int, DragTargetOffset>();
            var profileStr = _serializer.Serialize(profile);
            var dProfile = _deserializer.Deserialize<EntityProfile>(profileStr);
        }

        [Test]
        public void CanConvertUseCustomConverters()
        {
            var mainObj = new SimpleObjectWrapper
            {
                Obj1 = new SimpleInheritedObject2
                {
                    BaseFieldInt = 2,
                    BasePropInt = 2,
                    FieldString = "String",
                    PropString = "String2"
                },
                Obj2 = new SimpleInheritedObject3
                {
                    BaseFieldInt = 3,
                    BasePropInt = 4,
                    FieldString = "String3",
                    PropString = "String4",
                    List = new List<ISimpleBaseObject>
                    {
                        new SimpleInheritedObject3
                        {
                            BaseFieldInt = 3,
                            BasePropInt = 4,
                            FieldString = "String3",
                            PropString = "String4"
                        }
                    }
                }
            };

            var str = _serializer.Serialize(mainObj);
            var strWithType = _serializer.Serialize(mainObj, new SerializerSettings { SerializationTypeHandle = SerializationTypeHandle.All });
            
            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            // Assert.AreEqual("{\"FieldString\":\"String\",\"BaseFieldInt\":1,\"PropString\":\"String2\",\"BasePropInt\":2}", str);
            
            var strDeserialized = _deserializer.Deserialize<SimpleObjectWrapper>(str);
            
            Assert.IsTrue(strDeserialized != null);
            Assert.AreEqual(mainObj.Obj1.BaseFieldInt, strDeserialized.Obj1.BaseFieldInt);
            Assert.AreEqual(((SimpleInheritedObject2)mainObj.Obj1).FieldString, ((SimpleInheritedObject2)strDeserialized.Obj1).FieldString);
            Assert.AreEqual(mainObj.Obj1.BasePropInt, strDeserialized.Obj1.BasePropInt);
            Assert.AreEqual(((SimpleInheritedObject2)mainObj.Obj1).PropString, ((SimpleInheritedObject2)strDeserialized.Obj1).PropString);
            Assert.AreEqual(mainObj.Obj2.BaseFieldInt, strDeserialized.Obj2.BaseFieldInt);
            Assert.AreEqual(((SimpleInheritedObject3)mainObj.Obj2).FieldString, ((SimpleInheritedObject3)strDeserialized.Obj2).FieldString);
            Assert.AreEqual(mainObj.Obj2.BasePropInt, strDeserialized.Obj2.BasePropInt);
            Assert.AreEqual(((SimpleInheritedObject3)mainObj.Obj2).PropString, ((SimpleInheritedObject3)strDeserialized.Obj2).PropString);

            var objElm = ((SimpleInheritedObject3)mainObj.Obj2).List[0] as SimpleInheritedObject3;
            var deserElm = ((SimpleInheritedObject3)strDeserialized.Obj2).List[0] as SimpleInheritedObject3;
            Assert.AreEqual(objElm.BaseFieldInt, deserElm.BaseFieldInt);
            Assert.AreEqual(objElm.FieldString, deserElm.FieldString);
            Assert.AreEqual(objElm.BasePropInt, deserElm.BasePropInt);
            Assert.AreEqual(objElm.PropString, deserElm.PropString);
            
            
            strDeserialized = _deserializer.Deserialize<SimpleObjectWrapper>(strWithType);
            
            Assert.IsTrue(strDeserialized != null);
            Assert.AreEqual(mainObj.Obj1.BaseFieldInt, strDeserialized.Obj1.BaseFieldInt);
            Assert.AreEqual(((SimpleInheritedObject2)mainObj.Obj1).FieldString, ((SimpleInheritedObject2)strDeserialized.Obj1).FieldString);
            Assert.AreEqual(mainObj.Obj1.BasePropInt, strDeserialized.Obj1.BasePropInt);
            Assert.AreEqual(((SimpleInheritedObject2)mainObj.Obj1).PropString, ((SimpleInheritedObject2)strDeserialized.Obj1).PropString);
            Assert.AreEqual(mainObj.Obj2.BaseFieldInt, strDeserialized.Obj2.BaseFieldInt);
            Assert.AreEqual(((SimpleInheritedObject3)mainObj.Obj2).FieldString, ((SimpleInheritedObject3)strDeserialized.Obj2).FieldString);
            Assert.AreEqual(mainObj.Obj2.BasePropInt, strDeserialized.Obj2.BasePropInt);
            Assert.AreEqual(((SimpleInheritedObject3)mainObj.Obj2).PropString, ((SimpleInheritedObject3)strDeserialized.Obj2).PropString);

            objElm = ((SimpleInheritedObject3)mainObj.Obj2).List[0] as SimpleInheritedObject3;
            deserElm = ((SimpleInheritedObject3)strDeserialized.Obj2).List[0] as SimpleInheritedObject3;
            Assert.AreEqual(objElm.BaseFieldInt, deserElm.BaseFieldInt);
            Assert.AreEqual(objElm.FieldString, deserElm.FieldString);
            Assert.AreEqual(objElm.BasePropInt, deserElm.BasePropInt);
            Assert.AreEqual(objElm.PropString, deserElm.PropString);
        }

        [Test]
        public void OnlyConvertsBaseObjectsWhenAssignedToBaseClass()
        {
            var obj = new ObjectWithMembersOfInheritedClasses
            {
                SimpleBaseObject = new SimpleInheritedObject
                {
                    BaseFieldInt = 1,
                    BasePropInt = 2,
                    FieldString = "String",
                    PropString = "String2"
                }
            };

            var str = _serializer.Serialize(obj);
            var strWithType = _serializer.Serialize(obj, new SerializerSettings { SerializationTypeHandle = SerializationTypeHandle.All });
            
            Console.WriteLine(str);
            Console.WriteLine(strWithType);
            Assert.AreEqual("{\"SimpleBaseObject\":{\"FieldString\":\"String\",\"BaseFieldInt\":1,\"PropString\":\"String2\",\"BasePropInt\":2}}", str);
            Assert.AreEqual("{\"$type\":\"SFJsonTests.ObjectWithMembersOfInheritedClasses, SFJsonTests\",\"SimpleBaseObject\":{\"$type\":\"SFJsonTests.SimpleInheritedObject, SFJsonTests\",\"FieldString\":\"String\",\"BaseFieldInt\":1,\"PropString\":\"String2\",\"BasePropInt\":2}}", strWithType);
            
            var strDeserialized = _deserializer.Deserialize<ObjectWithMembersOfInheritedClasses>(str);
            
            Assert.IsTrue(strDeserialized != null);
            Assert.NotNull(obj.SimpleBaseObject);
            Assert.AreEqual(obj.SimpleBaseObject.BaseFieldInt, strDeserialized.SimpleBaseObject.BaseFieldInt);
            Assert.AreEqual(obj.SimpleBaseObject.BasePropInt, strDeserialized.SimpleBaseObject.BasePropInt);
            Assert.IsNotInstanceOf<SimpleInheritedObject>(strDeserialized.SimpleBaseObject);

            var strWithTypeDeserialized = _deserializer.Deserialize<ObjectWithMembersOfInheritedClasses>(strWithType);
            
            Assert.IsTrue(strWithTypeDeserialized != null);
            Assert.AreEqual(obj.SimpleBaseObject.BaseFieldInt, strWithTypeDeserialized.SimpleBaseObject.BaseFieldInt);
            Assert.AreEqual(obj.SimpleBaseObject.BasePropInt, strWithTypeDeserialized.SimpleBaseObject.BasePropInt);
            Assert.IsInstanceOf<SimpleInheritedObject>(strWithTypeDeserialized.SimpleBaseObject);
            Assert.AreEqual(((SimpleInheritedObject)obj.SimpleBaseObject).FieldString, ((SimpleInheritedObject)strWithTypeDeserialized.SimpleBaseObject).FieldString);
            Assert.AreEqual(((SimpleInheritedObject)obj.SimpleBaseObject).PropString, ((SimpleInheritedObject)strWithTypeDeserialized.SimpleBaseObject).PropString);
        }
    }
}