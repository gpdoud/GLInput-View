using System;
using System.Collections.Generic;
using System.Text;

namespace KenParseFileStrings {
    public class Vue {

        List<int> inputFileLengths = new List<int>{
            5, // company_code start = 0, len = 5
            8, 5, 10, 10, 15, 15, 8, 10, 20, 15, 1, 50, 15, 15, 15, 15, 15, 15 };

        private GLInput ParseLine(string line) {
            var currPos = 0;
            var idx = 0;
            var p_company_code = line.Substring(currPos, inputFileLengths[idx]);
            currPos += inputFileLengths[idx++];
            var plan = line.Substring(currPos, inputFileLengths[idx]);
            currPos += inputFileLengths[idx++];
            var source = line.Substring(currPos, inputFileLengths[idx]);
            currPos += inputFileLengths[idx++];
            // the rest of the data
            var glinput = new GLInput() {
                company_code = p_company_code
            };
            return glinput;
        }
        public List<GLInput> ParseLines(string[] lines) {
            var glinputs = new List<GLInput>();
            foreach(var line in lines) {
                var glinputinst = ParseLine(line);
                glinputs.Add(glinputinst);
            }
            return glinputs;
        }

        public string[] LoadFile(string path) {
            var lines = System.IO.File.ReadAllLines(path);
            return lines;
        }

        public Vue() { }
    }
}
