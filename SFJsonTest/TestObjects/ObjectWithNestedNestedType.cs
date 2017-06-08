namespace SFJsonTest
{
    public class ObjectWithNestedNestedType
    {
        public class NestedClass
        {
            public class NestedNestedClass
            {
                public string PropString { get; set; }
            }
            
            public NestedNestedClass PropNested { get; set; }
        }
        
        public NestedClass PropNested { get; set; }
    }
}