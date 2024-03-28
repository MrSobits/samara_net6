namespace Bars.GkhGji.Domain.SpecialAccountReport.Serialize
{
    using System.Collections.Generic;

    public class Report
    {
        public virtual string ReportYear { get; set; }

        public virtual string ReportMonth { get; set; }
        
        public virtual ReportContragent Contragent { get; set; }

        public virtual ReportCreditOrg CreditOrg { get; set; }

        public List<ReportElement> Elements { get; set; } 
    }
}