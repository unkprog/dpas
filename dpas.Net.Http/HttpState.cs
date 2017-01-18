using System;
using System.Collections.Generic;
using dpas.Core.Extensions;

namespace dpas.Net.Http
{
    public interface IHttpState : IDictionary<string, object>
    {
        void SetValue(string key, object value);
    }

    public class HttpState : Dictionary<string, object>, IHttpState
    {
        internal static Dictionary<string, IHttpState> dictionaryState = new Dictionary<string, IHttpState>();
        internal static object lockObject = new object();

        public static IHttpState GetState(string key)
        {
            IHttpState result;
            if (!dictionaryState.TryGetValue(key, out result))
            {
                result = new HttpState();
                lock (lockObject)
                {
                    dictionaryState.Add(key, result);
                }
            }
            return result;
        }

        public static object GetStateValue(IHttpState state, string key)
        {
            return state.GetValue(key);
        }

        public void SetValue(string key, object value)
        {
            if (ContainsKey(key))
                this[key] = value;
            else
                Add(key, value);
        }
    }

    public partial interface IHttpContext
    {
        void SetState(string key);
    }

    public partial class HttpContext 
    {
        public void SetState(string key)
        {
            this.State = HttpState.GetState(key);
        }
    }
}
