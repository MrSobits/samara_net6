namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using Bars.B4.DataAccess; 

    /// <summary>
    /// Лог о распечатке конкретного лс в рамках пачки
    /// </summary>
    public class PaymentDocumentAccountLog : BaseEntity
    {
        /// <summary>
        /// Запись лога
        /// </summary>
        public virtual PaymentDocumentLog Log { get; set; }

        /// <summary>
        /// Лс
        /// </summary>
        public virtual long AccountId { get; set; }
    }
}
