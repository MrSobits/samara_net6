namespace Bars.Gkh1468.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.DomainService;
    using Bars.Gkh1468.Entities;

    using Gkh.Authentification;

    public class OkiProviderPassportViewModel : BaseProviderPassportViewModel<OkiProviderPassport>
    {
        private IOkiPassportService _paspServ;

        public OkiProviderPassportViewModel(IFileManager fileManager)
            : base(fileManager)
        {
        }

        public IOkiPassportService PaspService
        {
            get
            {
                return _paspServ ?? (_paspServ = Container.Resolve<IOkiPassportService>());
            }
        }

        public override IDataResult List(IDomainService<OkiProviderPassport> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var year = loadParams.Filter.Get("year", (long)0);
            var month = loadParams.Filter.Get("month", (long)0);
            var signyFilter = string.Empty;
            OrderField signyOrder = null;

            if (loadParams.CheckRuleExists("Signy"))
            {
                signyFilter = loadParams.GetRuleValue("Signy").ToStr();
                loadParams.DeleteRule("Signy");
            }

            var curOp = Container.Resolve<IGkhUserManager>().GetActiveOperator();

            if (curOp == null || curOp.Contragent == null)
            {
                return new ListDataResult();
            }

            // Если с клиента не пришла сортировка - то сортируем по году и месяцу
            if (loadParams.Order.Length == 0)
            {
                loadParams.Order = new[]
                                       {
                                           new OrderField { Asc = false, Name = "ReportYear" },
                                           new OrderField { Asc = false, Name = "ReportMonth" },
                                           new OrderField { Asc = false, Name = "Municipality" }
                                       };
            }
            else if (loadParams.Order.Any(x => x.Name == "Signy"))
            {
                signyOrder = loadParams.Order.FirstOrDefault(x => x.Name == "Signy");
                loadParams.Order = loadParams.Order.Where(x => x.Name != "Signy").ToArray();
            }

            var data =
                domainService.GetAll()
                             .Where(x => x.Contragent.Id == curOp.Contragent.Id)
                             .WhereIf(year != 0, x => x.ReportYear == year)
                             .WhereIf(month != 0, x => x.ReportMonth == month)
                             .Select(
                                 x =>
                                 new
                                     {
                                         x.Id,
                                         x.State,
                                         x.ObjectCreateDate,
                                         x.SignDate,
                                         x.Percent,
                                         x.ReportYear,
                                         x.ReportMonth,
                                         Municipality = x.Municipality.Name,
                                         Contragent = x.Contragent.Name,
                                         x.Certificate,                               
                                         x.UserName
                                     })
                             .Filter(loadParams, Container)
                             .Order(loadParams)
                             .Paging(loadParams);
                             
            var result = data
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.State,
                    x.ObjectCreateDate,
                    x.SignDate,
                    x.Percent,
                    x.ReportYear,
                    x.ReportMonth,
                    x.Contragent,
                    x.Municipality,
                    x.UserName,
                    Signy = GetSigny(x.Certificate)
                })
                .Where(x => signyFilter == string.Empty || (x.Signy != null && x.Signy.Contains(signyFilter)))
                .ToArray();

            if (signyOrder != null)
            {
                result = signyOrder.Asc ? result.OrderBy(x => x.Signy).ToArray() : result.OrderByDescending(x => x.Signy).ToArray();
            }

            return new ListDataResult(result, data.Count());
        }
    }
}