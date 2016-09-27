using System;
using System.Reflection;
using System.Collections.Generic;
using System.IO;

namespace dpas.Core.IO
{
    public static class AssemblyExt
    {
        public static string GetCurrentDirectory()
        {
            return GetCurrentDirectory(Assembly.GetEntryAssembly());
        }

        public static string GetCurrentDirectory(Assembly assembly)
        {
            return Path.GetDirectoryName(assembly.Location);
        }

        private static Dictionary<string, Assembly> _loadedAssemblies = new Dictionary<string, Assembly>();

        public static Assembly LoadFromFile(string assemblyPath)
        {
            Assembly result = null;
            byte[] assemblyData = null;
            int bufferSize = 4096;

            using (FileStream fileStream = File.Open(assemblyPath, FileMode.Open))
            {
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    byte[] buffer = new byte[bufferSize];
                    int readBytesCount = 0;
                    while ((readBytesCount = fileStream.Read(buffer, 0, bufferSize)) > 0)
                        memoryStream.Write(buffer, 0, readBytesCount);
                    assemblyData = memoryStream.ToArray();
                }
            }

            // TODO: Нужно что-то придумать, как восстановить загрузку библиотеки на основе байтов
            //if (assemblyData != null && assemblyData.Length > 0)
            //    result = Assembly.Load(assemblyData);
            return result;
        }

        public static Assembly Load(string assemblyPath, bool refresh = false)
        {
            Assembly result = null;
            if (refresh)
                _loadedAssemblies.Remove(assemblyPath);

            if (!_loadedAssemblies.TryGetValue(assemblyPath, out result))
            {
                result = LoadFromFile(assemblyPath);
                if (result != null)
                    _loadedAssemblies.Add(assemblyPath, result);
            }
            return result;
        }

        //Type typeFormMain = assemblyUI.GetType("dpas.FormMain");
        //   return (Form)Activator.CreateInstance(typeFormMain, new object[] { Environment.CommandLine });
    }
}
