using SFJson.Attributes;

namespace SFJsonTest
{
    public class ObjectWithIgnoredMembers
    {
        public int PropInt { get; set; }
        [JsonIgnore] public int PropIgnoredInt { get; set; }
        
        [JsonIgnore] public int FieldIgnoredInt;
        public int FieldInt;
    }
    
    public class ObjectWithIgnoredObjectMembers
    {
        public ObjectWithIgnoredMembers PropObject { get; set; }
        [JsonIgnore] public ObjectWithIgnoredMembers PropIgnoredObject { get; set; }
    }
}