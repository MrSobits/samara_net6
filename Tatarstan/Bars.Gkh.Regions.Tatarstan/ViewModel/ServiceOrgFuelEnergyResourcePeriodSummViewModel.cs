namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using Bars.B4;

    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Представление <see cref="ServiceOrgFuelEnergyResourcePeriodSumm"/>
    /// </summary>
    public class ServiceOrgFuelEnergyResourcePeriodSummViewModel : BaseViewModel<ServiceOrgFuelEnergyResourcePeriodSumm>
    {
        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<ServiceOrgFuelEnergyResourcePeriodSumm> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var periodSumm = domainService.Get(id);

            var proxy = new PeriodSummProxy
            {
                Id = periodSumm.Id,

                Period = periodSumm.ContractPeriod.Name,
                Municipality = periodSumm.Municipality.Name,
                PublicServiceOrg = periodSumm.PublicServiceOrg.Contragent.Name,

                Charged = periodSumm.Details.SafeSum(y => y.Charged),
                Paid = periodSumm.Details.SafeSum(y => y.Paid),
                Debt = periodSumm.Details.SafeSum(y => y.Debt),
                PlanPayGas = periodSumm.Details.SafeSum(y => y.PlanPayGas),
                PlanPayElectricity = periodSumm.Details.SafeSum(y => y.PlanPayElectricity)
            };

            return new BaseDataResult(proxy);
        }

        private class PeriodSummProxy
        {
            public long Id { get; set; }

            public string Period { get; set; }

            public string Municipality { get; set; }

            public string PublicServiceOrg { get; set; }

            public decimal Charged { get; set; }

            public decimal Paid { get; set; }

            public decimal Debt { get; set; }

            public decimal PlanPayGas { get; set; }

            public decimal PlanPayElectricity { get; set; }
        }
    }
}
