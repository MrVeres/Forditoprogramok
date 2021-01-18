using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics;

namespace stdrlnt
{
    class sourceHandler
    {
        string source, finalcode, orders = "";
        string content = "";

        /*{ Dictionary<string, string> replaces = new Dictionary<string, string>();

         public void openOrders()
         {
             //szabályok egy fájlból
         }
             //white spaces
             {"\n", " " },
             {"  ", " " },
             { "{ ", " " },
             {" {", " " },
             {"  ", " " },
             //keywords
             {"IF", "10" },
             {"(", "20" },
             {")", "30" },
             {"=", "40" },
             {"==", "50" },
             {"{", "60" },
             {"}", "70" }
         };*/

        Dictionary<string, string> replaces = new Dictionary<string, string>();
        public void openOrders()
        {
            try
            {
                StreamReader SR = new StreamReader(File.OpenRead(orders));
                while (SR.Peek() > -1)
                {
                    string s = SR.ReadLine();
                    string[] KV = s.Split("|");
                    replaces.Add(KV[0], KV[1]);
                }
                SR.Close();
            }
            catch (IOException IOE)
            {
                Console.WriteLine(IOE.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        List<string> symbolTable = new List<string>();
        int symbolTableIndex = 0;

        string changeVariablesAndConstants(string varAndConstName)
        {
            symbolTable.Add(varAndConstName);
            symbolTableIndex += 1;
            string res = "00" + symbolTableIndex.ToString();

            return res.Substring(res.Length - 3);
        }
        public void replaceContent()
        {
            var blockComment = @"/[*][\w\d\s]+[*]/";
            var lineComments = @"//.*?\n";

            string patternNumber = @"([0-9]+)";
            string patternVar = @"([a-z-_]+)";

            content = Regex.Replace(content, blockComment, " ");
            content = Regex.Replace(content, lineComments, " ");

            content = Regex.Replace(content, patternNumber, changeVariablesAndConstants("$1"));
            content = Regex.Replace(content, patternVar, changeVariablesAndConstants("$1"));

            foreach (var x in replaces)
            {
                while (content.Contains(x.Key))
                {
                    content = content.Replace(x.Key, x.Value);
                }
            }
        }
        public void openFileToWrite()
        {
            try
            {
                StreamWriter SW = new StreamWriter(File.Open(finalcode, FileMode.Create));
                SW.WriteLine(content);
                SW.Flush();
                SW.Close();
            }
            catch (IOException IOE)
            {
                Console.WriteLine(IOE.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public void openFileToRead()
        {
            try
            {
                StreamReader SR = new StreamReader(File.OpenRead(source));
                content = SR.ReadToEnd();
                SR.Close();
            }
            catch (IOException IOE)
            {
                Console.WriteLine(IOE.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public sourceHandler(string source, string finalcode, string orders)
        {
            this.source = source;
            this.finalcode = finalcode;
            this.orders = orders;
        }
    }
    class Program
    {
        static void Main(string[] args)
        {

            string source = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\source.txt";
            string finalcode = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\finalcode.txt";
            string orders = Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName) + @"\orders.txt";
            sourceHandler sourceHandler = new sourceHandler(source, finalcode, orders);

            sourceHandler.openFileToRead();
            sourceHandler.openOrders();
            sourceHandler.replaceContent();
            sourceHandler.openFileToWrite();
            Console.WriteLine("Folyamat vége!");

            /*string blockComments = "\n\n/* block comment \n" +
                "almalma" +
                "\n" +
                "//line comment\n" +
                "//     line comment\n" +
                "IF (a==2) {b=6} WHILE";
            // 10 20 VAR[a] 30 CONST[6] 40 ...
            // 10 20 001 30 002 40 ...
            //symbolTable["a", "20"]
            var blockComment = @"/[*][\w\d\s]+[*]/";
            var lineComments = @"//.*?\n";

            Console.WriteLine("Eredeti szöveg: {0}", blockComments);

            string result = Regex.Replace(blockComments, blockComment, String.Empty);
            result = Regex.Replace(result, lineComments, String.Empty);

            Console.WriteLine("Eredmény: {0}", result);

            string patternNumber = @"([0-9]+)";
            string patternVar = @"([a-z-_]+)";
            string replaceNumber = " CONST[$1]";
            string replaceVar = " VAR[$1] ";

            result = Regex.Replace(result, patternNumber, replaceNumber);
            result = Regex.Replace(result, patternVar, replaceVar);

            Console.WriteLine("Eredmény 2 : {0}", result);*/
        }
    }
}
