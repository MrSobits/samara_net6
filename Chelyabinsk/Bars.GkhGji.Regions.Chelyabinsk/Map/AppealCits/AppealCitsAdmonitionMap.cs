namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    using Entities;


    /// <summary>Маппинг для "Обращениям граждан - Запрос"</summary>
    public class AppealCitsAdmonitionMap : BaseEntityMap<AppealCitsAdmonition>
    {

        public AppealCitsAdmonitionMap() :
                base("Обращениям граждан - Предостережение", "GJI_CH_APPCIT_ADMONITION")
        {
        }

        protected override void Map()
        {
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.DocumentName, "Документ").Column("DOCUMENT_NAME").Length(300);
            Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUM").Length(30);
            Property(x => x.PerfomanceDate, "Дата исполнения").Column("PERFORMANCE_DATE");
            Property(x => x.PerfomanceFactDate, "Дата фактического исполнения").Column("PERFORMANCE_FACT_DATE");
            Reference(x => x.AppealCits, "Обращение граждан").Column("APPCIT_ID").Fetch();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").Fetch();
            Reference(x => x.File, "Файл").Column("FILE_INFO_ID").Fetch();
            Reference(x => x.SignedFile, "Подписанный файл").Column("SIGNED_FILE_ID");
            Reference(x => x.Signature, "Подпись").Column("SIGNATURE_FILE_ID").Fetch();
            Reference(x => x.Certificate, "Сертификат").Column("CERTIFICATE_FILE_ID").Fetch();
            Reference(x => x.AnswerFile, "Файл ответа").Column("ANSWERFILE_INFO_ID").Fetch();
            Reference(x => x.Inspector, "Должностное лицо").Column("INSPECTOR_ID").Fetch();
            Reference(x => x.Executor, "Исполнитель").Column("EXECUTOR_ID").Fetch();
            Reference(x => x.RealityObject, "РО").Column("REALITY_OBJECT_ID");
            Property(x => x.ERKNMID, "Номер в ЕРКНМ").Column("ERKNMID").Length(100);
            Property(x => x.ERKNMGUID, "GUID в ЕРКНМ").Column("ERKNMGUID").Length(100);
            Property(x => x.SentToERKNM, "SentToERKNM").Column("IS_SENT").NotNull();
            Property(x => x.KindKND, "Вид контроля/надзора").Column("KIND_KND").NotNull();
            Property(x => x.PayerType, "Тип плательщика").Column("PAYER_TYPE_APPCIT_ADMONITION").NotNull();
            Property(x => x.INN, "ИНН").Column("INN_APPCIT_ADMONITION");
            Property(x => x.KPP, "КПП").Column("KPP_APPCIT_ADMONITION");
            Property(x => x.FIO, "ФИО").Column("FIO_APPCIT_ADMONITION");
            Reference(x => x.PhysicalPersonDocType, "Тип документа, удостоверяющего личность").Column("PP_DOC_TYPE_APPCIT_ADMONITION_ID");
            Property(x => x.DocumentNumberFiz, "Номер документа физлица").Column("DOCUMENT_NUMB_FIZ_APPCIT_ADMONITION");
            Property(x => x.DocumentSerial, "Серия документа физлица").Column("DOCUMENT_SERIAL_APPCIT_ADMONITION");
            Property(x => x.AccessGuid, "GUID в ЕРКНМ").Column("ACCESSGUID").Length(100);
            Property(x => x.FizINN, "ФИО").Column("FIZ_INN");
            Property(x => x.FizAddress, "ФИО").Column("FIZ_ADDR");
            Property(x => x.RiskCategory, "ФИО").Column("TYPE_RISK");
            Reference(x => x.InspectionReasonERKNM, "InspectionReasonERKNM").Column("ERKNM_REASON");
        }
    }

}
