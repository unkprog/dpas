namespace dpas.Net.Http.Mvc
{

    public interface IControllerContext : IHttpContext
    {
        IControllerInfo ControllerInfo { get; }
    }

    public class ControllerContext : HttpContext, IControllerContext
    {
        public ControllerContext(IHttpRequest aRequest) : base(aRequest)
        {
            ControllerInfo = new ControllerInfo(this);
        }

        public ControllerContext(byte[] data) : base(data)
        {
            ControllerInfo = new ControllerInfo(this);
        }
        public IControllerInfo ControllerInfo { get; private set; }
    }
}
