namespace Bars.Gkh1468.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;

    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh1468.DomainService;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Entities.Passport;
    using Gkh.Utils;

    public class HousePassportViewModel : BaseViewModel<HousePassport>
    {
        private IHousePassportService _paspServ;

        public IHousePassportService PaspService
        {
            get
            {
                return _paspServ ?? (_paspServ = Container.Resolve<IHousePassportService>());
            }
        }

        public override IDataResult List(IDomainService<HousePassport> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var year = loadParams.Filter.Get("year", 0);
            var month = loadParams.Filter.Get("month", 0);

            var currOp = Container.Resolve<IGkhUserManager>().GetActiveOperator();

            if (currOp == null)
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
                    new OrderField { Asc = false, Name = "RealityObject" }
                };
            }

            var muIds = Container.Resolve<IDomainService<OperatorMunicipality>>()
                .GetAll()
                .Where(x => x.Operator.Id == currOp.Id)
                .Select(x => x.Municipality.Id)
                .Distinct()
                .ToList();


            var paspService = Container.Resolve<IDomainService<HouseProviderPassport>>();


            var query = domainService.GetAll()
                .Where(x => x.RealityObject != null)
                .WhereIf(muIds.Count > 0, 
                  x =>
                        muIds.Contains(x.RealityObject.Municipality.Id) ||
                        muIds.Contains(x.RealityObject.MoSettlement.Id))
                .WhereIf(year > 0, x => x.ReportYear == year)
                .WhereIf(month > 0, x => x.ReportMonth == month)
                .Select(x => new
                        {
                            x.Id,
                            RealityObjectId = x.RealityObject.Id,
                            RealityObject = x.RealityObject.Address,
                            x.Percent,
                            x.ReportYear,
                            x.ReportMonth,
                            NumberNotCreated = 0,
					Count = paspService.GetAll()
						.Where(z => z.HousePassport.Id == x.Id)
						.Count(z => z.State.Name != "Подписан и получен в МО")
                        })
                .Filter(loadParams, Container);

			//если пользователь сортирует по полю "Не проверенных паспортов", то сортируем данные в памяти
			if (loadParams.Order.Any(x => x.Name == "Count"))
			{
				query = query.ToArray().AsQueryable();
			}

            var count = query.Count();

            var data = query.Order(loadParams).Paging(loadParams)
                .Select(x => new
                    {
                        x.Id,
                        x.RealityObjectId,
                        x.RealityObject,
                        x.Percent,
                        x.ReportYear,
                        x.ReportMonth,
                        periodStartDate = new DateTime(x.ReportYear, x.ReportMonth, 1),
					x.Count
                    })
                .ToArray();

            var roPeriod = data
                .Select(x => new RoPeriodProxy { Id = x.RealityObjectId, Period = x.periodStartDate })
                .ToArray();

            var notCreatedDict = GetNotCtreatedPassportContragents(roPeriod);

            var result = data
                    .Select(x => new
                    {
                        x.Id,
                        x.RealityObjectId,
                        x.RealityObject,
                        x.Percent,
                        x.ReportYear,
                        x.ReportMonth,
						x.Count,
                        notCreatedData = notCreatedDict.ContainsKey(x.RealityObjectId) && notCreatedDict[x.RealityObjectId].ContainsKey(x.periodStartDate)
                            ? notCreatedDict[x.RealityObjectId][x.periodStartDate]
                            : new List<long>()
                    })
                    .Select(x => new
                    {
                        x.Id,
                        x.RealityObjectId,
                        x.RealityObject,
                        x.Percent,
                        x.ReportYear,
                        x.ReportMonth,
                        NumberNotCreated = x.notCreatedData.Count(),
                        ContragentsNotCreated = x.notCreatedData.Any() ? x.notCreatedData.Select(c => c.ToString()).AggregateWithSeparator(", ") : string.Empty,
						x.Count
                    });

            return new ListDataResult(result, count);
        }

        private class RoPeriodProxy
        {
            public long Id;
            public DateTime Period;
        }

        private class RoContragent
        {
            public long Id;
            public long ContrId;
            public DateTime? Period;
        }

        public IDomainService<ManOrgContractRealityObject> ManOrgDomain { get; set; }
        public IDomainService<ServiceOrgRealityObjectContract> ServOrgDomain { get; set; }
        public IDomainService<PublicServiceOrgContract> PubServOrgDomain { get; set; }
        public IDomainService<RealityObjectResOrg> ResOrgDomain { get; set; }
        public IDomainService<HouseProviderPassport> HpPasspDomain { get; set; }

        private IDictionary<long, Dictionary<DateTime?, IEnumerable<long>>> GetNotCtreatedPassportContragents(IEnumerable<RoPeriodProxy> roPeriods)
        {
            var contrList = new List<RoContragent>();
            var hasPasp = new List<RoContragent>();

            var roIds = roPeriods.Select(x => x.Id).ToList();

            var manOrgContractsByRo = ManOrgDomain.GetAll()
                .Where(x => x.ManOrgContract != null && x.ManOrgContract.ManagingOrganization != null)
                .Where(x => x.ManOrgContract.ManagingOrganization.Contragent != null)
                .Where(x => roIds.Contains(x.RealityObject.Id))
                .Select(x => new
                    {
                        x.RealityObject.Id, 
                        x.ManOrgContract.StartDate, 
                        x.ManOrgContract.EndDate, 
                        ContrId = x.ManOrgContract.ManagingOrganization.Contragent.Id
                    })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.ToList());

            var servOrgContractsByRo = ServOrgDomain.GetAll()
                .Where(x => x.ServOrgContract != null && x.ServOrgContract.ServOrg != null)
                .Where(x => x.ServOrgContract.ServOrg.Contragent != null)
                .Where(x => roIds.Contains(x.RealityObject.Id))
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.ServOrgContract.DateStart,
                    x.ServOrgContract.DateEnd,
                    ContrId = x.ServOrgContract.ServOrg.Contragent.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.ToList());

            var pubServOrgContractsByRo = PubServOrgDomain.GetAll()
                .Where(x => x.PublicServiceOrg != null)
                .Where(x => x.PublicServiceOrg.Contragent != null)
                .Where(x => roIds.Contains(x.RealityObject.Id))
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.DateStart,
                    x.DateEnd,
                    ContrId = x.PublicServiceOrg.Contragent.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.ToList());

            var resOrgContractsByRo = ResOrgDomain.GetAll()
                .Where(x => x.ResourceOrg != null && x.RealityObject != null)
                .Where(x => x.ResourceOrg.Contragent != null)
                .Where(x => roIds.Contains(x.RealityObject.Id))
                .Select(x => new
                {
                    x.RealityObject.Id,
                    x.DateStart,
                    x.DateEnd,
                    ContrId = x.ResourceOrg.Contragent.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.ToList());

           var passportsByRo = HpPasspDomain.GetAll()
               .Where(x => x.RealityObject != null)
               .Where(x => roIds.Contains(x.RealityObject.Id))
               .Select(x => new
                {
                    x.RealityObject.Id, 
                    x.ReportYear, 
                    x.ReportMonth, 
                    ContrId = x.Contragent.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.ToList());

            var roPeriodProxies = roPeriods as RoPeriodProxy[] ?? roPeriods.ToArray();

            foreach (var roPeriod in roPeriodProxies)
            {
                RoPeriodProxy item = roPeriod;

                // контрагенты УО
                if (manOrgContractsByRo.ContainsKey(item.Id))
                {
                    var manOrgContracts = manOrgContractsByRo[item.Id];

                    contrList.AddRange(manOrgContracts
                        .Where(x => x.StartDate.HasValue && x.StartDate.Value <= item.Period)
                        .Where(x => !x.EndDate.HasValue || (x.EndDate.HasValue && x.EndDate.Value >= item.Period))
                        .Select(x => new RoContragent { Id = x.Id, Period = item.Period, ContrId = x.ContrId })
                    .ToList());
                }

                // контрагенты ПЖУ
                if (servOrgContractsByRo.ContainsKey(item.Id))
                {
                    var servOrgContracts = servOrgContractsByRo[item.Id];

                    contrList.AddRange(servOrgContracts
                        .Where(x => x.DateStart.HasValue && x.DateStart.Value <= item.Period)
                        .Where(x => !x.DateEnd.HasValue || (x.DateEnd.HasValue && x.DateEnd.Value >= item.Period))
                        .Select(x => new RoContragent { Id = x.Id, Period = item.Period, ContrId = x.ContrId })
                    .ToList());
                }

                // контрагенты ПР
                if (pubServOrgContractsByRo.ContainsKey(item.Id))
                {
                    var pubServOrgContracts = pubServOrgContractsByRo[item.Id];

                    contrList.AddRange(pubServOrgContracts
                    .Where(x => x.DateStart.HasValue && x.DateStart.Value <= item.Period)
                        .Where(x => !x.DateEnd.HasValue || (x.DateEnd.HasValue && x.DateEnd.Value >= item.Period))
                        .Select(x => new RoContragent { Id = x.Id, Period = item.Period, ContrId = x.ContrId })
                    .ToList());
                }

                if (resOrgContractsByRo.ContainsKey(item.Id))
                {
                    var resOrgContracts = resOrgContractsByRo[item.Id];

                // контрагенты ПКУ
                    contrList.AddRange(resOrgContracts
                    .Where(x => x.DateStart.HasValue && x.DateStart.Value <= item.Period)
                    .Where(x => !x.DateEnd.HasValue || (x.DateEnd.HasValue && x.DateEnd.Value >= item.Period))
                        .Select(x => new RoContragent { Id = x.Id, Period = item.Period, ContrId = x.ContrId })
                    .ToList());
                }

                if (passportsByRo.ContainsKey(item.Id))
                {
                    var passports = passportsByRo[item.Id];

                    hasPasp.AddRange(passports
                    .Where(x => x.ReportYear == item.Period.Year && x.ReportMonth == item.Period.Month)
                        .Select(x => new RoContragent { Id = x.Id, Period = item.Period, ContrId = x.ContrId })
                    .ToList());
                }
            }

            var dict = contrList
                .Distinct(new Comparer())
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.AsEnumerable()
                    .Select(z => new RoContragent { Id = z.Id, Period = z.Period, ContrId = z.ContrId })
                    .GroupBy(a => a.Period)
                    .ToDictionary(b => b.Key, c => c.AsEnumerable()
                        .Where(d => !hasPasp.Any(e => e.Id == d.Id && e.ContrId == d.ContrId && e.Period == d.Period))
                        .Select(d => d.ContrId)));

            return dict;
        }


        class Comparer : IEqualityComparer<RoContragent>
        {
            public bool Equals(RoContragent x, RoContragent y)
            {
                return x.Id == y.Id && x.ContrId == y.ContrId && x.Period == y.Period;
            }

            public int GetHashCode(RoContragent item)
            {
                if (ReferenceEquals(item, null)) return 0;

                int hashProductId = item.Id.GetHashCode();

                int hashProductContrId = item.ContrId.GetHashCode();

                return hashProductId ^ hashProductContrId;
            }
        }
    }
}