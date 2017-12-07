using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BCClear
{
    public class Program
    {      
        public static void Main(string[] args)
        {
            string igFolder = "C:\\VWorks Workspace\\Barcode Input Files";
            string tAssayFile = "AssayNexar.csv";
            string tDaughtFile = "DaugNexar.csv";
            string tExtFile = "Ext.csv";
            clearExtCSVFileContent(igFolder, tExtFile);
            clearDaugCSVFileContent(igFolder, tDaughtFile);
            clearAssayCSVFileContent(igFolder, tAssayFile);
        }


        public static void clearExtCSVFileContent(string igFolder,string fileName)
        {
            string fileDir = igFolder + "\\" + fileName;
            /* generate csv file */
            string lineTitle = File.ReadAllLines(fileDir).First();
            string[] lines = lineTitle.Split(',');
            var csvText = new StringBuilder();
            var newLine = string.Format("{0},{1},{2}", lines[0], lines[1], lines[2]);
            csvText.AppendLine(newLine);
            File.WriteAllText(fileDir,csvText.ToString());
        }

        public static void clearDaugCSVFileContent(string igFolder, string fileName)
        {
            string fileDir = igFolder + "\\" + fileName;
            /* generate csv file */
            string lineTitle = File.ReadAllLines(fileDir).First();
            string[] lines = lineTitle.Split(',');
            var csvText = new StringBuilder();
            var newLine = string.Format("{0},{1},{2},{3}", lines[0], lines[1], lines[2],lines[3]);
            csvText.AppendLine(newLine);
            File.WriteAllText(fileDir, csvText.ToString());
        }

        public static void clearAssayCSVFileContent(string igFolder, string fileName)
        {
            string fileDir = igFolder + "\\" + fileName;
            /* generate csv file */
            string lineTitle = File.ReadAllLines(fileDir).First();
            string[] lines = lineTitle.Split(',');
            var csvText = new StringBuilder();
            var newLine = string.Format("{0},{1},{2}", lines[0], lines[1], lines[2]);
            csvText.AppendLine(newLine);
            File.WriteAllText(fileDir, csvText.ToString());
        }
    }
}
