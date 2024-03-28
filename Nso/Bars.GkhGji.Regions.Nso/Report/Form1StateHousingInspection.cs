namespace Bars.GkhGji.Regions.Nso.Report
{
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Authentification;
    using Bars.GkhGjiCr.Report;

    public class NsoForm1StateHousingInspection : Form1StateHousingInspection
    {
        
        public NsoForm1StateHousingInspection()
            : base(new ReportTemplateBinary(Properties.Resources.Form1StateHousingInspection))
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

                reportParams.SimpleReportParams["ДатаОтчета"] = reportDate.ToShortDateString();

                base.PrepareReport(reportParams);
            }
            finally 
            {
                Container.Release(userManager);
            }
        }
    }
}
