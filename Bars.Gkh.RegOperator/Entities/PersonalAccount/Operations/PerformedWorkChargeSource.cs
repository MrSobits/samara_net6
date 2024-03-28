namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Базовая операция по зачету средств
    /// </summary>
    public class PerformedWorkChargeSource : ChargeOperationBase
    {
        protected MoneyOperation MoneyOperation;

        private readonly IList<PerformedWorkCharge> performedWorkCharges;

		/// <summary>
		/// .ctor
		/// </summary>
		/// <param name="operationDate">Период, с которого распределяем</param>
		/// <param name="account">ЛС</param>
		/// <param name="sum">Сумма</param>
		/// <param name="period">Период</param>
		public PerformedWorkChargeSource(DateTime operationDate, BasePersonalAccount account, decimal sum, ChargePeriod period) : base(period)
        {
            this.ChargeSource = TypeChargeSource.PerformedWorkCharge;
            this.performedWorkCharges = new List<PerformedWorkCharge>();
            this.PersonalAccount = account;
            this.Sum = sum;
            this.OperationDate = operationDate;
        }

        /// <summary>
        /// .ctor NH
        /// </summary>
        protected PerformedWorkChargeSource()
        {
        }

        /// <summary>
        /// ЛС
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Общая сумма
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Вся сумма распределена
        /// </summary>
        public virtual bool Distributed { get; set; }

        /// <summary>
        /// Распределеить на базовый тариф
        /// </summary>
        public virtual bool DistributeForBaseTariff { get; set; }

        /// <summary>
        /// Распределить на тариф решения
        /// </summary>
        public virtual bool DistributeForDecisionTariff { get; set; }

        /// <summary>
        /// Детализация зачета по периодам
        /// </summary>
        public virtual IEnumerable<PerformedWorkCharge> PerformedWorkCharges => this.performedWorkCharges;

        /// <summary>
        /// Вернуть кошельки, для которых возможно распределение
        /// </summary>
        /// <returns>Типы</returns>
        public virtual IEnumerable<WalletType> GetDistributableWalletTypes()
        {
            var listResult = new List<WalletType>();

            if (this.DistributeForBaseTariff)
            {
                listResult.Add(WalletType.BaseTariffWallet);
            }

            if (this.DistributeForDecisionTariff)
            {
                listResult.Add(WalletType.DecisionTariffWallet);
            }

            return listResult;
        }

		/// <summary>
		/// Применить начисление
		/// </summary>
		/// <param name="period">Период</param>
		/// <param name="distributuionType">Тип распределения</param>
		/// <param name="sum">Сумма</param>
		/// <param name="active">Флаг активности зачёта средств</param>
		/// <returns>Трансфер</returns>
		public virtual Transfer AddCharge(ChargePeriod period, WalletType distributuionType, decimal sum, bool active = true)
        {
            var charge = new PerformedWorkCharge(this, period, sum, active);

            this.performedWorkCharges.Add(charge);

            return this.ApplyCharge(charge, active);
        }

        /// <summary>
        /// Применить начисление
        /// </summary>
        /// <param name="charge">Операция по зачету среств</param>
        /// <param name="active">Флаг активности зачёта средств</param>
        /// <param name="applyPeriodSummary">Распределить в периоде</param>
        /// <returns>Трансфер</returns>
        public virtual Transfer ApplyCharge(PerformedWorkCharge charge, bool active = true, bool applyPeriodSummary = true)
        {
            if (charge.ChargeOperation != this)
            {
                throw new ArgumentException("Операция принадлежит другому источнику зачета средств");
            }

            charge.Active = active;
            return this.PersonalAccount.ApplyPerfWorkDeferredDistribution(
                this, 
                charge.Sum, 
                charge.DistributeType, 
                this.MoneyOperation,
                PerformedWorkDistributionParams.Create(charge, applyPeriodSummary));
        }

        /// <summary>
        /// Вернуть баланс, доступный для распределения
        /// </summary>
        /// <param name="distributed">Учитывать только распределенные</param>
        /// <returns>Сумма</returns>
        public virtual decimal GetBalance(bool distributed = false)
        {
            return this.Distributed ? 0m : this.Sum - this.PerformedWorkCharges.WhereIf(distributed, x => x.Active).SafeSum(x => x.Sum);
        }

        /// <summary>
        /// Создать операцию по передвижению денег
        /// </summary>
        /// <param name="period">
        /// Период
        /// </param>
        /// <returns>
        /// <see cref="ValueObjects.MoneyOperation"/>
        /// </returns>
        // ReSharper disable once OptionalParameterHierarchyMismatch
        public override MoneyOperation CreateOperation(ChargePeriod period)
        {
            var operation = base.CreateOperation(period);
            operation.Reason = "Зачет средств за выполненные работы";


            this.MoneyOperation = operation;
            return operation;
        }

        /// <summary>
        /// Очистить неактивные зачеты средств
        /// </summary>
        public virtual void ClearUnactive()
        {
            foreach (var performedWorkCharge in this.performedWorkCharges.Where(x => !x.Active).ToList())
            {
                this.performedWorkCharges.Remove(performedWorkCharge);
            }
        }
    }

    /// <summary>
    /// Параметры распределения зачета средств на ЛС
    /// </summary>
    public class PerformedWorkDistributionParams
    {
        /// <summary>
        /// Создать трансфер
        /// </summary>
        public bool CreateTransfer {get; set; }

        /// <summary>
        /// Распределить в периоде
        /// </summary>
        public bool ApplyPeriodSummary { get; set; }

        internal static PerformedWorkDistributionParams Create(PerformedWorkCharge charge, bool applySummary)
        {
            return new PerformedWorkDistributionParams
            {
                CreateTransfer = charge.Active,
                ApplyPeriodSummary = applySummary
            };
        }
    }
}
