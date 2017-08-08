namespace SFJsonTest
{
    public interface ISimpleInterface
    {
        int InterfacePropInt { get; set; }
    }

    public class ObjectImplementingInterface : ISimpleInterface
    {
        public int InterfacePropInt { get; set; }
        public int PropInt { get; set; }
    }
    
    public class ObjectWithInterface
    {
        public ISimpleInterface ObjectImplementingInterface { get; set; }
    }
}