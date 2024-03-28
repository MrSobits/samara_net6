namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Dto;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Bars.Gkh.RegOperator.Enums;

    using Decisions.Nso.Entities;
    using Entities.ValueObjects;
    using Entities;
    using Entities.Dict;
    using Gkh.Entities;
    using Gkh.Enums.Decisions;
    using Entities.PersonalAccount;
    using Gkh.Modules.ClaimWork.Entities;

    /// <summary>
    /// Глобальный параметров, необходимых при начислениях
    /// </summary>
    public interface ICalculationGlobalCache
    {
        /// <summary>
        /// Инициализировать лог
        /// </summary>
        /// <param name="period">Период начисления, в рамках которого используется кеш</param>
        /// <param name="accounts">Лицевые счета</param>
        void Initialize(IPeriod period, IQueryable<BasePersonalAccount> accounts);

        /// <summary>
        /// Метод указывает сборщику кэша, что мы будем считать минимум от указанной даты
        /// </summary>
        /// <remarks>Используется </remarks>
        /// <param name="account">Лицевой счет</param>
        /// <param name="date">Дата перерасчета</param>
        void SetManualRecalcDate(BasePersonalAccount account, DateTime date);

        /// <summary>
        /// Получить все закрытые периоды. Учитывать параметры инициализации кеша
        /// </summary>
        /// <returns>Список закрытых периодов</returns>
        IEnumerable<ChargePeriod> GetClosedPeriods();

        /// <summary>
        /// Получить список всех фиксированных начислений. Учитывать параметры инициализации кеша
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <returns>Список начислений по ЛС</returns>
        IEnumerable<PersonalAccountChargeDto> GetAllCharges(BasePersonalAccount account);

        /// <summary>
        /// Получить список всех трансферов. Учитывать параметры инициализации кеша
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <returns>Список оплат по ЛС</returns>
        IEnumerable<TransferDto> GetPaymentTransfers(BasePersonalAccount account);

        /// <summary>
        /// Получить список отмен
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <returns>Список оплат по ЛС</returns>
        IEnumerable<TransferDto> GetCancelPaymentTransfers(BasePersonalAccount account);

        /// <summary>
        /// Получить список отмен по базовому тарифу
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <returns>Список оплат по ЛС</returns>
        IEnumerable<TransferDto> GetBaseTariffCancelPaymentTransfers(BasePersonalAccount account);

        /// <summary>
        /// Получить список всех оплат по базовому тарифу. Учитывать параметры инициализации кеша
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <returns>Список оплат по ЛС</returns>
        IEnumerable<TransferDto> GetPaymentTransfersBaseTariff(BasePersonalAccount account);


        /// <summary>
        /// Получить список всех возвратов оплат по базаовому тарифу
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <returns>Список возвратов по ЛС</returns>
        IEnumerable<TransferDto> GetReturnTransfersBaseTariff(BasePersonalAccount account);

        /// <summary>
        /// Получить список всех возвратов оплат по тарифу решения
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <returns>Список возвратов по ЛС</returns>
        IEnumerable<TransferDto> GetReturnTransfersDecisionTariff(BasePersonalAccount account);

        /// <summary>
        /// Получить трансферы с кошелька
        /// по мировому соглашению
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        IEnumerable<Transfer> GetRaaWalletTransfers(BasePersonalAccount account);

        /// <summary>
        /// Получить Зачеты средств за выполненные ранее работы (старые)
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <returns>Зачеты средств за выполненные ранее работы</returns>
        IEnumerable<TransferDto> GetPerfWorks(BasePersonalAccount account);

        /// <summary>
        /// Получить список агрегаций по ЛС по периодам. Учитывать параметры инициализации кеша
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <returns>Список агрегаций по ЛС</returns>
        IEnumerable<PersonalAccountPeriodSummaryDto> GetAllSummaries(BasePersonalAccount account);

        /// <summary>
        /// Получить все параметры начислений пени
        /// </summary>
        /// <returns>Список параметров начисления пени</returns>
        IEnumerable<PaymentPenalties> GetPenaltyParams();

        /// <summary>
        /// Получить периоды действия способов формирования кр (из решений)
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        IEnumerable<Tuple<DateTime, CrFundFormationDecisionType>> GetRoFundFormationType(BasePersonalAccount account);

        /// <summary>
        /// Получить события для перерасчета по ЛС
        /// </summary>
        /// <param name="account">ЛС</param>
        List<PersonalAccountRecalcEvent> GetRecalcEvents(BasePersonalAccount account);

        /// <summary>
        /// Получить решения по тарифам для дома
        /// </summary>
        /// <param name="realty">Дом</param>
        IEnumerable<MonthlyFeeAmountDecision> GetFees(RealityObject realty);

        /// <summary>
        /// Получить параметр начисления пени по периоду
        /// и типу решения в доме
        /// </summary>
        /// <param name="period"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        PaymentPenalties GetPeriodPenalty(DateTime dateStart, DateTime dateEnd, BasePersonalAccount account);

        /// <summary>
        /// Получить расписание реструктуризации
        /// </summary>
        IEnumerable<RestructDebtSchedule> GetRestructureSchedule(BasePersonalAccount account);

        /// <summary>
        /// Получить детализацию реструктуризации
        /// </summary>
        IEnumerable<RestructDebtScheduleDetail> GetRestructureScheduleDetails(BasePersonalAccount account);

        /// <summary>
        /// Получить дату документа реструктуризации
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        DateTime? GetRestructDate(BasePersonalAccount account);

        /// <summary>
        /// Получить значения решений собственников по сроку уплаты взноса
        /// </summary>
        IEnumerable<OwnerPenaltyDelay> GetOwnerPenaltyDelays(BasePersonalAccount account, DateTime actualDate);

        /// <summary>
        /// Получить дату ручного перерасчета, если имеется
        /// </summary>
        DateTime? GetManuallyRecalcDate(BasePersonalAccount account);

        /// <summary>
        /// Вернуть историю перерасчетов
        /// </summary>
        IEnumerable<RecalcHistoryDto> GetRecalcHistory(BasePersonalAccount account);

        /// <summary>
        /// Получить запреты перерасчетов
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        IEnumerable<PersonalAccountBanRecalc> GetBanRecalc(BasePersonalAccount account);

        /// <summary>
        /// Вернуть зачеты средств (новые)
        /// </summary>
        /// <param name="account">ЛС</param>
        /// <returns></returns>
        IEnumerable<PerformedWorkCharge> GetPerfWorkCharge(BasePersonalAccount account);

        /// <summary>
        /// Вернуть протоколы расчёта
        /// </summary>
        /// <param name="guid">Guid начислений</param>
        /// <param name="first">Берём данные протокола на начало периода в случае, если передать true</param>
        /// <returns></returns>
        CalculationParameterTraceDto GetCalcParamTrace(string guid, bool first = true);

        /// <summary>
        /// Получить параметр начисления пени на дату
        /// и типу решения в доме
        /// </summary>
        /// <param name="date"></param>
        /// <param name="account"></param>
        /// <returns></returns>
        PaymentPenalties GetPeriodPenalty(DateTime date, BasePersonalAccount account);

        /// <summary>
        /// Получить параметры начисления пени на даты
        /// и типу решения в доме
        /// </summary>
        /// <param name="endDate"></param>
        /// <param name="account"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        PaymentPenalties GetPeriodPenalty(DateTime startDate, DateTime endDate, BasePersonalAccount account, out PaymentPenalties prevPenalty);

        /// <summary>
        /// Получить параметры начисления пени на даты
        /// и типу решения в доме
        /// </summary>
        /// <param name="endDate"></param>
        /// <param name="account"></param>
        /// <param name="startDate"></param>
        /// <returns></returns>
        IEnumerable<KeyValuePair<DateTime, decimal>> GetPeriodPenalties(DateTime startDate, DateTime endDate, BasePersonalAccount account);

        /// <summary>
        /// Получить параметры количества допустимой просрочки
        /// и типу решения в доме
        /// </summary>
        KeyValuePair<DateTime, int> GetPeriodDays(DateTime startDate, DateTime endDate, BasePersonalAccount account);

        /// <summary>
        /// Получить трансферы образованные от слияния ЛС (amount>0) в открытом периоде
        /// </summary>
        /// <param name="walletGuid">Гуид кошелька</param>
        /// <returns></returns>
        IEnumerable<TransferDto> GetTransferFromAccountsInCurrentPeriod(string walletGuid);

        /// <summary>
        /// Получить отмены начислений в открытом периоде
        /// </summary>
        /// <param name="personalAccountId">Код ЛС</param>
        /// <param name="сancelType">Тип отмены</param>
        /// <returns></returns>
        IEnumerable<CancelChargeDto> GetCancelChargesInCurrentPeriod(long personalAccountId, CancelType сancelType);

        /// <summary>
        /// Получить отмены начислений в открытом периоде
        /// </summary>
        /// <param name="personalAccountId">Код ЛС</param>
        /// <param name="сancelType">Тип отмены</param>
        /// <returns></returns>
        IEnumerable<CancelChargeDto> GetAllCancelChargesInCurrentPeriod(long personalAccountId, CancelType сancelType);

        /// <summary>
        /// Получить объект с кошельками на ЛС
        /// </summary>
        /// <param name="personalAccountId"></param>
        /// <returns></returns>
        WalletDto GetWalletByAccountId(long personalAccountId);

        /// <summary>
        /// Получить день начала и день окончания фиксированного пеирода
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        FixedPeriodCalcPenalties GetFixPeriodCalc(DateTime start);

        /// <summary>
        /// Получить тариф из протокола на дату
        /// </summary>
        /// <param name="decisions"></param>
        /// <param name="begin"></param>
        /// <returns></returns>
        DecisionTariffDto GetDecisionTarif(RealityObject realityObject, DateTime begin);

        /// <summary>
        /// Включен ли модуль ПИР
        /// </summary>
        bool IsClaimWorkEnabled { get; }

        /// <summary>
        /// Рассчитывать пени
        /// </summary>
        bool CalculatePenalty { get; }

        bool RecalcPenaltyByCurrentRefinancingRate { get; }

        /// <summary>
        /// Рассчитывать пени исходя из сальдо ЛС
        /// </summary>
        Dictionary<long, bool> CalcPenByAccId { get; }

        /// <summary>
        /// Упрощенный расчет пени
        /// </summary>
        bool SimpleCalculatePenalty { get; }

        /// <summary>
        /// Расчет пени для муниципалов раз в год
        /// </summary>
        bool CalcPenaltyOneTimeMunicipalProperty { get; }        

        /// <summary>
        /// Дата окончания расчётов пени по упрощённой схеме
        /// </summary>
        DateTime? DatePenaltyCalcTo { get; }

        /// <summary>
        /// Дата начала расчёта пени по упрощённой схеме
        /// </summary>
        DateTime? DatePenaltyCalcFrom { get; }

        /// <summary>
        /// Не пересчитывать пени за предыдущий период
        /// </summary>
        NumberDaysDelay NumberDaysDelay { get; }

        /// <summary>
        /// Начислять пени на доп. взносы
        /// </summary>
        bool CalculatePenaltyForDecisionTarif { get; }

        /// <summary>
        /// Дата вступления в силу
        /// </summary>
        DateTime? NewPenaltyCalcStart { get; set; }

        /// <summary>
        /// Допустимая просрочка
        /// </summary>
        int? NewPenaltyCalcDays { get; set; }

        /// <summary>
        /// Использовать фиксированный период расчета пени
        /// </summary>
        bool IsFixCalcPeriod { get; set; }

        /// <summary>
        /// Ставка рефинансирования (при отсутствии оплат)
        /// </summary>
        RefinancingRate RefinancingRate { get; set; }

        /// <summary>
        /// Допустимый срок просрочки оплаты для физ лиц
        /// </summary>
        int IndividualAllowDelayPaymentDays { get; set; }

        /// <summary>
        /// Допустимый срок просрочки оплаты для юр лиц
        /// </summary>
        int LegalAllowDelayPaymentDays { get; set; }

        /// <summary>
        /// Очистить
        /// </summary>
        void Clear();
    }

    /// <summary>
    /// Параметры пени
    /// </summary>
    /// <typeparam name="TValue">Тип параметра</typeparam>
    public class PenaltyParameterValue<TValue>
    {
        /// <summary>
        /// Дата начала действия параметра
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        /// Дата окончания действия параметра
        /// </summary>
        public DateTime? DateEnd { get; set; }

        /// <summary>
        /// Способ формирования фонда
        /// </summary>
        public CrFundFormationDecisionType FundFormationType { get; set; }

        /// <summary>
        /// Параметры, из которых сформировался текущий параметр
        /// </summary>
        public IList<PaymentPenalties> Source { get; set; }

        /// <summary>
        /// Значение параметра
        /// </summary>
        public TValue Value { get; set; }

        /// <summary>
        /// Добавить параметр и изменить дату окончания действия
        /// </summary>
        /// <param name="parameter">Параметр</param>
        public void SetEndDate(PaymentPenalties parameter)
        {
            this.Source.Add(parameter);
            this.DateEnd = parameter.DateEnd;
        }

        /// <summary>
        /// Вернуть в формате ключ-значение
        /// </summary>
        /// <returns>Экземпляр <see cref="KeyValuePair{TKey,TValue}"/></returns>
        public KeyValuePair<DateTime, TValue> ToKeyValuePair()
        {
            return new KeyValuePair<DateTime, TValue>(this.DateStart, this.Value);
        }
    }
}