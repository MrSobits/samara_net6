namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;

    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Tatarstan.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Представление <see cref="FuelEnergyOrgContractDetail"/>
    /// </summary>
    public class FuelEnergyOrgContractDetailViewModel : BaseViewModel<FuelEnergyOrgContractDetail>
    {
        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="FuelEnergyOrgContractInfo"/>
        /// </summary>
        public IDomainService<FuelEnergyOrgContractInfo> FuelEnergyOrgContractInfoDomain { get; set; }

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<FuelEnergyOrgContractDetail> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var contragentIds = this.UserManager.GetContragentIds();

            var periodId = baseParams.Params.GetAs<long>("period");
            var municipalityIds = baseParams.Params.GetAs("municipality", new List<long>());
            var pubServOrgIds = baseParams.Params.GetAs("pubServOrg", new List<long>());

            var infoQuery = this.FuelEnergyOrgContractInfoDomain.GetAll()
                .WhereIfContains(contragentIds.IsNotEmpty(), x => x.FuelEnergyResourceContract.FuelEnergyResourceOrg.Contragent.Id, contragentIds);

            // записи видят ТЭР или РСО, для этого добавил "или" условие
            var query = domainService.GetAll()
                .Where(x => x.PeriodSumm.ContractPeriod.Id == periodId)
                .WhereIf(contragentIds.IsNotEmpty(), x => contragentIds.Contains(x.PeriodSumm.PublicServiceOrg.Contragent.Id) || infoQuery.Any(y => y.PeriodSummary == x.PeriodSumm))
                .WhereIfContains(municipalityIds.IsNotEmpty(), x => x.PeriodSumm.Municipality.Id, municipalityIds)
                .WhereIfContains(pubServOrgIds.IsNotEmpty(), x => x.PeriodSumm.PublicServiceOrg.Id, pubServOrgIds)
                .Select(
                    x =>
                        new FuelEnergyOrgContractDetailProxy
                        {
                            Id = x.Id,
                            PeriodSummId = x.PeriodSumm.Id,
                            Municipality = x.PeriodSumm.Municipality.Name,
                            PublicServiceOrg = x.PeriodSumm.PublicServiceOrg.Contragent.Name,
                            Service = x.Service.Name,
                            Charged = x.Charged,
                            Paid = x.Paid,
                            Debt = x.Debt,
                            GasEnergyPercents = x.GasEnergyPercents.Percentage,
                            ElectricityEnergyPercents = x.ElectricityEnergyPercents.Percentage,
                        });

            var count = query.Count();

            // переопределяем сортировку, чтобы на выходе были группы по PeriodSummaryId или PublicServiceOrg
            this.SetGroupOrder(loadParam);

            return new ListDataResult(query.Order(loadParam).Paging(loadParam).ToList(), count);
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<FuelEnergyOrgContractDetail> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var contractDetail = domainService.Get(id);

            var proxy = new FuelEnergyOrgContractDetailProxy
            {
                Id = contractDetail.Id,
                Period = contractDetail.PeriodSumm.ContractPeriod.Name,
                Municipality = contractDetail.PeriodSumm.Municipality.Name,
                PublicServiceOrg = contractDetail.PeriodSumm.PublicServiceOrg.Contragent.Name,
                Charged = contractDetail.Charged,
                Paid = contractDetail.Paid,
                Debt = contractDetail.Debt
            };

            return new BaseDataResult(proxy);
        }

        private class FuelEnergyOrgContractDetailProxy
        {
            public long Id { get; set; }

            public long PeriodSummId { get; set; }

            public string Period { get; set; }

            public string Municipality { get; set; }

            public string PublicServiceOrg { get; set; }

            public decimal Charged { get; set; }

            public decimal Paid { get; set; }

            public decimal Debt { get; set; }

            public decimal GasEnergyPercents { get; set; }

            public decimal ElectricityEnergyPercents { get; set; }

            public string Service { get; set; }

            public decimal PlanPayGas => this.Paid * this.GasEnergyPercents / 100.0m;

            public decimal PlanPayElectricity => this.Paid * this.ElectricityEnergyPercents / 100.0m;
        }

        private void SetGroupOrder(LoadParam loadParam)
        {
            // если не пришли сортировки, то просто добавляем групповую сортировку
            if (loadParam.Order.IsEmpty())
            {
                loadParam.Order = new[] { new OrderField { Asc = true, Name = "PeriodSummId" } };
                return;
            }

            // если сортируется по РСО, то и так группы будут верные
            if (loadParam.Order.First().Name == "PublicServiceOrg")
            {
                return;
            }

            // если пришла сортировка по PublicServiceOrg, но не первая, то переопределяем очередность сортировок
            if (loadParam.Order.Any(x => x.Name == "PublicServiceOrg"))
            {
                loadParam.Order = new[]
                    {
                        new OrderField { Asc = true, Name = "PublicServiceOrg" }
                    }
                    .Union(loadParam.Order)
                    .DistinctBy(x => x.Name)
                    .ToArray();
            }

            // иначе добавляем сортировку по групповому ключу и всё
            loadParam.Order = new[]
                {
                    new OrderField { Asc = true, Name = "PeriodSummId" }
                }
                .Union(loadParam.Order)
                .DistinctBy(x => x.Name)
                .ToArray();
        }
    }
}
