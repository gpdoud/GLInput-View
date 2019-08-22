using System;
using System.Collections.Generic;
using System.Text;

namespace KenParseFileStrings {
    public struct LineFmt {
        public string Field { get; set; }
        public int Start { get; set; }
        public int Len { get; set; }
        public int LTrim { get; set; }
        public int RTrim { get; set; }
    }
    public class Vue {

        static Dictionary<string, LineFmt> layout = new Dictionary<string, LineFmt> {
            ["company_code"]    = new LineFmt() { Field = "company_code",  Start = 0,   Len = 5 },
            ["plan_code"]       = new LineFmt() { Field = "plan_code",     Start = 5,   Len = 8 },
            ["source"]          = new LineFmt() { Field = "source",        Start = 13,  Len = 5 },
            ["eas_account"]     = new LineFmt() { Field = "eas_account",   Start = 18,  Len = 10, LTrim = 3 },
            ["sap_account"]     = new LineFmt() { Field = "sap_account",   Start = 28,  Len = 10, LTrim = 3 },
            ["profit_center"]   = new LineFmt() { Field = "profit_center", Start = 38,  Len = 15, LTrim = 3 },
            ["policy_number"]   = new LineFmt() { Field = "policy_number", Start = 53,  Len = 15 },
            ["tran_date"]       = new LineFmt() { Field = "tran_date",     Start = 68,  Len = 8 },
            ["calendar_date"]   = new LineFmt() { Field = "calendar_date", Start = 76,  Len = 10 },
            ["ref_id"]          = new LineFmt() { Field = "ref_id",        Start = 86,  Len = 20 },
            ["amount"]          = new LineFmt() { Field = "amount",        Start = 106, Len = 15 },
            ["dbcr_ind"]        = new LineFmt() { Field = "dbcr_ind",      Start = 121, Len = 1 },
            ["desc"]            = new LineFmt() { Field = "desc",          Start = 122, Len = 50 },
            ["LOB"]             = new LineFmt() { Field = "LOB",           Start = 172, Len = 15 },
            ["budget_center"]   = new LineFmt() { Field = "budget_center", Start = 187, Len = 15 },
            ["rein_company"]    = new LineFmt() { Field = "rein_company",  Start = 202, Len = 15 },
            ["state"]           = new LineFmt() { Field = "state",         Start = 217, Len = 15 },
            ["tax_status"]      = new LineFmt() { Field = "tax_status",    Start = 232, Len = 15 },
            ["memo_code"]       = new LineFmt() { Field = "memo_code",     Start = 247, Len = 15 }
        };

        /// <summary>
        /// Extracts fields from the input string using parameters from the Dictionary which contains the 
        /// starting position of the field and the number of characters. It also allows for optionally
        /// trimming characters from the left and/or right of the extracted field.
        /// 
        /// Trimming characters is required to remove leading zeros on some account numbers. For this
        /// to operate properly, the right side it trimmed first then the left. It must be done in 
        /// this order.
        /// </summary>
        /// 
        /// <param name="line">
        /// This is a line from the input file. It is a single string with no
        /// delimiters. All fields are positional.
        /// </param>
        /// 
        /// <param name="fieldname">
        /// The name of the field to be extracted. It must be a key in 
        /// the dictionary.
        /// </param>
        /// 
        /// <param name="layout">
        /// The dictionary containing the line layout info.
        /// </param>
        /// 
        /// <returns>
        /// A field as a string extracted from the input.
        /// </returns>
        private string ParseField(string line, string fieldname,  Dictionary<string, LineFmt> layout) {
            var fieldLayout = layout[fieldname];
            if(fieldLayout.Start + fieldLayout.Len > line.Length) {
                return string.Empty;
            }
            var field = line.Substring(fieldLayout.Start, fieldLayout.Len).Trim();
            if(fieldLayout.RTrim > 0) {
                field = field.Substring(0, field.Length - fieldLayout.RTrim);
            }
            if(fieldLayout.LTrim > 0) {
                field = field.Substring(fieldLayout.LTrim);
            }
            return field;
        }

        private GLInput ParseLine(string line) {
            var glinput = new GLInput {
                company_code = ParseField(line, "company_code", layout),
                plan_code = ParseField(line, "plan_code", layout),
                source = ParseField(line, "source", layout),
                eas_account = ParseField(line, "eas_account", layout),
                sap_account = ParseField(line, "sap_account", layout),
                profit_center = ParseField(line, "profit_center", layout),
                tran_date = ParseField(line, "tran_date", layout),
                calendar_date = ParseField(line, "calendar_date", layout),
                ref_id = ParseField(line, "ref_id", layout),
                amount = decimal.Parse(ParseField(line, "amount", layout)),
                dbcr_ind = ParseField(line, "dbcr_ind", layout),
                desc = ParseField(line, "desc", layout),
                LOB = ParseField(line, "LOB", layout),
                budget_center = ParseField(line, "budget_center", layout),
                rein_company = ParseField(line, "rein_company", layout),
                state = ParseField(line, "state", layout),
                tax_status = ParseField(line, "tax_status", layout),
                memo_code = ParseField(line, "memo_code", layout)
            };
            return glinput;
        }
        public List<GLInput> ParseLines(string[] lines) {
            var glinputs = new List<GLInput>();
            foreach(var line in lines) {
                glinputs.Add(ParseLine(line));
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
