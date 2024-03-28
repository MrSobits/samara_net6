namespace Bars.GisIntegration.Base.Map.Bills
{
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Map;

    /// <summary>
    /// Маппинг сущности Bars.GisIntegration.RegOp.Entities.Payment.RisAcknowledgment
    /// </summary>
    public class RisAcknowledgmentMap : BaseRisEntityMap<RisAcknowledgment>
    {
        /// <summary>
        /// Конструктор маппинга
        /// </summary>
        public RisAcknowledgmentMap()
            : base("Bars.Gkh.Ris.Entities.Payment.RisAcknowledgment", "RIS_ACKNOWLEDGMENT")
        {
        }

        /// <summary>
        /// Инициализация маппинга
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.OrderId, "Уникальный идентификатор распоряжения").Column("ORDERID").Length(32).NotNull();
            this.Property(x => x.PaymentDocumentNumber, "Идентификатор платежного документа").Column("PAYMENTDOCUMENTNUMBER").Length(18).NotNull();
            this.Property(x => x.HSType, "Вид коммунальной услуги").Column("HSTYPE").Length(40);
            this.Property(x => x.MSType, "Вид ЖУ").Column("MSTYPE").Length(40);
            this.Property(x => x.ASType, "Вид дополнительной услуги").Column("ASTYPE").Length(40);
            this.Property(x => x.Amount, "Сумма квитирования в копейках").Column("AMOUNT");
            this.Reference(x => x.Notification, "Уведомление о выполнении распоряжения").Column("NOTIFY_ID").Fetch();
            this.Reference(x => x.PaymentDocument, "Платежный документ").Column("PAY_DOC_ID").Fetch();
        }
    }
}
