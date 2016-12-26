namespace dpas.Net.Http.Mvc
{
    public interface IController
    {
        void Exec(HttpContext context, ControllerInfo controllerInfo);
    }

    public class Controller : IController
    {

        public virtual void Exec(HttpContext context, ControllerInfo controllerInfo)
        {

        }


        //internal static Dictionary<string, Dictionary<string, IController>> dictionaryController = new Dictionary<string, Dictionary<string, IController>>();
        //internal static object lockObject = new object();

        //public static IController GetController(string key, string controllerName)
        //{
        //    Dictionary<string, IController> controllers;
        //    IController result = null;
        //    if(dictionaryController.TryGetValue(key, out controllers))
        //        controllers.TryGetValue(controllerName, out result);
        //    return result;
        //}

        //public static IController AddController(string controllerName, )
        //{
        //    Dictionary<string, IController> controllers;
        //    IController result = null;
        //    if (dictionaryController.TryGetValue(key, out controllers))
        //        controllers.TryGetValue(controllerName, out result);
        //    return result;
        //}

        //public static object GetStateValue(Dictionary<string, object> state, string key)
        //{
        //    return state.GetValue(key);
        //}
    }
}
