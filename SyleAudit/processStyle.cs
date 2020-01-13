using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SyleAudit
{
    class ProcessStyle
    {
        private List<string> _styleSheets = new List<string>();
        private List<ClassInfo> _scc = new List<ClassInfo>();

        public List<ClassInfo> Styles
        {
            get { return this._scc; }
            set { this._scc = value; }
        }

        //public ProcessStyle()
        //{
        //    this._styleSheets = new List<string>();
        //    this._scc = new SortedList<string, StyleClass>();
        //}

        //public void AddStyleSheet(string path)
        //{
        //    this._styleSheets.Add(path);
        //    ProcessStyleSheet(path);
        //}

        public string GetStyleSheet(int index)
        {
            return this._styleSheets[index];
        }

        public List<ClassInfo> checkForStyles(string path, bool debugMode)
        {
            this._styleSheets.Add(path);
            List<string> cssfilePaths = Directory.GetFiles(path, "*.css", SearchOption.AllDirectories).ToList().Where(x => !x.Contains("node_module")).ToList();
            utility.log_yellow("Found " + cssfilePaths.Count.ToString() + " .css files at folder");

            List<string> scssfilePaths = Directory.GetFiles(path, "*.scss", SearchOption.AllDirectories).ToList().Where(x => !x.Contains("node_module")).ToList();
            utility.log_yellow("Found " + scssfilePaths.Count.ToString() + " .scss files at folder");

            List<string> sassfilePaths = Directory.GetFiles(path, "*.sass", SearchOption.AllDirectories).ToList().Where(x => !x.Contains("node_module")).ToList();
            utility.log_yellow("Found " + sassfilePaths.Count.ToString() + " .sass files at folder");

            //var cntNodeModFiles = filePaths.Where(x => x.Contains("node_module")).Count();
            List<string> filePaths = new List<string>();
            filePaths.AddRange(cssfilePaths);
            filePaths.AddRange(scssfilePaths);
            filePaths.AddRange(sassfilePaths);

            //List<StyleInfo> files = new List<StyleInfo>();
            List<ClassInfo> files = new List<ClassInfo>();
            foreach (string file in filePaths)
            {
                if (debugMode)
                {
                    Console.WriteLine(file);
                }
                var parsedFile = getInfo(file);
                //if (!files.ContainsKey(parsedFile.Name))
                //{
                if(parsedFile.Count > 0) { 
                    files.AddRange(parsedFile);
                }
                //else
                //{
                //    //var file = files
                //}

                //if (parsedFile.lines.Count > 0)
                //{
                //    files.Add(parsedFile);
                //}
            }
            return files;
        }

        public List<ClassInfo> getInfo(string filePath)
        {
            
            var hi = new List<ClassInfo>();
            var HTMLFileInfo = new FileInfo(filePath);
            var fileName = HTMLFileInfo.Name;
            using (var reader = new StreamReader(filePath))
            {
                int lineNum = 0;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    lineNum++;
                    //if (line.IndexOf(".") > -1 && line.IndexOf("html") == -1)
                    if(line.Contains(".") && !line.Contains("@") && !line.Contains("//") && !line.Contains("http") && !line.Contains(";"))
                    {
                        var splitClasses = line.Replace("{", "").Split('.').ToList();
                        foreach(var x in splitClasses)
                        {
                            var style = x.Trim();
                            if (!string.IsNullOrWhiteSpace(style))
                            {
                                var si = new ClassInfo();
                                si.className = style;
                                si.path = filePath;
                                si.lineNo = lineNum;
                                si.fileName = fileName;
                                hi.Add(si);
                            }
                        }
                    }
                }
            }
            return hi;
        }
    }
}