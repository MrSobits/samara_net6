namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    public class LicenseActionMap : BaseEntityMap<LicenseAction>
    {
        
        public LicenseActionMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.LicenseAction", "GJI_LICENSE_ACTION")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.ApplicantAgreement, "Отметка о пользовательском соглашении для передачи сведений").Column("APPLICANT_AGREEMENT").Length(500);
            this.Property(x => x.ApplicantEmail, "Email Пользователя для отправки уведомлений").Column("APPLICANT_EMAIL").Length(100);
            this.Property(x => x.ApplicantEsiaId, "Идентификатор Заявителя в ЕСИА").Column("APPLICANT_ESIA_ID").Length(200);
            this.Property(x => x.ApplicantFirstName, "Имя").Column("APPLICANT_NAME").Length(20);
            this.Property(x => x.ApplicantInn, "ИНН Пользователя").Column("APPLICANT_INN").Length(20);
            this.Property(x => x.ApplicantLastName, "Фамилия").Column("APPLICANT_LASTNAME").Length(200);
            this.Property(x => x.ApplicantMiddleName, "Отчество").Column("APPLICANT_MIDDLENAME").Length(250);
            this.Property(x => x.ApplicantOkved, "Код ОКВЭД").Column("APPLICANT_OKVED").Length(100);
            this.Property(x => x.ApplicantPhone, "Номер моб телефона Пользователя для отправки уведомлений").Column("APPLICANT_PHONE").Length(20);
            this.Property(x => x.ApplicantSnils, "СНИЛС Пользователя").Column("APPLICANT_SNILS").Length(100);
            this.Property(x => x.ApplicantType, "Тип Пользователя (ЮЛ, ФЛ, ИП)").Column("APPLICANT_TYPE").Length(100);
            this.Property(x => x.DocumentDate, "Дата выдачи документа").Column("DOCUMENT_DATE");
            this.Property(x => x.Address, "Адрес заявителя").Column("POST_ADDRESS");
            this.Property(x => x.TypeAnswer, "Тип ответа").Column("TYPE_ANSWER");
            this.Property(x => x.DocumentIssuer, "Орган выдачи документа").Column("DOCUMENT_ISSUER").Length(500);
            this.Property(x => x.DocumentName, "Наименование документа").Column("DOCUMENT_NAME").Length(500);
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER").Length(50);
            this.Property(x => x.DocumentSeries, "Серия").Column("DOCUMENT_SERIES").Length(50);
            this.Property(x => x.DocumentType, "Документ (тип)").Column("DOCUMENT_TYPE").Length(100);
            this.Reference(x => x.FileInfo, "Файл запроса").Column("FILE_INFO_ID").Fetch();
            this.Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").Fetch();
            this.Property(x => x.LicenseActionType, "Тип запроса").Column("ACTION_TYPE");
            this.Property(x => x.LicenseDate, "Дата лицензии").Column("LICENSE_DATE");
            this.Property(x => x.LicenseNumber, "Номер лицензии").Column("LICENSE_NUMBER").Length(100);
            this.Property(x => x.MiddleNameFl, "Отчество").Column("MIDDLENAMEFL").Length(250);
            this.Property(x => x.NameFl, "Имя").Column("NAMEFL").Length(20);
            this.Property(x => x.Position, "Должность").Column("POSITION").Length(200);
            this.Property(x => x.SurnameFl, "Фамилия").Column("SURNAMEFL").Length(200);
            this.Reference(x => x.State, "State").Column("STATE_ID");
            this.Reference(x => x.File, "Лицензия").Column("FILE_ID");
            this.Property(x => x.DeclineReason, "Причина отклонения").Column("DECLINE_REASON");
            this.Property(x => x.ReplyTo, "Получатель ответа").Column("REPLY_TO");
            this.Property(x => x.RPGUNumber, "Тип обращения").Column("RPGU_NUMBER");
            this.Property(x => x.MessageId, "Тип обращения").Column("MESSAGE_ID");
        }
    }
}
