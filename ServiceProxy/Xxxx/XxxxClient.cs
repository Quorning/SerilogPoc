namespace ServiceProxy.Xxxx
{
    public interface IXxxxClient
    {
        TestOperationResponse TestOperation(TestOperationRequest request);
    }

    public class XxxxClient : IXxxxClient
    {
        public TestOperationResponse TestOperation(TestOperationRequest request)
        {
            return new TestOperationResponse()
            {
                ResponseProp1 = new Prop1() { Prop11 = "foo", Prop12 = "bar" },
                ResponseProp2 = new Prop2() { Prop21 = "foo", Prop22 = "bar" },
                ResponseProp3 = "foo",
                ResponseProp4 = "bar"
            };
        }
    }

    public class TestOperationRequest
    {
        public Prop1 RequestProp1 { get; set; }
        public Prop2 RequestProp2 { get; set; }
        public string RequestProp3 { get; set; }
        public string RequestProp4 { get; set; }
    }

    public class Prop1
    {
        public string Prop11 { get; set; }
        public string Prop12 { get; set; }
    }

    public class Prop2
    {
        public string Prop21 { get; set; }
        public string Prop22 { get; set; }
    }

    public class TestOperationResponse
    {
        public Prop1 ResponseProp1 { get; set; }
        public Prop2 ResponseProp2 { get; set; }
        public string ResponseProp3 { get; set; }
        public string ResponseProp4 { get; set; }
    }
}
