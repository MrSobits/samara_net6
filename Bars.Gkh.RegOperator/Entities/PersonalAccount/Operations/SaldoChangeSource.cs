namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Источник массового изменения сальдо в связи с установкой/изменения
    /// </summary>
    public class SaldoChangeSource : ChargeOperationBase
    {
        private readonly TypeChargeSource[] accessibleTypes = 
        {
            TypeChargeSource.BalanceChange,
            TypeChargeSource.SaldoChangeMass,
            TypeChargeSource.ImportSaldoChangeMass
        };

        private IList<SaldoChangeDetail> changeDetails;
        private MoneyOperation moneyOperation;

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="typeChargeSource">Тип изменения сальдо</param>
        /// <param name="period">Период</param>
        public SaldoChangeSource(TypeChargeSource typeChargeSource, ChargePeriod period) : base(period)
        {
            if (!this.accessibleTypes.Contains(typeChargeSource))
            {
                throw new ArgumentException("Неверный тип операции", nameof(typeChargeSource));
            }

            this.ChargeSource = typeChargeSource;
            this.OperationDate = this.FactOperationDate;
            this.changeDetails = new List<SaldoChangeDetail>();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        protected SaldoChangeSource()
        {
        }

        /// <summary>
        /// Импорт, который является инициатором изменения
        /// </summary>
        public virtual SaldoChangeExport SaldoChangeExport { get; set; }

        /// <summary>
        /// Детализация изменений
        /// </summary>
        public virtual IEnumerable<SaldoChangeDetail> ChangeDetails => this.changeDetails;

        /// <summary>
        /// Операция движения денег, не хранимое, используется при проведении операций
        /// </summary>
        public virtual MoneyOperation Operation => this.moneyOperation ?? this.CreateOperation(this.Period);

        /// <summary>
        /// Установить сальдо
        /// </summary>
        /// <param name="data">Изменения по сальдо</param>
        /// <returns>Список трансферов</returns>
        public virtual IList<Transfer> ApplyChange(ISaldoChangeData data)
        {
            var result = this.ApplyInternal(data);
            return data.PersonalAccount.ChangeSaldoMass(this, result.Item1, result.Item2, result.Item3);
        }

        /// <summary>
        /// Добавить изменение сальдо
        /// </summary>
        /// <param name="account">Лицевой счет</param>
        /// <param name="saldoChangeBase">Изменение сальдо по базовому тарифу</param>
        /// <param name="saldoChangeDecision">Изменение сальдо по тарифу решения</param>
        /// <param name="saldoChangePenalty">Изменение сальдо по пени</param>
        public virtual void AddImportChange(BasePersonalAccount account, decimal saldoChangeBase, decimal saldoChangeDecision, decimal saldoChangePenalty)
        {
            // Так как мы грузим сразу изменение сальдо, а не изменённое сальдо, 
            // грузим как Amount = NewValue - 0 
            // ¯\_(ツ)_/¯
            if (saldoChangeBase != 0)
            {
                new SaldoChangeDetail(this, account)
                {
                    ChangeType = WalletType.BaseTariffWallet,
                    OldValue = 0,
                    NewValue = saldoChangeBase
                }.AddTo(this.changeDetails);
            }

            if (saldoChangeDecision != 0)
            {
                new SaldoChangeDetail(this, account)
                {
                    ChangeType = WalletType.DecisionTariffWallet,
                    OldValue = 0,
                    NewValue = saldoChangeDecision
                }.AddTo(this.changeDetails);
            }

            if (saldoChangePenalty != 0)
            {
                new SaldoChangeDetail(this, account)
                {
                    ChangeType = WalletType.PenaltyWallet,
                    OldValue = 0,
                    NewValue = saldoChangePenalty
                }.AddTo(this.changeDetails);
            }
        }

        /// <summary>
        /// Создать операцию движения денег
        /// </summary>
        /// <returns>Операция</returns>
        public override MoneyOperation CreateOperation(ChargePeriod period)
        {
            // Если операция применялась, то новую операцию не даём создавать
            if (this.Id > 0 || this.moneyOperation != null)
            {
                return null;
            }

            this.moneyOperation = base.CreateOperation(period);
            this.moneyOperation.Reason = this.ChargeSource.GetDisplayName();

            return this.moneyOperation;
        }

        private Tuple<decimal, decimal, decimal> ApplyInternal(
            ISaldoChangeData data)
        {
            SaldoChangeDetail saldoByBase = null;
            if (data.SaldoByBaseTariff != data.NewSaldoByBaseTariff)
            {
                saldoByBase = new SaldoChangeDetail(this, data.PersonalAccount)
                {
                    ChangeType = WalletType.BaseTariffWallet,
                    OldValue = data.SaldoByBaseTariff,
                    NewValue = data.NewSaldoByBaseTariff
                }.AddTo(this.changeDetails);
            }

            SaldoChangeDetail saldoByDecision = null;
            if (data.SaldoByDecisionTariff != data.NewSaldoByDecisionTariff)
            {
                saldoByDecision = new SaldoChangeDetail(this, data.PersonalAccount)
                {
                    ChangeType = WalletType.DecisionTariffWallet,
                    OldValue = data.SaldoByDecisionTariff,
                    NewValue = data.NewSaldoByDecisionTariff
                }.AddTo(this.changeDetails);
            }

            SaldoChangeDetail saldoByPenalty = null;
            if (data.SaldoByPenalty != data.NewSaldoByPenalty)
            {
                saldoByPenalty = new SaldoChangeDetail(this, data.PersonalAccount)
                {
                    ChangeType = WalletType.PenaltyWallet,
                    OldValue = data.SaldoByPenalty,
                    NewValue = data.NewSaldoByPenalty
                }.AddTo(this.changeDetails);
            }


            return new Tuple<decimal, decimal, decimal>(
                saldoByBase?.Amount ?? 0,
                saldoByDecision?.Amount ?? 0,
                saldoByPenalty?.Amount ?? 0);
        }
    }
}