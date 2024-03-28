namespace Bars.Gkh.RegOperator.Controllers.PersonalAccount
{
    using System.IO;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Utils;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils.Web;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.Repositories.ChargePeriod;

    public class PersonalAccountCalcDebtController : FileStorageDataController<PersonalAccountCalcDebt>
    {
        public IPersonalAccountCalcDebtService Service { get; set; }

        public ActionResult Export(BaseParams baseParams)
        {
            var result = this.Service.Export(baseParams);
            if (result.Success)
            {
                var reportResult = result as ReportResult;
                var res = reportResult.ReportStream as MemoryStream;
                return new DataResult(res.ToArray(), reportResult.FileName, ResultCode.Success);
            }

            return result.ToJsonResult();
        }

        public ActionResult Print(BaseParams baseParams)
        {
            var result = this.Service.Print(baseParams);
            if (result.Success)
            {
                var reportResult = result as ReportResult;
                var res = reportResult.ReportStream as MemoryStream;
                return new DataResult(res.ToArray(), reportResult.FileName, ResultCode.Success);
            }

            return result.ToJsonResult();
        }

        public ActionResult GetPeriodInfo()
        {
            var result = this.Service.GetPeriodInfo();

            return result.ToJsonResult();
        }
    }
}