namespace Bars.GkhGji.Report
{
    using System;

    public sealed class AppealCitProxy
    {
        public long Id { get; set; }

        public DateTime? DateFrom { get; set; }

        public string NumberGji { get; set; }

        public string Municipality { get; set; }
        
        public string Correspondent { get; set; }

        public bool IsFile { get; set; }

        public string KindStatement { get; set; }

        public string Description { get; set; }

        public string PreviousAppealCits { get; set; }

        public string ManagingOrganization { get; set; }

        public string Surety { get; set; }

        public string SuretyResolve { get; set; }

        public DateTime? SuretyDate { get; set; }

        public string Executant { get; set; }

        public string Tester { get; set; }

        public DateTime? CheckTime { get; set; }

    }
}
