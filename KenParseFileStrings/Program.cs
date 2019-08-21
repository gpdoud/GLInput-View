using System;
using System.Collections.Generic;

namespace KenParseFileStrings {
    class Program {
        static void Main(string[] args) {

            if(args.Length == 0) { throw new Exception("No file passed in"); }
            var inPath = args[0];
            var vue = new Vue();
            var lines = vue.LoadFile(inPath);
            var glinputCollection = vue.ParseLines(lines);
            var csvLines = new List<string>(glinputCollection.Count);
            foreach(var glinput in glinputCollection) {
                csvLines.Add(glinput.ToCsv());
            }
            var outPath = inPath.Replace(".txt", ".csv");
            System.IO.File.WriteAllLines(outPath, csvLines);
        }
    }
}
