namespace Bars.Gkh.RegOperator.Modules.ClaimWork.Entity
{
    using System;
    using B4.DataAccess;

    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Modules.ClaimWork.Enums;

    /// <summary>
    /// View Реестр неплательщиков физ. лиц
    /// </summary>
    public class ViewIndividualClaimWork : PersistentObject
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual DebtorState DebtorState { get; set; }

        /// <summary>
        /// Муниципальный район
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Наименование абонента
        /// </summary>
        public virtual string AccountOwnerName { get; set; }

        /// <summary>
        /// Адрес прописки
        /// </summary>
        public virtual string RegistrationAddress { get; set; }

        /// <summary>
        /// Адреса ЛС
        /// </summary>
        public virtual string AccountsAddress { get; set; }

        /// <summary>
        /// Количество ЛС
        /// </summary>
        public virtual int AccountsNumber { get; set; }

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
        /// Задолженность погашена
        /// </summary>
        public virtual bool IsDebtPaid { get; set; }

        /// <summary>
        /// Дата погашения задолженности
        /// </summary>
        public virtual DateTime? DebtPaidDate { get; set; }

        /// <summary>
        /// Дата создания ПИР
        /// </summary>
        public virtual DateTime? PIRCreateDate { get; set; }

        /// <summary>
        /// Дата создания первого документа
        /// </summary>
        public virtual DateTime? FirstDocCreateDate { get; set; }

        /// <summary>
        /// Начисления по 185ФЗ
        /// </summary>
        public virtual bool HasCharges185FZ { get; set; }

        /// <summary>
        /// Минимальная доля площади на имеющихся ЛС
        /// </summary>
        public virtual decimal MinShare { get; set; }

        /// <summary>
        /// Флаг наличия несовершеннолетних
        /// </summary>
        public virtual bool Underage { get; set; }

        /// <summary>
        /// Id пользователя
        /// </summary>
        public virtual User User { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        public virtual DateTime ObjectCreateDate { get; set; }
    }
}