﻿using System.Collections;
using System.Collections.Generic;

namespace SFJsonTests
{
    public class ObjectWithList
    {
        public List<int> List { get; set; }
    }
    
    public class ObjectWithIEnumerable
    {
        public IEnumerable<int> List { get; set; }
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
}