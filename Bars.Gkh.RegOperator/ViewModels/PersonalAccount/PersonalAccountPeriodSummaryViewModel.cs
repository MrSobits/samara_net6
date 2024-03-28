namespace Bars.Gkh.RegOperator.ViewModels.PersonalAccount
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    /// <summary>
    /// Вью-модель для сущности Ситуация на л/с на период
    /// </summary>
    public class PersonalAccountPeriodSummaryViewModel : BaseViewModel<PersonalAccountPeriodSummary>
    {
        /// <summary>
        /// Домен-сервис для Начисления на счету дома
        /// </summary>
        public IDomainService<RealityObjectChargeAccountOperation> RoChargeOperDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Поступление оплат аренды
        /// </summary>
        public IDomainService<RentPaymentIn> RentPaymentDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Ранее накопленные средства
        /// </summary>
        public IDomainService<AccumulatedFunds> AccumFundsDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Изменение исходящего сальдо счета
        /// </summary>
        public IDomainService<PeriodSummaryBalanceChange> SaldoOutChangeDomain { get; set; }

        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен-сервис</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат запроса</returns>
        public override IDataResult List(IDomainService<PersonalAccountPeriodSummary> domainService, BaseParams baseParams)
        {
            var operationId = baseParams.Params.GetAsId("operationId");

            var loadParam = baseParams.GetLoadParam();

            if (operationId == 0)
            {
                operationId = loadParam.Filter.GetAsId("operationId");
            }

            var operation = this.RoChargeOperDomain.Get(operationId);

            if (operation == null)
            {
                return new ListDataResult();
            }

            var data =
                domainService.GetAll()
                    .Where(x => x.PersonalAccount.Room.RealityObject.Id == operation.Account.RealityObject.Id)
                    .Where(x => x.Period.Id == operation.Period.Id)
                    .Select(
                        x => new
                        {
                            x.PersonalAccount.PersonalAccountNum,
                            Period = x.Period.StartDate,
                            periodIsOpen = !x.Period.IsClosed,
                            x.SaldoIn,
                            x.SaldoOut,
                            ChargeTotal = 
                                x.ChargeTariff + x.RecalcByBaseTariff 
                                + x.Penalty 
                                + x.BaseTariffChange + x.DecisionTariffChange + x.PenaltyChange, // TODO fix recalc
                            TotalPayment = x.TariffPayment + x.TariffDecisionPayment + x.PenaltyPayment,
                            Penalty = x.Penalty + x.RecalcByPenalty + x.PenaltyChange,
                            x.PenaltyPayment,
                            Recalc = x.RecalcByBaseTariff + x.RecalcByDecisionTariff, // TODO fix recalc
                            PeriodId = x.Period.Id,
                            x.PersonalAccount.Room.RoomNum,
                            AccountId = x.PersonalAccount.Id,
                            ChargedByBaseTariff = x.ChargedByBaseTariff + x.RecalcByBaseTariff + x.BaseTariffChange,
                            x.TariffPayment,
                            ChargeByDecision = (x.ChargeTariff - x.ChargedByBaseTariff) + x.RecalcByDecisionTariff + x.DecisionTariffChange,
                            x.TariffDecisionPayment
                        })
                    .Select(
                        x => new
                        {
                            x.PersonalAccountNum,
                            x.AccountId,
                            x.PeriodId,
                            x.Period,
                            x.SaldoIn,
                            x.ChargeTotal,
                            x.TotalPayment,
                            Dept = x.ChargeTotal - x.TotalPayment,
                            x.Penalty,
                            x.PenaltyPayment,
                            x.Recalc,
                            x.SaldoOut,
                            x.RoomNum,
                            x.ChargedByBaseTariff,
                            x.ChargeByDecision,
                            x.TariffDecisionPayment,
                            x.TariffPayment
                        })
                    .Filter(loadParam, this.Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), data.Count());
        }
    }
}