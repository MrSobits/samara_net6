namespace Bars.Gkh.Regions.Tatarstan.Interceptors.PubServContractPeriodSumm
{
    using Bars.B4;
    using Bars.Gkh.Regions.Tatarstan.DomainService;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Интерцептор <see cref="ContractPeriodSummDetail" />
    /// </summary>
    public class ContractPeriodSummDetailInterceptor : EmptyDomainInterceptor<ContractPeriodSummDetail>
    {
        /// <summary>
        /// Интерфейс работы с расщеплением платежей
        /// </summary>
        public IChargeSplittingService ChargeSplittingService { get; set; }

        /// <summary>
        /// Метод вызывается перед обновлением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeUpdateAction(IDomainService<ContractPeriodSummDetail> service, ContractPeriodSummDetail entity)
        {
            var result = this.ChargeSplittingService.RecalcSummary(entity.ContractPeriodSumm);

            return result.Success ? base.BeforeUpdateAction(service, entity) : result;
        }
    }
}