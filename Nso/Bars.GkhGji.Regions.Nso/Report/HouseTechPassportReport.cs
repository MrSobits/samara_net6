namespace Bars.GkhGji.Regions.Nso.Report
{
    using System;

    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Report;

    public class NosHouseTechPassportReport : HouseTechPassportReport
    {

        public NosHouseTechPassportReport()
            : base(new ReportTemplateBinary(Properties.Resources.HouseTechPassportReport))
        {
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            try
            {
                var user = userManager.GetActiveUser();

                if (user != null)
                {
                    reportParams.SimpleReportParams["Пользователь"] = user.Name;    
                }

                reportParams.SimpleReportParams["ДатаОтчета"] = DateTime.Today.ToShortDateString();

                base.PrepareReport(reportParams);
            }
            finally 
            {
                Container.Release(userManager);
            }
        }
    }
}
