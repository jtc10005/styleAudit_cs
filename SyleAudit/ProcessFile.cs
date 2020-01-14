using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyleAudit
{
    class ProcessFile
    {



        public static void Start(string path, bool debugMode)
        {
            utility.log_green("Path: " + path + Environment.NewLine);
            var htmlProcess = new ProcessHTML();
            var htmlData = htmlProcess.checkHtml(path, debugMode);

            var styleProcess = new ProcessStyle();
            var styledata = styleProcess.checkForStyles(path, debugMode);

            validateStylesExistInHTML(htmlData, styledata);
            //checkhtmlForStyles(htmlData, styledata);
        }


        static void validateStylesExistInHTML(List<ClassInfo> htmldata, List<ClassInfo> styledata)
        {
            utility.log_green("Checking for styles that are not in HTML files....");
            //var showLine = false;
            //List<string> alerts = new List<string>();
            foreach (var sd in styledata)
            {
                //if (!showLine)
                //{
                //    utility.log_yellow("checking file: " + sd.fileName + " - " + sd.path);
                //}
                //showLine = true;
                var styleName = sd.className;
                var exists = htmldata.Where(x => x.className == styleName).Count() > 0;
                if (!exists)
                {

                    
                    utility.log_red(sd.fileName + " Line " + sd.lineNo.ToString() + " contains class '" + styleName + "' which was not found in any html");
                }

            }

        }

        static void checkhtmlForStyles(List<ClassInfo> htmldata, List<ClassInfo> styledata)
        {
            utility.log_green("Checking for styles that are in HTML but not in style files....");
            var showLine = false;
            List<string> alerts = new List<string>();
            foreach (var sd in htmldata)
            {
                var styleName = sd.className;
                var exists = styledata.Where(x => x.className == styleName).Count() > 0;
                if (!exists)
                {
                    if (!showLine)
                    {
                        utility.log_yellow("Alerts found for file: " + sd.fileName + " - " + sd.path);
                    }
                    showLine = true;

                    utility.log_red("----- Line " + sd.lineNo.ToString() + " contains class '" + styleName + "' which was not found in any style");
                }

            }
        }
    }
}
