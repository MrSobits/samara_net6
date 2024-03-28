namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report
{
    using System;

    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;

    public class NosHouseTechPassportReport : HouseTechPassportReport
    {

        public NosHouseTechPassportReport()
            : base(new ReportTemplateBinary(Resources.HouseTechPassportReport))
        {
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

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
                this.Container.Release(userManager);
            }
        }
    }
}
