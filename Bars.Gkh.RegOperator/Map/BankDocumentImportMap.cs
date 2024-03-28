namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;

    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Документы, загруженные из банка"</summary>
    public class BankDocumentImportMap : BaseImportableEntityMap<BankDocumentImport>
    {

        public BankDocumentImportMap() :
                base("Документы, загруженные из банка", "REGOP_BANK_DOC_IMPORT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.ExportId, "Идентификатор для экспорта").Column("EXPORT_ID").NotNull();
            this.Property(x => x.ImportDate, "Дата загрузки документа").Column("IMPORT_DATE");
            this.Property(x => x.DocumentType, "Информация о типе загруженного документа").Column("DOCUMENT_TYPE").Length(250);
            this.Property(x => x.DocumentDate, "Дата, указанная в загружаемом документе").Column("DOCUMENT_DATE");
            this.Property(x => x.AcceptDate, "Дата подтверждения").Column("ACCEPT_DATE");
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER").Length(250);
            this.Property(x => x.ImportedSum, "Сумма по реестру").Column("IMPORTED_SUM");
            this.Property(x => x.Status, "Состояние импорта").Column("STATUS").NotNull();
            this.Reference(x => x.LogImport, "Ссылка на лог загрузки и сам документ").Column("LOG_IMPORT_ID").Fetch();
            this.Property(x => x.PaymentAgentCode, "Код платежного агента").Column("PAYMENT_AGENT_CODE").Length(250);
            this.Property(x => x.PaymentAgentName, "Наименование платежного агента").Column("PAYMENT_AGENT_NAME").Length(250);
            this.Property(x => x.DistributePenalty, "Распределять на задолженность по пени").Column("DISTR_PENALTY").NotNull();
            this.Property(x => x.PersonalAccountDeterminationState, "Статус определения ЛС").Column("PAD_STATE").NotNull();
            this.Property(x => x.PaymentConfirmationState, "Статус подтверждения оплат").Column("PC_STATE").NotNull();
            this.Property(x => x.State, "Состояние").Column("STATE").NotNull();
            this.Property(x => x.TransferGuid, "Гуид, который запишется либо в SourceGuid, либо в TargetGuid объекта Transfer").Column("TRANSFER_GUID").Length(250);
            this.Property(x => x.BankStatement, "Строка связанной Банковской выписки").Column("BANK_STATEMENT").Length(500);
            this.Property(x => x.CheckState, "Проверка пройдена").Column("CHECK_STATE").NotNull();
            this.Property(x => x.AcceptedSum, "Подтвержденная сумма").Column("ACCEPTED_SUM");
            this.Property(x => x.DistributedSum, "Учтенная сумма").Column("DISTRIBUTED_SUM");
            this.Property(x => x.ImportType, "Тип импорта").Column("IMPORT_TYPE").Length(255);
            this.Property(x => x.ReportDate, "Дата выгрузки отчета по документу").Column("REPORT_DATE");
        }
    }

    /// <summary>ReadOnly ExportId</summary>
    public class BankDocumentImportNhMapping : BaseHaveExportIdMapping<BankDocumentImport>
    {
    }
}
