using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyleAudit
{
    class Program
    {
        static void Main(string[] args)
        {

            var path = args[0];
            bool debugMode = Boolean.Parse(args[1]);

            if (!string.IsNullOrEmpty(path))
            {
                ProcessFile.Start(path, debugMode);
            }


            // Keep the console window open after the program has run.
            Console.WriteLine("FIN");
            Console.Read();
        }
    }
}
