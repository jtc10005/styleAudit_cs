using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyleAudit
{
    class ProcessHTML
    {
        public List<ClassInfo> classes { get; set; }

        public List<ClassInfo> checkHtml(string path, bool debugMode)
        {
            //DirectoryInfo d = new DirectoryInfo(@path);//Assuming Test is your Folder
            //FileInfo[] Files = d.GetFiles("*.html"); //Getting Text files

            string[] filePaths = Directory.GetFiles(path, "*.html", SearchOption.AllDirectories);
            utility.log_yellow("Found " + filePaths.Length.ToString() + " .html files at folder");

            //var cntNodeModFiles = filePaths.Where(x => x.Contains("node_module")).Count();
            filePaths = filePaths.Where(x => !x.Contains("node_module") && !x.Contains("testbed") && !x.Contains("dist")).ToArray();


            classes = new List<ClassInfo>();

            foreach (string file in filePaths)
            {
                if (debugMode)
                {
                    Console.WriteLine(file);
                }

                var parsedFile = getHTMLInfo(file);
                if (parsedFile.Count > 0)
                {
                    classes.AddRange(parsedFile);
                }
            }
            return classes;
        }

        public List<ClassInfo> getHTMLInfo(string filePath)
        {
            var hi = new List<ClassInfo>();
            var HTMLFileInfo = new FileInfo(filePath);
            var fileName = HTMLFileInfo.Name;
            using (var reader = new StreamReader(filePath))
            {
                int lineNum = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine().Trim();
                    lineNum++;
                    List<string> classes = new List<string>();
                    if (line.ToUpper().Contains("CLASS") )
                    {
                        if (!line.ToUpper().Contains("CLASS]"))
                        {

                            var startIndex = line.IndexOf("class");

                            int realStartIndex = 0;
                            int stopIndex = 0;
                            bool startChecking = false;
                            if (startIndex != -1)
                            {
                                for (int i = startIndex; i < line.Length; i++)
                                {
                                    if (!startChecking)
                                    {
                                        if (line[i] == '"')
                                        {
                                            startChecking = true;
                                            realStartIndex = i + 1;
                                        }
                                    }
                                    else
                                    {
                                        if (line[i] == '"')
                                        {
                                            stopIndex = i;
                                            break;
                                        }
                                    }

                                }
                                classes = line.Substring(realStartIndex, stopIndex - realStartIndex).Split(' ').ToList();
                            }
                        }


                        

                        classes.ForEach(x =>
                        {
                            if (!string.IsNullOrWhiteSpace(x))
                            {
                                var li = new ClassInfo();
                                li.lineNo = lineNum;
                                li.className = x;
                                li.path = filePath;
                                li.fileName = fileName;

                                hi.Add(li);
                            }
                        });
                        
                    }

                }

            }
            return hi;
        }

    }
}

