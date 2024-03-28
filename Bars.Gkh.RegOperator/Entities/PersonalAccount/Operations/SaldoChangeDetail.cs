namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Детализация изменения сальдо в рамках одного ЛС
    /// </summary>
    public class SaldoChangeDetail : BaseImportableEntity
    {
        /// <summary>
        /// NHibernate
        /// </summary>
        public SaldoChangeDetail()
        {
            
        }

        /// <summary>
        /// .ctor
        /// </summary>
        public SaldoChangeDetail(ChargeOperationBase chargeOperation, BasePersonalAccount personalAccount)
        {
            this.ChargeOperation = chargeOperation;
            this.PersonalAccount = personalAccount;
        }

        /// <summary>
        /// Операция, в рамках которой происходили изменения
        /// </summary>
        public virtual ChargeOperationBase ChargeOperation { get; set; }

        /// <summary>
        /// Лицевой счёт
        /// </summary>
        public virtual BasePersonalAccount PersonalAccount { get; set; }

        /// <summary>
        /// Вид изменения
        /// </summary>
        public virtual WalletType ChangeType { get; set; }

        /// <summary>
        /// Старое значение
        /// </summary>
        public virtual decimal OldValue { get; set; }

        /// <summary>
        /// Новое значение
        /// </summary>
        public virtual decimal NewValue { get; set; }

        /// <summary>
        /// Дельта изменений
        /// </summary>
        public virtual decimal Amount => this.NewValue - this.OldValue;
    }
}