namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.ViewModel
{
    using System.Linq;
    using B4;

    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Enums;

    using Gkh.Domain;
    using Entities;

    using Bars.Gkh.Utils;
    using Gkh.Entities;

    /// <summary>
    /// ViewModel для Собственник в исковом заявлении
    /// </summary>
    public class DebtorReferenceCalculationViewModel : BaseViewModel<DebtorReferenceCalculation>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<DebtorReferenceCalculation> domainService, BaseParams baseParams)
        {
            var debtorId = baseParams.Params.GetAs("debtorId", 0L);
            var periodDomain = this.Container.Resolve<IDomainService<ChargePeriod>>().GetAll();

            var loadParam = this.GetLoadParam(baseParams);
            return domainService.GetAll()
                .Where(x => x.AgentPIRDebtor.Id == debtorId)
                .Join(periodDomain, x => x.PeriodId, y => y.Id, (x, y) => new
                {
                    x.Id,
                    x.AccountNumber,
                    y.Name,
                    y.StartDate,
                    y.EndDate,
                    x.AreaShare,
                    x.BaseTariff,
                    x.RoomArea,
                    x.PaymentDate,
                    x.TarifDebt,
                    x.TarifDebtPay,
                    x.TariffCharged,
                    x.TarifPayment,
                    x.Description,
                    x.Penalties,
                    x.AccrualPenalties,
                    x.PenaltyPayment,
                    x.PenaltyPaymentDate
                }).ToListDataResult(loadParam); 

             
        }
    }
}