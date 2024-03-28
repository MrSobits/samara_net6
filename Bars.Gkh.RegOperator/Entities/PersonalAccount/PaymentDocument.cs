namespace Bars.Gkh.RegOperator.Entities.PersonalAccount
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// История созданных документов на оплату. Сущность нужна только для нумерации документов
    /// </summary>
    public class PaymentDocument : BaseEntity
    {
        /// <summary>
        /// Поле синхронизации
        /// </summary>
        public static object SyncObject = new object();

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual int Number { get; set; }

        // Ниже намеренно хранятся только идентификаторы сущностей

        /// <summary>
        /// идентификатор лицевого счета
        /// </summary>
        public virtual long AccountId { get; set; }

        /// <summary>
        /// идентификатор периода
        /// </summary>
        public virtual long PeriodId { get; set; }
    }
}