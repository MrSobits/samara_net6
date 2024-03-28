namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Представление <see cref="FuelEnergyOrgContractInfo"/>
    /// </summary>
    public class FuelEnergyOrgContractInfoViewModel : BaseViewModel<FuelEnergyOrgContractInfo>
    {
        /// <summary>
        /// Менеджер пользователей
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<FuelEnergyOrgContractInfo> domainService, BaseParams baseParams)
        {
            var contractId = baseParams.Params.GetAs<long>("periodSummId");
            var contragentIds = this.UserManager.GetContragentIds();

            var loadParam = baseParams.GetLoadParam();

            var result = domainService.GetAll()
                .Where(x => x.PeriodSummary.Id == contractId)
                .WhereIf(contragentIds.IsNotEmpty(), x => contragentIds.Contains(x.FuelEnergyResourceContract.FuelEnergyResourceOrg.Contragent.Id))
                .AsEnumerable()
                .Select(
                    x =>
                        new FuelEnergyOrgContractInfoProxy
                        {
                            Id = x.Id,
                            FuelEnergyOrg = x.FuelEnergyResourceContract.FuelEnergyResourceOrg.Contragent.Name,
                            CommunalResource = x.Resource.Name,
                            SaldoIn = x.SaldoIn,
                            DebtIn = x.DebtIn,
                            Charged = x.Charged,
                            Paid = x.Paid,
                            SaldoOut = x.SaldoOut,
                            DebtOut = x.DebtOut,
                            PlanPaid = x.PlanPaid,
                            PaidDelta = x.PaidDelta
                        })
                .AsQueryable();

            var count = result.Count();

            return new ListDataResult(result.Order(loadParam).Paging(loadParam), count);
        }

        private class FuelEnergyOrgContractInfoProxy
        {
            public long Id { get; set; }

            public string FuelEnergyOrg { get; set; }

            public string CommunalResource { get; set; }

            public decimal SaldoIn { get; set; }

            public decimal DebtIn { get; set; }

            public decimal Charged { get; set; }

            public decimal Paid { get; set; }

            public decimal SaldoOut { get; set; }

            public decimal DebtOut { get; set; }

            public decimal PlanPaid { get; set; }

            public decimal PaidDelta { get; set; }
        }
    }
}
