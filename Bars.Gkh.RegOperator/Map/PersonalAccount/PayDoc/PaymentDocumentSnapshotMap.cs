namespace Bars.Gkh.RegOperator.Map.PersonalAccount.PayDoc
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;

    /// <summary>Маппинг для "Класс для хранения истории платежных документов"</summary>
    public class PaymentDocumentSnapshotMap : BaseEntityMap<PaymentDocumentSnapshot>
    {
        /// <summary>
        /// Маппинг для "Класс для хранения истории платежных документов"
        /// </summary>
        public PaymentDocumentSnapshotMap() :
                base("Класс для хранения истории платежных документов", "REGOP_PAYMENT_DOC_SNAPSHOT")
        {
        }

        /// <summary>
        /// Маппинг для "Класс для хранения истории платежных документов"
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.HolderId, "Идентификатор объекта, по которому формируется платежка Это может быть Id либо Ли" +
                    "цевого счета, либо Абонента").Column("HOLDER_ID");
            this.Property(x => x.HolderType, "Тип объекта, по которому формируется платежка (ЛС, абонент)").Column("HOLDER_TYPE").Length(250);
            this.Property(x => x.PaymentDocumentType, "Тип документа на оплату").Column("DOC_TYPE").NotNull().DefaultValue(0);
            this.Reference(x => x.Period, "Период начисления").Column("PERIOD_ID").NotNull().Fetch();
            this.Property(x => x.Data, "Основные данные документа").Column("RAW_DATA").Length(250);
            this.Property(x => x.DocNumber, "Номер документа (для юр, для физ пустое)").Column("DOC_NUM").Length(250);
            this.Property(x => x.DocDate, "Дата документа (для юр, для физ пустое)").Column("DOC_DATE");
            this.Property(x => x.Payer, "Плательщик (имя || наименование контрагента)").Column("PAYER").Length(250);
            this.Property(x => x.Municipality, "Муниципальное образование").Column("MUNICIPALITY").Length(500);
            this.Property(x => x.Settlement, "Муниципальный район").Column("SETTLEMENT").Length(250);
            this.Property(x => x.Address, "Адрес абонента").Column("OWNER_ADDRESS").Length(250);
            this.Property(x => x.OwnerType, "Тип абонента").Column("OWNER_TYPE");
            this.Property(x => x.PaymentReceiverAccount, "Р/С получателя платежа").Column("RECEIVER_ACCOUNT").Length(250);
            this.Property(x => x.DeliveryAgent, "Агент доставки").Column("DELIVERY_AGENT").Length(250);
            this.Property(x => x.TotalCharge, "Всего к оплате").Column("TOTAL_PAYMENT");
            this.Property(x => x.PaymentState, "Статус оплаты").Column("PAYMENT_STATE").DefaultValue(0);
            this.Property(x => x.IsBase, "Базовый слепок").Column("IS_BASE").DefaultValue(true);
            this.Property(x => x.SendingEmailState, "Статус отправки на почту").Column("SENDING_EMAIL_STATE").DefaultValue(0);
            this.Property(x => x.AccountCount, "Количество ЛС").Column("ACCOUNT_COUNT").DefaultValue(0);
            this.Property(x => x.HasEmail, "Наличие эл. почты").Column("HAS_EMAIL").DefaultValue(20);
            this.Property(x => x.OwnerInn, "Инн плательщика").Column("OWNER_INN").DefaultValue(20);
        }
    }
}