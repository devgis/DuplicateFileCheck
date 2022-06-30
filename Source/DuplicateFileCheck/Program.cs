using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace DEVGIS.DuplicateFileCheck
{
    class Program
    {
        static Dictionary<string, MyFileInfo> dictMyFileInfos = null;
        static bool writLog = true;
        static string logPath = string.Empty;
        static List<string> paths = new List<string> {};
        static void Main(string[] args)
        {
            writLog = false;
            logPath = string.Empty;
            paths = new List<string>();
            //-p  paths 
            //-l lpath .outputlogpath
            //-w true false true output log 
            if (args != null && args.Length > 0)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i].ToLower())
                    {
                        case "-p":
                            for (int j = i + 1; j < args.Length; j++)
                            {
                                if (!args[j].StartsWith("-") && Directory.Exists(args[j]))
                                {
                                    paths.Add(args[j]);
                                }
                                else
                                {
                                    break;
                                }
                            }
                            break;
                        case "-w":
                            try
                            {
                                if ("true".Equals(args[i + 1].ToLower()))
                                {
                                    writLog = true;
                                }
                                else
                                {
                                    writLog = false;
                                }
                            }
                            catch
                            { }
                            break;
                        case "-l":
                            try
                            {
                                string lp = args[i + 1];
                                if (!Directory.Exists(lp))
                                {
                                    Directory.CreateDirectory(lp);
                                }

                                logPath = lp;
                            }
                            catch
                            { }
                            break;
                            break;
                    }
                }
            }
            Start();
        }

        private static void Start()
        {
            DateTime time1 = DateTime.Now;
            Console.WriteLine($"{time1.ToString()}:Started!");
            dictMyFileInfos = new Dictionary<string, MyFileInfo>();
            logPath = Path.Combine(logPath, time1.ToString("yyyyMMddHHmmss") + ".log");

            foreach (string path in paths)
            {
                VisitPath(path);
            }
            DateTime time2 = DateTime.Now;
            Console.WriteLine($"{time2.ToString()}:Completed!");
            Console.WriteLine($"TotalCount:{dictMyFileInfos.Count}");
            Console.WriteLine("Result-------------------------------------------------------------------------------");
            int sameindex = 0;
            StringBuilder sb = new StringBuilder();
            foreach (var value in dictMyFileInfos.Values)
            {
                if (value.DuplicateFiles != null)
                {
                    sameindex++;
                    string s = sameindex + ":" + value.Name;
                    foreach (var d in value.DuplicateFiles)
                    {
                        s += " And " + d.Name;
                    }
                    Console.WriteLine(s);
                    sb.AppendLine(s);
                }
            }
            Console.WriteLine("Result-------------------------------------------------------------------------------");
            sb.AppendLine($"TotalDuplicateCounts:{sameindex}");
            sb.AppendLine($"TotalUsedSeconds:{(time2 - time1).TotalSeconds}");
            Console.WriteLine($"Done! File name is {logPath}");
            Console.WriteLine("Press any key to exit!");
            File.WriteAllText(logPath, sb.ToString());
            Console.Read();
        }
        private static void ShowLog(string message)
        {
            
        }
        private static void VisitPath(string Path)
        {
            Console.WriteLine($"{DateTime.Now.ToString()}:Visiting {Path}");
            foreach (var file in Directory.GetFiles(Path))
            {
                var myfileinfo = GetMyFileInfo(file);
                if (dictMyFileInfos.ContainsKey(myfileinfo.Key))
                {
                    if (dictMyFileInfos[myfileinfo.Key].DuplicateFiles == null)
                    {
                        dictMyFileInfos[myfileinfo.Key].DuplicateFiles = new List<MyFileInfo>();
                    }
                    dictMyFileInfos[myfileinfo.Key].DuplicateFiles.Add(myfileinfo);
                }
                else
                {
                    dictMyFileInfos.Add(myfileinfo.Key, myfileinfo);
                }
            }

            //visit the paths
            foreach (var dir in Directory.GetDirectories(Path))
            {
                VisitPath(dir);
            }
        }

        private static MyFileInfo GetMyFileInfo(string FileName)
        {
            if (File.Exists(FileName))
            {
                return new MyFileInfo { Key= GetMD5WithFilePath (FileName),Name= FileName };
            }
            else
            {
                return null;
            }
        }

        static MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
        static public string GetMD5WithFilePath(string filePath)
        {
            FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] hash_byte = md5.ComputeHash(file);
            string str = System.BitConverter.ToString(hash_byte);
            str = str.Replace("-", "");
            return str;
        }
    }
}
