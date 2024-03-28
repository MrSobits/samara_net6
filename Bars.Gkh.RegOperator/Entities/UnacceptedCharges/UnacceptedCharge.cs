namespace Bars.Gkh.RegOperator.Entities
{
    using System.Collections.Generic;
    using B4.Modules.States;
    using B4.Utils.Annotations;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    using Gkh.Utils;
    using DomainModelServices;
    using Gkh.Enums.Decisions;
    using PersonalAccount;

    /// <summary>
    /// Неподтвержденное начисление
    /// </summary>
    public class UnacceptedCharge : BaseImportableEntity
    {
        #region private fields

        private List<PersonalAccountCalcParam_tmp> _params;
        private List<CalculationParameterTrace> traces;
        private List<RecalcHistory> recaclHistory;

        #endregion

        #region persisted properties

        /// <summary>
        /// Ссылка на пакет неподтвержденных начислений
        /// </summary>
        public virtual UnacceptedChargePacket Packet { get; set; }

        /// <summary>
        /// ЛС
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// GUID начисления
        /// </summary>
        public virtual string Guid { get; set; }

        /// <summary>
        /// Сумма начисления. Складывается из суммы по базовому тарифу, суммы переплаты, суммы по пени, суммы по перерасчету
        /// </summary>
        public virtual decimal Charge { get; set; }

        /// <summary>
        /// Сумма начисления по тарифу
        /// </summary>
        public virtual decimal ChargeTariff { get; set; }

        /// <summary>
        /// Сумма начисления, которая пришла сверх базового тарифа
        /// </summary>
        public virtual decimal TariffOverplus { get; set; }

        /// <summary>
        /// Сумма начисления по пени
        /// </summary>
        public virtual decimal Penalty { get; set; }

        /// <summary>
        /// Перерасчет пени
        /// </summary>
        public virtual decimal RecalcPenalty { get; set; }

        /// <summary>
        /// Сумма перерасчета по базовому тарифу
        /// </summary>
        public virtual decimal RecalcByBaseTariff { get; set; }

        /// <summary>
        /// Перерасчет по тарифу решения
        /// </summary>
        public virtual decimal RecalcByDecision { get; set; }

        /// <summary>
        /// Подтверждено
        /// </summary>
        public virtual bool Accepted { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Статус ЛС на момент расчета
        /// </summary>
        public virtual State AccountState { get; set; }

        /// <summary>
        /// Номер расчетного счета
        /// </summary>
        public virtual string ContragentAccountNumber { get; set; }

        /// <summary>
        /// Способ формирования фонда КР
        /// </summary>
        public virtual CrFundFormationDecisionType CrFundFormationDecisionType { get; set; }

        #endregion persisted properties

        #region non-persisted

        /// <summary>
        /// Трассировка параметров расчетов ЛС
        /// </summary>
        public virtual IEnumerable<CalculationParameterTrace> ParameterTraces { get { return this.traces; } } 
        public virtual IEnumerable<PersonalAccountCalcParam_tmp> CalculatedParams { get { return this._params; } } 
        /// <summary>
        /// Историй перерасчета
        /// </summary>
        public virtual IEnumerable<RecalcHistory> RecalcHistory { get { return this.recaclHistory; } } 

        #endregion

        /// <summary>
        /// Неподтвержденные начисления
        /// </summary>
        public UnacceptedCharge()
        {
            this.Guid = System.Guid.NewGuid().ToString();
            this._params = new List<PersonalAccountCalcParam_tmp>();
            this.traces = new List<CalculationParameterTrace>();
            this.recaclHistory = new List<RecalcHistory>();
        }

        /// <summary>
        /// Создание неподтвержденного начисления
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <param name="packet">Пакет начислений</param>
        public UnacceptedCharge(BasePersonalAccount account, UnacceptedChargePacket packet) : this()
        {
            ArgumentChecker.NotNull(account, nameof(account));
            ArgumentChecker.NotNull(packet, nameof(packet));

            this.PersonalAccount = account;
            this.Packet = packet;
            this.AccountState = account.State;

        }

        #region Methods

        /// <summary>
        /// Расчет текущего начисления
        /// </summary>
        /// <param name="period">Период, за который начисляем</param>
        /// <param name="factory">Фабрика для получения реализаций калькуляторов</param>
        public virtual void Calculate(IPeriod period, IChargeCalculationImplFactory factory)
        {
            ArgumentChecker.NotNull(period, nameof(period));
            ArgumentChecker.NotNull(factory, nameof(factory));

            var calc = factory.CreateCalculator();
            var result = calc.Calculate(period, this.PersonalAccount, this);

            this.ChargeTariff = result.TariffCharge.ByBaseTariff.RegopRoundDecimal(2);
            this.TariffOverplus = result.TariffCharge.Overplus.RegopRoundDecimal(2);

            this.RecalcByBaseTariff = result.TariffRecalc.ByBaseTariff.RegopRoundDecimal(2);
            this.RecalcByDecision = result.TariffRecalc.ByDecisionTariff.RegopRoundDecimal(2);

            this.Penalty = result.Penalty.Penalty.RegopRoundDecimal(2);
            this.RecalcPenalty = result.Penalty.Recalc.RegopRoundDecimal(2);

            this.Charge = this.ChargeTariff + this.TariffOverplus + this.Penalty + this.RecalcByBaseTariff + this.RecalcByDecision + this.RecalcPenalty;

            this.traces.AddRange(result.Traces);
            this.recaclHistory.AddRange(result.RecalcHistory);
            this._params.AddRange(result.Params);
        }
        #endregion
    }
}