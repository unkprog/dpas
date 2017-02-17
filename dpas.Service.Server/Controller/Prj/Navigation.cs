using System;
using System.IO;
using dpas.Net.Http;
using dpas.Net.Http.Mvc;

namespace dpas.Service.Controller.Prj
{

    public class Navigation : dpas.Net.Http.Mvc.Navigation
    {
        public Navigation()
        {
            rootPathView = "/mvc/view/prj";
            rootPathController = "/mvc/controller/prj";
        }

        protected override string GetCurrentPage(IControllerContext context)
        {
            return context.ControllerInfo.CurrentPage;
        }

    }


}
