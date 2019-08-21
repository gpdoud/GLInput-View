using System;
using System.Collections.Generic;
using System.Text;

namespace KenParseFileStrings {
    public struct FieldLayout {
        public string Fieldname { get; set; }
        public int Start { get; set; }
        public int Len { get; set; }
    }
    public class Vue {

        Dictionary<string, FieldLayout> layout = new Dictionary<string, FieldLayout> {
            ["company_code"]    = new FieldLayout() { Fieldname = "company_code",     Start = 0,      Len = 5 },
            ["plan_code"]       = new FieldLayout() { Fieldname = "plan_code",        Start = 5,      Len = 8 },
            ["source"]          = new FieldLayout() { Fieldname = "source",           Start = 13,     Len = 5 },
            ["eas_account"]     = new FieldLayout() { Fieldname = "eas_account",      Start = 18,     Len = 10 },
            ["sap_account"]     = new FieldLayout() { Fieldname = "sap_account",      Start = 28,     Len = 10 },
            ["profit_center"]   = new FieldLayout() { Fieldname = "profit_center",    Start = 38,     Len = 15 },
            ["policy_number"]   = new FieldLayout() { Fieldname = "policy_number",    Start = 53,     Len = 15 },
            ["tran_date"]       = new FieldLayout() { Fieldname = "tran_date",        Start = 68,     Len = 8 },
            ["calendar_date"]   = new FieldLayout() { Fieldname = "calendar_date",    Start = 76,     Len = 10 },
            ["ref_id"]          = new FieldLayout() { Fieldname = "ref_id",           Start = 86,     Len = 20 },
            ["amount"]          = new FieldLayout() { Fieldname = "amount",           Start = 106,    Len = 15 },
            ["dbcr_ind"]        = new FieldLayout() { Fieldname = "dbcr_ind",         Start = 121,    Len = 1 },
            ["desc"]            = new FieldLayout() { Fieldname = "desc",             Start = 122,    Len = 50 },
            ["LOB"]             = new FieldLayout() { Fieldname = "LOB",              Start = 172,    Len = 15 },
            ["budget_center"]   = new FieldLayout() { Fieldname = "budget_center",    Start = 187,    Len = 15 },
            ["rein_company"]    = new FieldLayout() { Fieldname = "rein_company",     Start = 202,    Len = 15 },
            ["state"]           = new FieldLayout() { Fieldname = "state",            Start = 217,    Len = 15 },
            ["tax_status"]      = new FieldLayout() { Fieldname = "tax_status",       Start = 232,    Len = 15 },
            ["memo_code"]       = new FieldLayout() { Fieldname = "memo_code",        Start = 247,    Len = 15 }
        };

        private string ParseField(string line, string fieldname,  Dictionary<string, FieldLayout> layout) {
            var fieldLayout = layout[fieldname];
            if(fieldLayout.Start + fieldLayout.Len > line.Length) {
                return string.Empty;
            }
            return line.Substring(fieldLayout.Start, fieldLayout.Len).Trim();
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
