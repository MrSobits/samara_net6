namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Детализация по разделению лицевого счета
    /// </summary>
    public class SplitAccountDetail : BaseImportableEntity
    {
        /// <summary>
        /// Операция, в рамках которой производился перенос долга
        /// </summary>
        public virtual ChargeOperationBase Operation { get; set; }

        /// <summary>
        /// Лицевой счет зачисления
        /// </summary>
        public virtual BasePersonalAccount Account { get; set; }

        /// <summary>
        /// Тип кошелька
        /// </summary>
        public virtual WalletType WalletType { get; set; }

        /// <summary>
        /// Сумма переноса
        /// </summary>
        public virtual decimal Amount { get; set; }
    }
}