namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;
    using B4;
    using Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// ViewModel для Собственник в исковом заявлении
    /// </summary>
    public class LawsuitReferenceCalculationViewModel : BaseViewModel<LawsuitReferenceCalculation>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат получения списка
        /// </returns>
        public override IDataResult List(IDomainService<LawsuitReferenceCalculation> domainService, BaseParams baseParams)
        {
            var lawsuitId = baseParams.Params.GetAs("Lawsuit", 0L);

            var loadParam = this.GetLoadParam(baseParams);
            return domainService.GetAll()
                .Where(x => x.Lawsuit.Id == lawsuitId)
                .Select(x => new
                {
                    x.Id,
                    x.AccountNumber,
                    x.PeriodId.Name,
                    x.PeriodId.StartDate,
                    x.PeriodId.EndDate,
                    x.AreaShare,
                    x.BaseTariff,
                    x.RoomArea,
                    x.TarifDebtPay,
                    x.PaymentDate,
                    x.TarifDebt,
                    x.TariffCharged,
                    x.TarifPayment,
                    x.Description
                }).ToListDataResult(loadParam);
        }
    }
}