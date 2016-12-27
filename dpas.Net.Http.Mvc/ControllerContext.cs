using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dpas.Net.Http.Mvc
{

    public class ControllerContext : HttpContext
    {
        public ControllerContext(IHttpRequest aRequest) : base(aRequest)
        {
            ControllerInfo = new ControllerInfo(this);
        }

        public ControllerContext(byte[] data) : base(data)
        {
            ControllerInfo = new ControllerInfo(this);
        }
        public ControllerInfo ControllerInfo { get; private set; }
    }
}
