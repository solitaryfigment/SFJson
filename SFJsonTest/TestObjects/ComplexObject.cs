namespace SFJsonTest
{
    public class ComplexObject
    {
        public SelfReferencedSimpleObject Inner { get; set; }
        public SimpleTestObject SimpleTestObject { get; set; }
        public SimpleTestObjectWithProperties SimpleTestObjectWithProperties { get; set; }
        public PrimitiveHolder PrimitiveHolder { get; set; }
    }
}