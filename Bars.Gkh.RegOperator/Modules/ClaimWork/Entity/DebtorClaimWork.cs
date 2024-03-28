namespace Bars.Gkh.RegOperator.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums.ClaimWork;
    using Bars.Gkh.Modules.ClaimWork.Enums;
    using Bars.Gkh.RegOperator.Entities.Owner;

    using Gkh.Modules.ClaimWork.Entities;

    using Newtonsoft.Json;
    using Gkh.Entities;

    /// <summary>
    /// Основание претензионно исковой работы для неплательщиков
    /// </summary>
    public class DebtorClaimWork : BaseClaimWork
    {
        /// <summary>
        /// Абонент
        /// </summary>
        public virtual PersonalAccountOwner AccountOwner { get; set; }

        /// <summary>
        /// Тип должника
        /// </summary>
        public virtual DebtorType DebtorType { get; set; }

        /// <summary>
        /// Сумма текущей задолженности по базовому тарифу
        /// </summary>
        public virtual decimal CurrChargeBaseTariffDebt { get; set; }

        /// <summary>
        /// Сумма текущей задолженности по тарифу решения
        /// </summary>
        public virtual decimal CurrChargeDecisionTariffDebt { get; set; }

        /// <summary>
        /// Сумма текущей задолженности
        /// </summary>
        public virtual decimal CurrChargeDebt { get; set; }

        /// <summary>
        /// Сумма текущей задолженности по пени
        /// </summary>
        public virtual decimal CurrPenaltyDebt { get; set; }

        /// <summary>
        /// Сумма исходной задолженности по базовому тарифу
        /// </summary>
        public virtual decimal OrigChargeBaseTariffDebt { get; set; }

        /// <summary>
        /// Сумма исходной задолженности по тарифу решения
        /// </summary>
        public virtual decimal OrigChargeDecisionTariffDebt { get; set; }

        /// <summary>
        /// Сумма исходной задолженности
        /// </summary>
        public virtual decimal OrigChargeDebt { get; set; }

        /// <summary>
        /// Сумма исходной задолженности по пени
        /// </summary>
        public virtual decimal OrigPenaltyDebt { get; set; }

        /// <summary>
        /// Пользователь, начавший претензионную работу
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Статус ПИР по неплательщику
        /// </summary>
        public virtual DebtorState DebtorState { get; set; }

        /// <summary>
        /// Статус ПИР по неплательщику
        /// </summary>
        public virtual DebtorState DebtorStateHistory { get; set; }

        //Для безумного Воронежа
        /// <summary>
        /// ID долевого собственника из LawsuitIndiviualOwnerInfo
        /// </summary>
        //public virtual LawsuitIndividualOwnerInfo IdPartial { get; set; }

        /// <summary>
        /// Дата создания дела ПИР
        /// </summary>
        public virtual DateTime? PIRCreateDate { get; set; }

        /// <summary>
        /// Подрядчик
        /// </summary>
        public virtual Contragent SubContragent { get; set; }

        /// <summary>
        /// Договор с подрядчиком
        /// </summary>
        public virtual string SubContractNum { get; set; }

        /// <summary>
        /// Дата договора с подрядчиком
        /// </summary>
        public virtual DateTime? SubContractDate { get; set; }



        private IList<ClaimWorkAccountDetail> accountDetails;

        [JsonIgnore]
        [IgnoreDataMember]
        public virtual IEnumerable<ClaimWorkAccountDetail> AccountDetails => this.accountDetails
            ?? (this.accountDetails = new List<ClaimWorkAccountDetail>());

        public DebtorClaimWork()
        {
            this.ClaimWorkTypeBase = ClaimWorkTypeBase.Debtor;
            this.accountDetails = new List<ClaimWorkAccountDetail>();
        }

        public virtual void SetDebtorState(DebtorState newDebtorState, State newState)
        {
            this.DebtorState = newDebtorState;
            this.IsDebtPaid = newDebtorState == DebtorState.PaidDebt;
            if (newDebtorState != DebtorState.PaidDebt)
            {
                this.DebtPaidDate = null;
            }

            this.State = newState;
        }

        public virtual void AddAccountDetail(ClaimWorkAccountDetail detail)
        {
            if (this.accountDetails == null)
            {
                this.accountDetails = new List<ClaimWorkAccountDetail>();
            }
            this.accountDetails.Add(detail);
        }
    }
}