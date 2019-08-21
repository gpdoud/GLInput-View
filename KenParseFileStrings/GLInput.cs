using System;
using System.Collections.Generic;
using System.Text;

namespace KenParseFileStrings {

    public class GLInput {

        public string company_code { get; set; }
        public string plan_code { get; set; }
        public string source { get; set; }
        public string eas_account { get; set; }
        public string sap_account { get; set; }
        public string profit_center { get; set; }
        public string policy_number { get; set; }
        public string tran_date { get; set; }
        public string calendar_date { get; set; }
        public string ref_id { get; set; }
        public decimal amount { get; set; }
        public string dbcr_ind { get; set; }
        public string desc { get; set; }
        public string LOB { get; set; }
        public string budget_center { get; set; }
        public string rein_company { get; set; }
        public string state { get; set; }
        public string tax_status { get; set; }
        public string memo_code { get; set; }

        public string ToCsv() {
            var csvStrings = new string[] {
                this.company_code,
                this.plan_code,
                this.source,
                this.eas_account,
                this.sap_account,
                this.profit_center,
                this.policy_number,
                this.tran_date,
                this.calendar_date,
                this.ref_id,
                this.amount.ToString(),
                this.dbcr_ind,
                this.desc,
                this.LOB,
                this.budget_center,
                this.rein_company,
                this.state,
                this.tax_status,
                this.memo_code
            };
            var csvLine = string.Join(',', csvStrings);
            return csvLine;
        }

    }
}
