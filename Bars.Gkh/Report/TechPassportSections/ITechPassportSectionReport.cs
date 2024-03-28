namespace Bars.Gkh.Report.TechPassportSections
{
    using System.Collections.Generic;
    using B4.Modules.Reports;
    using Bars.Gkh.PassportProvider;

    public interface ITechPassportSectionReport
    {
        void PrepareSection(IPassportProvider iPassportProvider, ReportParams reportParams, long realtyObjectId, 
            Dictionary<string, Dictionary<string, string>> techPassportData = null);
    }
}