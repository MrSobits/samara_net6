/// <mapping-converter-backup>
/// namespace Bars.Gkh.Map
/// {
///     using Bars.Gkh.Entities;
///     using Bars.B4.DataAccess.ByCode;
/// 
///     /// <summary>
///     /// Заявка на доступ к экзамену 
///     /// </summary>
///     public class PersonRequestToExamMap : BaseImportableEntityMap<PersonRequestToExam>
///     {
///         public PersonRequestToExamMap()
///             : base("GKH_PERSON_REQUEST_EXAM")
///         {
/// 
///             Map(x => x.RequestNum, "REQUEST_NUM");
///             Map(x => x.RequestDate, "REQUEST_DATE");
///             Map(x => x.ExamDate, "EXAM_DATE");
///             Map(x => x.ExamTime, "EXAM_TIME");
///             Map(x => x.CorrectAnswersPercent, "COR_ANSWER_PERCENT");
///             Map(x => x.ProtocolNum, "PROTOCOL_NUM");
///             Map(x => x.ProtocolDate, "PROTOCOL_DATE");
/// 
///             References(x => x.Person, "PERSON_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.RequestFile, "REQUEST_FILE_ID", ReferenceMapConfig.Fetch);
/// 			References(x => x.PersonalDataConsentFile, "PERSONAL_DATA_CONSENT_FILE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.ProtocolFile, "PROTOCOL_FILE_ID", ReferenceMapConfig.Fetch);
///             References(x => x.State, "STATE_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>Маппинг для "Заявка на доступ к экзамену"</summary>
    public class PersonRequestToExamMap : BaseImportableEntityMap<PersonRequestToExam>
    {        
        public PersonRequestToExamMap() : 
                base("Заявка на доступ к экзамену", "GKH_PERSON_REQUEST_EXAM")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.Person, "Person").Column("PERSON_ID").NotNull().Fetch();
            this.Property(x => x.RequestSupplyMethod, "Способ подачи заявления").Column("SUPPLY_METHOD").NotNull().DefaultValue(RequestSupplyMethod.NotSet);
            this.Property(x => x.RequestNum, "Номер").Column("REQUEST_NUM").Length(250);
            this.Property(x => x.RequestDate, "Дата").Column("REQUEST_DATE");
            this.Property(x => x.RequestTime, "Время").Column("REQUEST_TIME").Length(250);
            this.Reference(x => x.RequestFile, "Файл заявления").Column("REQUEST_FILE_ID").Fetch();
            this.Reference(x => x.PersonalDataConsentFile, "Файл согласия на обработку перс.данных").Column("PERSONAL_DATA_CONSENT_FILE_ID").Fetch();
            this.Property(x => x.NotificationNum, "Номер уведомления").Column("NOTIF_NUM").Length(250);
            this.Property(x => x.NotificationDate, "Дата уведомления").Column("NOTIF_DATE");
            this.Property(x => x.IsDenied, "Отказ").Column("IS_DENIED");

            this.Property(x => x.ExamDate, "Дата экзамена").Column("EXAM_DATE");
            this.Property(x => x.ExamTime, "Время экзамена").Column("EXAM_TIME").Length(250);
            this.Property(x => x.CorrectAnswersPercent, "Количество набранных баллов").Column("COR_ANSWER_PERCENT");
            this.Property(x => x.ProtocolNum, "Номер протокола").Column("PROTOCOL_NUM").Length(250);
            this.Property(x => x.ProtocolDate, "Дата протокола").Column("PROTOCOL_DATE");
            this.Reference(x => x.ProtocolFile, "Файл протокола").Column("PROTOCOL_FILE_ID").Fetch();
            this.Property(x => x.ResultNotificationNum, "Номер уведомления(из блока Результаты экзамена)").Column("RESULT_NOTIF_NUM").Length(250);
            this.Property(x => x.ResultNotificationDate, "Дата уведомления(из блока Результаты экзамена)").Column("RESULT_NOTIF_DATE");
            this.Property(x => x.MailingDate, "Дата отправки почтой").Column("MAILING_DATE");

            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
        }
    }
}