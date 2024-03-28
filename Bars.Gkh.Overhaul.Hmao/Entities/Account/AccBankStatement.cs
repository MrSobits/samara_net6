namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using System;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Банковская выписка
    /// </summary>
    public class AccBankStatement : BaseImportableEntity, IStatefulEntity
    {
        /// <summary>
        ///  Счет
        /// </summary>
        public virtual BankAccount BankAccount { get; set; }

        /// <summary>
        /// Номер счета
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Дата выписки
        /// </summary>
        public virtual DateTime DocumentDate { get; set; }

        /// <summary>
        /// Исходящий остаток
        /// </summary>
        public virtual decimal? BalanceOut { get; set; }

        /// <summary>
        /// Входящий остаток
        /// </summary>
        public virtual decimal? BalanceIncome { get; set; }

        /// <summary>
        /// Дата последней операции по счету
        /// </summary>
        public virtual DateTime? LastOperationDate { get; set; }

        public virtual State State { get; set; }
    }
}
