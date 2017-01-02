using System.IO;
using System.Xml;
using dpas.Service.Project;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace dpas.Console.Test
{
    public class Program
    {
        public class tclass
        {
            public int i { get; set; }
            public string s { get; set; }
        }

        public class tclass1
        {
            public tclass tclass { get; set; }
         
        }
        public static void Main(string[] args)
        {
            

            tclass1 c = new tclass1() { tclass = new tclass() { i = 1, s = "str" } };
            var json = Json.Serialize(c);
            var ddd = Json.Parse(json);
           

            System.Console.ReadKey();
        }
    }
}
