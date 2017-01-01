using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dpas
{
    public static partial class Json
    {
        public static object Parse(string json)
        {
            return new JsonParser(json).Decode();
        }

        public static string Serialize(object obj)
        {
            return new JsonSerializer().ToJson(obj);
        }
    }
}
