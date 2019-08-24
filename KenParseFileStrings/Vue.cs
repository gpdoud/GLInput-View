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
        public bool LTrimZero { get; set; }
        public bool DbCrInt { get; set; }
    }
    public enum Field {
        CompanyCode, PlanCode, Source, EasAccount, SapAccount, ProfitCenter, PolicyNumber,
        TranDateMonth, TranDateDay, TranDateYear,
        CalDateMonth, CalDateYear, RefId, Amount, DbCrId, Description, LineOfBusiness,
        BudgetCenter, ReinsuranceCo, State, TaxStatus, MemoCode
    }
    public class Vue {

        static Dictionary<Field, LineFmt> layout = new Dictionary<Field, LineFmt> {
            [Field.CompanyCode]    = new LineFmt() { Field = "company_code",    Start =   0, Len =  5 },
            [Field.PlanCode]       = new LineFmt() { Field = "plan_code",       Start =   5, Len =  8 },
            [Field.Source]         = new LineFmt() { Field = "source",          Start =  13, Len =  5 },
            [Field.EasAccount]     = new LineFmt() { Field = "eas_account",     Start =  18, Len = 10, LTrim = 3 },
            [Field.SapAccount]     = new LineFmt() { Field = "sap_account",     Start =  28, Len = 10, LTrim = 3 },
            [Field.ProfitCenter]   = new LineFmt() { Field = "profit_center",   Start =  38, Len = 15, LTrim = 3 },
            [Field.PolicyNumber]   = new LineFmt() { Field = "policy_number",   Start =  53, Len = 15 },
            [Field.TranDateMonth]  = new LineFmt() { Field = "tran_date_month", Start =  68, Len =  2 },
            [Field.TranDateDay]    = new LineFmt() { Field = "tran_date_month", Start =  70, Len =  2 },
            [Field.TranDateYear]   = new LineFmt() { Field = "tran_date_month", Start =  72, Len =  4 },
            [Field.CalDateMonth]   = new LineFmt() { Field = "cal_date_month",  Start =  76, Len =  2 },
            [Field.CalDateYear]    = new LineFmt() { Field = "cal_date_year",   Start =  82, Len =  4 },
            [Field.RefId]          = new LineFmt() { Field = "ref_id",          Start =  86, Len = 20 },
            [Field.Amount]         = new LineFmt() { Field = "amount",          Start = 106, Len = 15, LTrimZero = true },
            [Field.DbCrId]         = new LineFmt() { Field = "dbcr_ind",        Start = 121, Len =  1, DbCrInt = true },
            [Field.Description]    = new LineFmt() { Field = "desc",            Start = 122, Len = 50 },
            [Field.LineOfBusiness] = new LineFmt() { Field = "LOB",             Start = 172, Len = 15 },
            [Field.BudgetCenter]   = new LineFmt() { Field = "budget_center",   Start = 187, Len = 15 },
            [Field.ReinsuranceCo]  = new LineFmt() { Field = "rein_company",    Start = 202, Len = 15 },
            [Field.State]          = new LineFmt() { Field = "state",           Start = 217, Len = 15 },
            [Field.TaxStatus]      = new LineFmt() { Field = "tax_status",      Start = 232, Len = 15 },
            [Field.MemoCode]       = new LineFmt() { Field = "memo_code",       Start = 247, Len = 15 }
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
        private string ParseField(string line, Field fieldname,  Dictionary<Field, LineFmt> layout) {
            var fieldLayout = layout[fieldname];
            if(fieldLayout.Start + fieldLayout.Len > line.Length) {
                return string.Empty;
            }
            var field = line.Substring(fieldLayout.Start, fieldLayout.Len).Trim();
            if(fieldLayout.RTrim > 0) {
                field = RightTrimChars(field, field.Length - fieldLayout.RTrim);
            }
            if(fieldLayout.LTrim > 0) {
                field = LeftTrimChars(field, fieldLayout.LTrim);
            }
            if(fieldLayout.LTrimZero) {
                field = LeftTrimZeros(field);
            }
            if(fieldLayout.DbCrInt) {
                field = TranslateDbCrInd(field);
            }
            return field;
        }
        private string RightTrimChars(string data, int nbrChars) {
            return data.Substring(0, data.Length - nbrChars);
        }
        private string LeftTrimChars(string data, int nbrChars) {
            return data.Substring(nbrChars);
        }
        private string LeftTrimZeros(string data) {
            return data.TrimStart('0');
        }
        private string TranslateDbCrInd(string plusMinus) {
            return plusMinus.Equals("+") ? "D" : "C";
        }


        private GLInput ParseLine(string line) {
            var glinput = new GLInput {
                company_code    = ParseField(line, Field.CompanyCode,       layout),
                plan_code       = ParseField(line, Field.PlanCode,          layout),
                source          = ParseField(line, Field.Source,            layout),
                eas_account     = ParseField(line, Field.EasAccount,        layout),
                sap_account     = ParseField(line, Field.SapAccount,        layout),
                profit_center   = ParseField(line, Field.ProfitCenter,      layout),
                tran_date_month = ParseField(line, Field.TranDateMonth,     layout),
                tran_date_day   = ParseField(line, Field.TranDateDay,       layout),
                tran_date_year  = ParseField(line, Field.TranDateYear,      layout),
                cal_date_month  = ParseField(line, Field.CalDateMonth,      layout),
                cal_date_year   = ParseField(line, Field.CalDateYear,       layout),
                ref_id          = ParseField(line, Field.RefId,             layout),
                amount          = ParseField(line, Field.Amount,            layout),
                dbcr_ind        = ParseField(line, Field.DbCrId,            layout),
                desc            = ParseField(line, Field.Description,       layout),
                LOB             = ParseField(line, Field.LineOfBusiness,    layout),
                budget_center   = ParseField(line, Field.BudgetCenter,      layout),
                rein_company    = ParseField(line, Field.ReinsuranceCo,     layout),
                state           = ParseField(line, Field.State,             layout),
                tax_status      = ParseField(line, Field.TaxStatus,         layout),
                memo_code       = ParseField(line, Field.MemoCode,          layout)
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
