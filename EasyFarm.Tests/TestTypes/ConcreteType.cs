namespace EasyFarm.Tests.TestTypes
{
    public class ConcreteType
    {
        public int PropertyValue { get; set; }
        public int PrivateSetterProperty { get; private set; }
        public int FieldValue;
        public int MethodCall() { return 0; }
    }
}