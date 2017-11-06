using System.Collections;
using System.Collections.Generic;

namespace SFJsonTest
{
    public class ObjectWithDictionary
    {
        public IDictionary<string,int> Dictionary { get; set; }
    }
    
    public class ObjectWithIEnumerableDictionary
    {
        public IEnumerable Dictionary { get; set; }
    }
    
    public class ObjectWithObjectDictionary
    {
        public Dictionary<SimpleTestObject, SimpleTestObject> Dictionary { get; set; }
    }
    
    public class ObjectWithComplexObjectDictionary
    {
        public IDictionary<ComplexObject, ComplexObject> Dictionary { get; set; }
    }
}