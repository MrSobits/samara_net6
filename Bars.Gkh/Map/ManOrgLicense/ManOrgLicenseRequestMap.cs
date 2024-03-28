namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;

    /// <summary>Маппинг для "Заявка на Лицензию управляющей организации"</summary>
    public class ManOrgLicenseRequestMap : BaseImportableEntityMap<ManOrgLicenseRequest>
    {
        public ManOrgLicenseRequestMap()
            :
            base("Заявка на Лицензию управляющей организации", "GKH_MANORG_LIC_REQUEST")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.DateRequest, "Дата обращения").Column("DATE_REQUEST");
            this.Property(
                x => x.RegisterNumber,
                "как обычно для всех сущностей с нумерацией делаю и стркоовый номер и тектовый нас" +
                "лучай если заходят изенить номер на маску").Column("REG_NUMBER").Length(100);
            this.Property(
                x => x.RegisterNum,
                "как обычно для всех сущностей с нумерацией делаю и стркоовый номер и тектовый нас" +
                "лучай если заходят изенить номер на маску").Column("REG_NUM");
            this.Property(x => x.ConfirmationOfDuty, "подтверждение гос пошлины").Column("CONF_DUTY").Length(1000);
            this.Property(x => x.ReasonOffers, "Основание предложения").Column("REASON_OFFERS").Length(10000);
            this.Property(x => x.ReasonRefusal, "Причина отказа").Column("REASON_REFUSAL").Length(1000);
            this.Property(x => x.Type, "Тип обращения").Column("TYPE");
            this.Property(x => x.ReplyTo, "Получатель ответа").Column("REPLY_TO");
            this.Property(x => x.RPGUNumber, "Тип обращения").Column("RPGU_NUMBER");
            this.Property(x => x.MessageId, "Тип обращения").Column("MESSAGE_ID");
            this.Property(x => x.Note, "Примечание").Column("NOTE").Length(1000);
            this.Property(x => x.Applicant, "Заявитель").Column("APPLICANT");
            this.Property(x => x.TaxSum, "Сумма пошлины").Column("TAX_SUM");
            this.Property(x => x.DeclineReason, "Причина отклонения").Column("DECLINE_REASON");
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Reference(x => x.Contragent, "Контрагент - Выбирается из УО но сохраняется Контрагент").Column("CONTRAGENT_ID");
            this.Reference(x => x.File, "File").Column("FILE_ID");
            this.Reference(x => x.ManOrgLicense, "Лицензия").Column("MANORG_LICENSE_ID");
            this.Reference(x => x.LicenseRegistrationReason, "Причины переоформления лицензии").Column("REGISTRATION_REASON_ID");
            this.Reference(x => x.LicenseRejectReason, "Причина отказа").Column("REJECT_REASON_ID");
            this.Property(x => x.NoticeAcceptanceDate, "Дата уведомления о принятии документов к рассмотрению").Column("NOTICE_ACCEPTANCE_DATE");
            this.Property(x => x.NoticeViolationDate, "Дата уведомления об устранении нарушений").Column("NOTICE_VIOLATION_DATE");
            this.Property(x => x.ReviewDate, "Дата рассмотрения документов").Column("REVIEW_DATE");
            this.Property(x => x.NoticeReturnDate, "Дата уведомления о возврате документов").Column("NOTICE_RETURN_DATE");
            this.Property(x => x.ReviewDateLk, "Дата рассмотрения документов ЛК").Column("REVIEW_DATE_LK");
            this.Property(x => x.PreparationOfferDate, "Дата подготовки мотивированного предложения").Column("PREPARATION_OFFER_DATE");
            this.Property(x => x.SendResultDate, "Дата отправки результата").Column("SEND_RESULT_DATE");
            this.Property(x => x.SendMethod, "Способ отправки").Column("SEND_METHOD");
            this.Property(x => x.OrderDate, "Дата приказа/отказа о выдаче лицензии").Column("ORDER_DATE");
            this.Property(x => x.OrderNumber, "Номер приказа/отказа о выдаче лицензии").Column("ORDER_NUMBER");
            this.Reference(x => x.OrderFile, "Номер приказа/отказа о выдаче лицензии").Column("ORDER_FILE");
            this.Property(x => x.IdSerial, "Серия документа удостоверяющего личность").Column("IDENT_SERIAL").Length(10);
            this.Property(x => x.IdNumber, "Номер документа удостоверяющего личность").Column("IDENT_NUMBER").Length(10);
            this.Property(x => x.IdIssuedDate, "Дата выдачи документа удостоверяющег оличность").Column("IDENT_ISSUEDDATE");
            this.Property(x => x.IdIssuedBy, "Кем выдан документ удостоверяющий личность").Column("IDENT_ISSUEDBY").Length(2000);
            this.Property(x => x.TypeIdentityDocument, "Документ удостоверяющий личность").Column("IDENT_TYPE");
        }
    }
}