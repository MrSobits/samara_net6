namespace Bars.GisIntegration.Base.Map.Payment
{
    using Bars.GisIntegration.Base.Entities.Payment;
    using Bars.GisIntegration.Base.Map;

    public class NotificationOfOrderExecutionMap : BaseRisEntityMap<NotificationOfOrderExecution>
    {
        public NotificationOfOrderExecutionMap() :
            base("Bars.GisIntegration.RegOp.Entities.Payment.NotificationOfOrderExecution", "RIS_NOTIFORDEREXECUT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.SupplierId, "Уникальный идентификатор плательщика").Column("SUPPLIER_ID").Length(25);
            this.Property(x => x.SupplierName, "Наименование плательщика").Column("SUPPLIER_NAME").Length(160);
            this.Property(x => x.RecipientInn, "ИНН получателя платежа").Column("RECIPIENT_INN").Length(12);
            this.Property(x => x.RecipientKpp, "КПП получателя платежа").Column("RECIPIENT_KPP").Length(9);
            this.Property(x => x.BankName, "Наименование банка получателя платежа").Column("BANK_NAME").Length(160);
            this.Property(x => x.RecipientBik, "БИК банка получателя платежа").Column("RECIPIENT_BANK_BIK").Length(9);
            this.Property(x => x.CorrespondentBankAccount, "Корр. счет банка получателя").Column("RECIPIENT_BANK_CORRACC").Length(20);
            this.Property(x => x.RecipientAccount, "Счет получателя").Column("RECIPIENT_ACCOUNT").Length(20);
            this.Property(x => x.RecipientName, "Наименование получателя").Column("RECIPIENT_NAME").Length(160);
            this.Property(x => x.OrderId, "Уникальный идентификатор распоряжения").Column("ORDER_ID").Length(32);
            this.Reference(x => x.RisPaymentDocument, "Платежный документ").Column("RIS_PAYM_DOC_ID").Fetch().NotNull();
            this.Property(x => x.AccountNumber, "Номер лицевого счета/Иной идентификтатор плательщика").Column("ACCOUNT_NUMBER").Length(30);
            this.Property(x => x.OrderNum, "Номер распоряжения").Column("ORDER_NUM").Length(9);
            this.Property(x => x.OrderDate, "Дата распоряжения").Column("ORDER_DATE");
            this.Property(x => x.Amount, "Сумма").Column("AMOUNT");
            this.Property(x => x.PaymentPurpose, "Назначение платежа").Column("PAYMENT_PURPOSE").Length(210);
            this.Property(x => x.Comment, "Произвольный комментарий").Column("COMMENT").Length(210);
            this.Property(x => x.Inn, "Сведения об исполнителе - ИНН").Column("INN");
            this.Property(x => x.RecipientEntprSurname, "Исполнитель-ИП - Фамилия").Column("RECIPIENT_ENTPR_SURNAME");
            this.Property(x => x.RecipientEntprFirstName, "Исполнитель-ИП - Имя").Column("RECIPIENT_ENTPR_FIRSTNAME");
            this.Property(x => x.RecipientEntprPatronymic, "Исполнитель-ИП - Отчество").Column("RECIPIENT_ENTPR_PATRONYMIC");
            this.Property(x => x.RecipientLegalKpp, "Исполнитель-ЮЛ - КПП").Column("RECIPIENT_LEGAL_KPP");
            this.Property(x => x.RecipientLegalName, "Исполнитель-ЮЛ - Наименование").Column("RECIPIENT_LEGAL_NAME");
            this.Property(x => x.RecipientEntprFio, "Исполнитель-ИП - (ФИО одной строкой)").Column("RECIPIENT_ENTPR_FIO");
            this.Property(x => x.PaymentDocumentID, "Идентификатор платежного документа").Column("PAYMENT_DOCUMENT_ID");
            this.Property(x => x.PaymentDocumentNumber, "Номер платежного документа").Column("PAYMENT_DOCUMENT_NUMBER");
            this.Property(x => x.Year, "Год").Column("YEAR");
            this.Property(x => x.Month, "Месяц").Column("MONTH");
            this.Property(x => x.UnifiedAccountNumber, "Единый лицевой счет").Column("UNIFIED_ACCOUNT_NUMBER");
            this.Property(x => x.FiasHouseGuid, "Глобальный уникальный идентификатор дома по ФИАС").Column("FIAS_HOUSE_GUID");
            this.Property(x => x.Apartment, "Номер жилого помещения").Column("APARTMENT");
            this.Property(x => x.Placement, "Номер комнаты жилого помещения").Column("PLACEMENT");
            this.Property(x => x.NonLivingApartment, "Номер нежилого помещения").Column("NON_LIVING_APARTMENT");
            this.Property(x => x.ConsumerSurname, "Реквизиты потребителя - Фамилия").Column("CONSUMER_SURNAME");
            this.Property(x => x.ConsumerFirstName, "Реквизиты потребителя - Имя").Column("CONSUMER_FIRST_NAME");
            this.Property(x => x.ConsumerPatronymic, "Реквизиты потребителя - Отчество").Column("CONSUMER_PATRONYMIC");
            this.Property(x => x.ConsumerInn, "Реквизиты потребителя - ИНН").Column("CONSUMER_INN");
            this.Property(x => x.ServiceID, "Идентификатор жилищно-коммунальной услуги").Column("SERVICE_ID");
        }
    }
}