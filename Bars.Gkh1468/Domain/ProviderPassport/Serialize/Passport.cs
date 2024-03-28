namespace Bars.Gkh1468.Domain.ProviderPassport.Serialize
{
    using System.Collections.Generic;

    public class Passport
    {
        public virtual int ReportYear { get; set; }

        public virtual int ReportMonth { get; set; }

        public virtual string State { get; set; }
        
        public virtual PassportContragent Contragent { get; set; }
        
        public virtual decimal Percent { get; set; }

        public List<PassportElement> Elements { get; set; } 
    }
}