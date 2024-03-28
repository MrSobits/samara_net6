using System.Collections.Generic;

namespace Bars.Gkh1468.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.DomainService;
    using Gkh.Utils;

    public class OkiPassportViewModel : BaseViewModel<OkiPassport>
    {
        private IOkiPassportService _paspServ;

        public IOkiPassportService PaspService
        {
            get
            {
                return _paspServ ?? (_paspServ = Container.Resolve<IOkiPassportService>());
            }
        }

        public override IDataResult List(IDomainService<OkiPassport> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var year = loadParams.Filter.Get("year", (long)0);
            var month = loadParams.Filter.Get("month", (long)0);

            var curDate = DateTime.Now;
            var curYear = curDate.Year;
            var curMonth = curDate.Month;

            var currentUser = Container.Resolve<IUserIdentity>();

            if (currentUser is AnonymousUserIdentity)
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

            var curOp =
                Container.Resolve<IDomainService<Operator>>()
                         .GetAll()
                         .FirstOrDefault(x => x.User.Id == currentUser.UserId);

            if (curOp == null)
            {
                return new ListDataResult();
            }

            var muList =
                Container.Resolve<IDomainService<OperatorMunicipality>>()
                         .GetAll()
                         .Where(x => x.Operator.Id == curOp.Id)
                         .Select(x => x.Municipality.Id)
                         .ToList();

            var paspService = Container.Resolve<IDomainService<OkiProviderPassport>>();
            var dict =
                paspService.GetAll()
                           .Where(x => muList.Contains(x.Municipality.Id))
                           .GroupBy(x => x.OkiPassport.Id)
                           .ToDictionary(
                               x => x.Key,
                               y => y.Count(z => z.State != null && z.State.Name == "Подписан и получен в МО"));

            var data =
                domainService.GetAll()
                             .WhereIf(year > 0, x => x.ReportYear == year)
                             .WhereIf(month > 0, x => x.ReportMonth == month)
                             .Where(x => muList.Contains(x.Municipality.Id) || muList.Contains(x.Municipality.ParentMo.Id))
                             .Select(
                                 x =>
                                 new
                                     {
                                         x.Id,
                                         Municipality = x.Municipality.Name,
                                         MunicipalityId = x.Municipality.Id,
                                         x.Percent,
                                         x.ReportYear,
                                         x.ReportMonth,
                                         NumberNotCreated = 0,
                                         Count = 0
                                     })
                             .Filter(loadParams, Container);
            var dataForResult = data.Order(loadParams).Paging(loadParams).ToArray();

            var notCreatedDict = GetNotCtreatedPassportContragents(muList);

            var result =
                dataForResult.Select(
                    x =>
                    new
                        {
                            x.Id,
                            x.Municipality,
                            x.Percent,
                            x.ReportYear,
                            x.ReportMonth,
                            NumberNotCreated = notCreatedDict[x.MunicipalityId].Count(),
                            ContragentsNotCreated = notCreatedDict[x.MunicipalityId].Select(y => y.ToString()).AggregateWithSeparator(", "),
                            Count = dict.ContainsKey(x.Id) ? dict[x.Id] : 0
                        });

            return new ListDataResult(result, result.Count());
        }

        public IDomainService<ManagingOrgMunicipality> ManOrgDomain { get; set; }
        public IDomainService<ServiceOrgMunicipality> ServOrgDomain { get; set; }
        public IDomainService<PublicServiceOrgMunicipality> PubServOrgDomain { get; set; }
        public IDomainService<SupplyResourceOrgMunicipality> ResOrgDomain { get; set; }
        public IDomainService<OkiProviderPassport> OpPasspDomain { get; set; }

        private Dictionary<long,IEnumerable<long>> GetNotCtreatedPassportContragents(IEnumerable<long> moIds)
        {
            var moids = moIds.ToArray();

            var list = ManOrgDomain.GetAll()
                            .Where(x => moids.Contains(x.Municipality.Id))
                            .Select(x => new {x.Municipality.Id, ContrId = x.ManOrg.Contragent.Id})
                            .ToList();

            list.AddRange(ServOrgDomain.GetAll()
                           .Where(x => moids.Contains(x.Municipality.Id))
                           .Select(x => new { x.Municipality.Id, ContrId = x.ServOrg.Contragent.Id })
                           .ToList());

            list.AddRange(PubServOrgDomain.GetAll()
                           .Where(x => moids.Contains(x.Municipality.Id))
                           .Select(x => new { x.Municipality.Id, ContrId = x.PublicServiceOrg.Contragent.Id })
                           .ToList());

            list.AddRange(ResOrgDomain.GetAll()
                           .Where(x => moids.Contains(x.Municipality.Id))
                           .Select(x => new { x.Municipality.Id, ContrId = x.SupplyResourceOrg.Contragent.Id })
                           .ToList());

            var hasPasp = OpPasspDomain.GetAll()
                .Where(x => moids.Contains(x.Municipality.Id))
                .Select(x => new { x.Municipality.Id, ContrId = x.Contragent.Id })
                .ToArray();

            var dict = list.GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.AsEnumerable()
                    .Where(z => !hasPasp.Any(a => a.Id == z.Id && z.ContrId == a.ContrId))
                    .Distinct(z => z.ContrId)
                    .Select(z => z.ContrId));

            return dict;
        }
    }
}