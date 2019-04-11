using System.IO;

namespace System
{
    public static class MyAppContext
    {
        static object InitStateObject;

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initialize(Func<string, string> mapPath)
        {
            if (InitStateObject != null)
            {
                throw new Exception("已初始化");
            }

            InitStateObject = new object();
            if (mapPath != null)
            {
                MapPath = mapPath ?? throw new ArgumentNullException(nameof(mapPath));
                RootPath = mapPath("~/");
                AppDocsPath = mapPath("~/App_Docs");
                AppDataPath = mapPath("~/App_Data"); 
            }
             
        }
        public static  void ResetCache()
        { 
        }
        /// <summary>
        /// 映射虚拟路径到物理路径
        /// </summary>
        public static Func<string, string> MapPath { get; private set; }

        /// <summary>
        /// WEB根目录(物理路径)
        /// </summary>
        public static string RootPath { get; private set; }

        /// <summary>
        /// App_Docs物理路径
        /// </summary>
        public static string AppDocsPath { get; private set; }

        /// <summary>
        /// App_Data物理路径
        /// </summary>
        public static string AppDataPath { get; private set; }

        /// <summary>
        /// 合成文件夹路径并确保文件夹存在
        /// </summary>
        public static string EnsureDirectory(string rootPath, string relativePath)
        {
            var path = Path.Combine(rootPath, relativePath);
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}
