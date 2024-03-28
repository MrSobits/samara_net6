namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Маапинг сущности <see cref="SberbankPaymentDoc"/>
    /// </summary>
    public class SberbankPaymentDocMap : BaseEntityMap<SberbankPaymentDoc>
    {
        /// <summary>
        /// Конструктор
        /// </summary>
        public SberbankPaymentDocMap() : base("Квитанция на оплату Сбербанку", "SBERBANK_PAYMENT_DOC") { }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.Period, "Период").Column("PERIOD").NotNull();
            this.Reference(x => x.Account, "Аккаунт").Column("ACCOUNT").NotNull();
            this.Property(x => x.LastDate, "Дата последнего запроса").Column("LAST_DATE").NotNull();
            this.Property(x => x.Count, "Количество запросов").Column("COUNT").NotNull();
            this.Property(x => x.GUID, "GUID пользователя").Column("GUID").NotNull();
            this.Reference(x => x.PaymentDocFile, "Файл").Column("FILE");
        }
    }
}