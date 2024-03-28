namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report
{
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Properties;
    using Bars.GkhGjiCr.Report;

    public class ChelyabinskForm1StateHousingInspection : Form1StateHousingInspection
    {
        
        public ChelyabinskForm1StateHousingInspection()
            : base(new ReportTemplateBinary(Resources.Form1StateHousingInspection))
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

                reportParams.SimpleReportParams["ДатаОтчета"] = this.reportDate.ToShortDateString();

                base.PrepareReport(reportParams);
            }
            finally 
            {
                this.Container.Release(userManager);
            }
        }
    }
}
