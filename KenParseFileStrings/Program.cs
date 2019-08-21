using System;
using System.Collections.Generic;

namespace KenParseFileStrings {
    class Program {
        static void Main(string[] args) {

            var vue = new Vue();
            var lines = vue.LoadFile(@"C:\repos\KenParseFileStrings\KenParseFileStrings\vue.txt");
            var glinputCollection = vue.ParseLines(lines);
            var csvLines = new List<string>(glinputCollection.Count);
            foreach(var glinput in glinputCollection) {
                csvLines.Add(glinput.ToCsv());
            }
            System.IO.File.WriteAllLines(@"C:\repos\KenParseFileStrings\KenParseFileStrings\vue.csv", csvLines);
        }
    }
}
