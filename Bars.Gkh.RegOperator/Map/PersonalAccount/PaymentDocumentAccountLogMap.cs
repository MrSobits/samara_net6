namespace Bars.Gkh.RegOperator.Map.PersonalAccount
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;

    /// <summary>
    /// Маппинг для "Лог о распечатке конкретного лс в рамках пачки"
    /// </summary>
    public class PaymentDocumentAccountLogMap : BaseEntityMap<PaymentDocumentAccountLog>
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        public PaymentDocumentAccountLogMap() 
            :base("Лог о распечатке конкретного лс в рамках пачки", "REGOP_PAYMENT_DOC_ACC_LOG")
        {
        }

        /// <summary>
        /// Мапинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Log, "Запись лога").Column("LOG_ID").NotNull().Fetch();
            this.Property(x => x.AccountId, "Id лицевого счета").Column("ACCOUNT_ID").NotNull();
        }
    }
}
