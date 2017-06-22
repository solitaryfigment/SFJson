using System.Collections.Generic;

namespace SFJsonTest
{
    public class ObjectWithDictionary
    {
        public Dictionary<int, int> Dictionary { get; set; }
    }
    
    public class ObjectWithObjectDictionary
    {
        public Dictionary<SimpleTestObject, SimpleTestObject> Dictionary { get; set; }
    }
    
    public class ObjectWithComplexObjectDictionary
    {
        public Dictionary<ComplexObject, ComplexObject> Dictionary { get; set; }
    }
}