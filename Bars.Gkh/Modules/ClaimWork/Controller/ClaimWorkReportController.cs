namespace Bars.Gkh.Modules.ClaimWork.Controller
{
    using System.Linq;
    using B4;

    using Bars.B4.Utils;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.RegOperator.DomainService;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// 
    /// </summary>
    public class ClaimWorkReportController : BaseController
    {
        /// <summary>
        /// Получаю список сущностей для которых реализован IClaimWorkCodedReport + отчет по лс
        /// </summary>
        public ActionResult GetReportList(BaseParams baseParams)
        {
            var claimWorkReportProvider = this.Container.Resolve<IClaimWorkReportService>();
            var persAccReportProvider = this.Container.Resolve<IPersonalAccountReportService>();

            var reports = claimWorkReportProvider.GetReportList(baseParams);

            var codeForms = baseParams.Params["codeForm"].ToString().Split(',');
            if (!codeForms.Where(x => x.Equals("PersonalAccountReport")).IsEmpty())
            {
                reports.Add(persAccReportProvider.GetReportInfo(baseParams));
            }
            return new JsonListResult(reports);
        }

        /// <summary>
        /// Создание и вывод одной печатной формы
        /// </summary>
        public ActionResult ReportPrint(BaseParams baseParams)
        {
            var reportId = baseParams.Params.GetAs<string>("reportId");
            var isAccountReport = reportId.Equals("PersonalAccountReport") || reportId.Equals("PersonalAccountClaimworkReport");

            //var result = isAccountReport
            //    ? this.Container.Resolve<IPersonalAccountReportService>().GetReport(baseParams)       
            //    : this.Container.Resolve<IClaimWorkReportService>().GetReport(baseParams);

            //var isAccountReport = (reportId.Equals("PersonalAccountReport"));
            object result=null;
            switch (reportId)
            {
                case "PersonalAccountReport":
                case "PersonalAccountClaimworkReport":
                    {
                        result = this.Container.Resolve<IPersonalAccountReportService>().GetReport(baseParams);
                    };
                    break;
                case "CourtOrderAccountReport":
                case "CourtOrderAccountDeclarationReport":
                case "CourtOrderAccountDeclaration185Report":
                case "CourtOrderAccountDeclaration512Report":
                case "LawSuitAccountReport":
                case "CourtOrderPr":
                    {
                        result = this.Container.Resolve<IClaimWorkReportService>().GetAccountReport(baseParams);
                    };
                    break;
                default:
                    {
                        result = this.Container.Resolve<IClaimWorkReportService>().GetReport(baseParams);
                    };
                    break;
            }
            return new JsonNetResult(result);
        }

        /// <summary>
        /// Массовое создание печатных форм и сохранение на ftp
        /// </summary>
        public ActionResult MassReportPrint(BaseParams baseParams)
        {
            var reportId = baseParams.Params.GetAs<string>("reportId");
            var isAccountReport = (reportId.Equals("PersonalAccountReport"));          

            var result = isAccountReport
                ? this.Container.Resolve<IPersonalAccountReportService>().GetMassReport(baseParams)       
                : this.Container.Resolve<IClaimWorkReportService>().GetMassReport(baseParams);
            
            return new JsonNetResult(result);
        }

        /// <summary>
        /// Соформировать печатную форму для сведений о собственниках
        /// </summary>
        public ActionResult ReportLawsuitOnwerPrint(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IClaimWorkReportService>().GetLawsuitOnwerReport(baseParams);

            return new JsonNetResult(result);
        }

        /// <summary>
        /// Соформировать печатную форму для лицевых счетов
        /// </summary>
        public ActionResult ReportAccountPrint(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IClaimWorkReportService>().GetAccountReport(baseParams);

            return new JsonNetResult(result);
        }

        /// <summary>
        /// Соформировать массово печатную форму для лицевых счетов
        /// </summary>
        public ActionResult MassReportAccountPrint(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IClaimWorkReportService>().GetMassAccountReport(baseParams);

            return new JsonNetResult(result);
        }
    }

    public class ReportInfo
    {
        public string Id;
        public string Name;
        public string Description;
    }
}