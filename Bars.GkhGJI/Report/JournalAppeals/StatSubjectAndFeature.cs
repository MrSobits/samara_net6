namespace Bars.GkhGji.Report
{
    using System.Collections.Generic;

    public class StatSubjectAndFeature
    {
        public Dictionary<string, string> StatSubject { get; set; }

        public Dictionary<string, string> Feature { get; set; }
    }

    public class AppCitStatSubject
    {
        public long AppCitId { get; set; }

        public string SubjectCode { get; set; }

        public string SubsubjectCode { get; set; }
    }
}
