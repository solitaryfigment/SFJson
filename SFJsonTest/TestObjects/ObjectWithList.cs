using System.Collections.Generic;

namespace SFJsonTest
{
    public class ObjectWithList
    {
        public List<int> List { get; set; }
    }
    
    public class ObjectWithListOfObjects
    {
        public List<PrimitiveHolder> List { get; set; }
    }
    
    public class ObjectWithArray
    {
        public int[] Array { get; set; }
    }
    
    public class ObjectWithArrayOfObjects
    {
        public PrimitiveHolder[] Array { get; set; }
    }
    
    public class ObjectWithDictionary
    {
        public Dictionary<int, int> Dictionary { get; set; }
    }
}