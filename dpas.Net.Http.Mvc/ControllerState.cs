using System.Collections.Generic;
using dpas.Core.Extensions;

namespace dpas.Net.Http.Mvc
{
    public class ControllerState
    {
        internal static Dictionary<string, Dictionary<string, object>> dictionaryState = new Dictionary<string, Dictionary<string, object>>();
        internal static object lockObject = new object();

        public static Dictionary<string, object> GetState(string key)
        {
            Dictionary<string, object> result;
            if (!dictionaryState.TryGetValue(key, out result))
            {
                result = new Dictionary<string, object>();
                lock (lockObject)
                {
                    dictionaryState.Add(key, result);
                }
            }
            return result;
        }

        public static object GetStateValue(Dictionary<string, object> state, string key)
        {
            return state.GetValue(key);
        }
    }
}
