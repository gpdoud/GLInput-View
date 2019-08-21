using System;

namespace KenParseFileStrings {
    class Program {
        static void Main(string[] args) {

            var vue = new Vue();
            var lines = vue.LoadFile(@"C:\repos\KenParseFileStrings\KenParseFileStrings\vue.txt");
            vue.ParseLines(lines);

        }
    }
}
