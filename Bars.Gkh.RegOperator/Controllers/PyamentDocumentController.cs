namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Config;
    using B4.Utils;

    using Bars.Gkh.Entities;

    using Entities;
    using Gkh.Domain;

    public class PeriodPaymentDocumentsController : BaseController
    {
        public IViewModel<PeriodPaymentDocuments> ViewModel { get; set; }
        public IDomainService<PeriodPaymentDocuments> DomainService { get; set; }
        public IDomainService<ChargePeriod> PeriodDomain { get; set; }
        public IConfigProvider Config { get; set; }

        public ActionResult List(BaseParams baseParams)
        {
            return new JsonNetResult(ViewModel.List(DomainService, baseParams));
        }

        public ActionResult GetLink(BaseParams @params)
        {
            var periodId = @params.Params.GetAsId("periodid");
            ChargePeriod period = null;

            if (periodId > 0)
            {
                period = PeriodDomain.Get(periodId);
            }

            var ftpHost = Config.GetConfig().AppSettings.GetAs<string>("FtpHost");

            if (ftpHost.IsEmpty())
            {
                return JsFailure("Не найден адрес ftp сервера!");
            }

            var link = "<a href=\"{0}/{1}\" style=\"color: #08c;\">{0}/{1}</a>".FormatUsing(ftpHost, period.Return(x => x.Name));

            return new JsonNetResult(new {success = true, data = link});
        }
    }
}