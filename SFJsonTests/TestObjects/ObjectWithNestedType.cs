namespace SFJsonTests
{
    public class ObjectWithNestedType
    {
        public class NestedClass
        {
            public string PropString { get; set; }
        }
        
        public NestedClass PropNested { get; set; }
        public int PropInt { get; set; }
    }
}