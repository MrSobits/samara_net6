namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;


    /// <summary>Маппинг для "Ответ по заявке"</summary>
    public class MKDLicRequestAnswerMap : BaseEntityMap<MKDLicRequestAnswer>
    {
        
        public MKDLicRequestAnswerMap() : 
                base("Ответ по обращению", "GJI_MKD_LIC_REQUEST_ANSWER")
        {

        }
        
        protected override void Map()
        {
            this.Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME").Length(300);
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUM").Length(300);
            this.Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(2000);
            this.Property(x => x.IsMoved, "Признак того что ответ отправлен куда нужно").Column("IS_MOVED");
            this.Property(x => x.Year, "Год ответа (необходим для формирования номера документа)").Column("YEAR");
            this.Property(x => x.ExecDate, "Дата исполнения (направления ответа)").Column("EXEC_DATE");
            this.Property(x => x.ExtendDate, "Дата продления срока исполнения").Column("EXTEND_DATE");
            this.Property(x => x.IsUploaded, "Признак об успешной/не успешной загрузки в ЕАИС ОГ").Column("IS_UPLOADED");
            this.Property(x => x.AdditionalInfo, "Дополнительная информация об не успешной загрузки в ЕАИС ОГ").Column("ADDITIONAL_INFO");
            this.Property(x => x.TypeAppealAnswer, "Тип документа ответа").Column("ANSWER_TYPE");
            this.Property(x => x.TypeAppealFinalAnswer, "Тип документа ответа").Column("ANSWER_FINAL_TYPE");

            this.Reference(x => x.MKDLicRequest, "Заявка").Column("MKD_LIC_REQUEST_ID").NotNull().Fetch();
            this.Reference(x => x.Addressee, "Адресат").Column("REVENUE_SOURCE_ID").Fetch();
            this.Reference(x => x.AnswerContent, "Содержание ответа").Column("ANSWER_CONTENT_ID").Fetch();
            this.Reference(x => x.Executor, "Исполнитель").Column("INSPECTOR_ID").Fetch();
            this.Reference(x => x.Signer, "Подписант").Column("SIGNER_ID").Fetch();
            this.Reference(x => x.File, "Файл").Column("FILE_INFO_ID").Fetch();
            this.Reference(x => x.FileDoc, "Файл").Column("FILE_DOC_INFO_ID").Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Reference(x => x.ConcederationResult, "Результат рассмотрения").Column("CONCED_RESULT_ID");
            this.Reference(x => x.FactCheckingType, "Вид проверки факта").Column("FACT_CHECK_TYPE_ID");

            Property(x => x.GisGkhGuid, "ГИС ЖКХ GUID").Column("GIS_GKH_GUID").Length(36);
            Property(x => x.GisGkhTransportGuid, "ГИС ЖКХ Transport GUID").Column("GIS_GKH_TRANSPORT_GUID").Length(36);
            Property(x => x.GisGkhAttachmentGuid, "ГИС ЖКХ Attachment GUID").Column("GIS_GKH_ATTACHMENT_GUID").Length(36);
            Property(x => x.Hash, "Hash").Column("HASH");
            Reference(x => x.RedirectContragent, "Контрагент для перенаправления обращения").Column("REDIRECT_CONTRAGENT_ID").Fetch();

            this.Property(x => x.Address, "Адрес").Column("ADDRESS").Length(2000);
            this.Property(x => x.SerialNumber, "Порядковый номер для АС ДОУ").Column("SERIAL_NUMBER").Length(50);
        }
    }
}
