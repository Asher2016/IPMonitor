using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace PlatForm.Util
{
    public static class Resolver
    {
        private static readonly string AppRootPath;

        private static readonly object lockObj = new object();

        static Resolver()
        {
            #region Init AppRootPath

            string checkFile = "RootCheckFile";
            if (!string.IsNullOrEmpty(AppDomain.CurrentDomain.BaseDirectory) && File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, checkFile)))
            {
                AppRootPath = AppDomain.CurrentDomain.BaseDirectory;
            }
            else if (!string.IsNullOrEmpty(Environment.CurrentDirectory) && File.Exists(Path.Combine(Environment.CurrentDirectory, checkFile)))
            {
                AppRootPath = Environment.CurrentDirectory;
            }
            else if (!string.IsNullOrEmpty(AppDomain.CurrentDomain.RelativeSearchPath) && File.Exists(Path.Combine(AppDomain.CurrentDomain.RelativeSearchPath, checkFile)))
            {
                AppRootPath = AppDomain.CurrentDomain.RelativeSearchPath;
            }
            else
            {
                string assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                if (File.Exists(Path.Combine(assemblyDir, checkFile)))
                {
                    AppRootPath = assemblyDir;
                }
            }

            #endregion Init AppRootPath

            CachedPath = new Dictionary<string, string>();
        }

        private static Dictionary<string, string> CachedPath { get; set; }

        public static string ResolvePath(string relativePath, bool needCache = true)
        {
            string resolvedPath;
            if (!needCache)
            {
                resolvedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
            }
            else
            {
                if (!CachedPath.TryGetValue(relativePath, out resolvedPath))
                {
                    lock (lockObj)
                    {
                        if (!CachedPath.TryGetValue(relativePath, out resolvedPath))
                        {
                            resolvedPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, relativePath);
                            CachedPath.Add(relativePath, resolvedPath);
                        }
                    }
                }
            }

            return resolvedPath;
        }
    }
}
