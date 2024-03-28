namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    /// <summary>Маппинг для "Документы аварийного дома"</summary>
    public class EmergencyObjectDocumentsMap : BaseImportableEntityMap<EmergencyObjectDocuments>
    {
        public EmergencyObjectDocumentsMap() : 
                base("Документы аварийного дома", "GKH_EMERGENCY_DOCUMENTS")
        {
        }
        
        protected override void Map()
        {
            this.Property(x => x.RequirementPublicationDate, "Дата издания требования").Column("REQ_PUB_DATE");
            this.Property(x => x.RequirementDocumentNumber, "Номер документа").Column("REQ_DOC_NUM");
            this.Property(x => x.DecreePublicationDate, "Дата издания постановления").Column("DEC_PUB_DATE");
            this.Property(x => x.DecreeRequisitesMpa, "Реквизиты МПА").Column("DEC_REQ_MPA");
            this.Property(x => x.DecreeMpaPublicationDate, "Дата опубликования МПА").Column("DEC_PUB_DATE_MPA");
            this.Property(x => x.DecreeMpaRegistrationDate, "Дата регистрации МПА, изданного в Управлении Росреестра по РТ").Column("DEC_REG_DATE_MPA");
            this.Property(x => x.DecreeMpaNotificationDate, "Дата уведомления Управления Росреестра по РТ об изданном МПА").Column("DEC_NTF_DATE_MPA");
            this.Property(x => x.AgreementPublicationDate, "Дата издания соглашения").Column("AGR_PUB_DATE");

			this.Reference(x => x.EmergencyObject, "Аварийность жилого дома").Column("EMERGENCY_OBJ_ID").NotNull().Fetch();
            this.Reference(x => x.RequirementFile, "Файл требования").Column("REQ_FILE_INFO_ID").Fetch();
            this.Reference(x => x.DecreeFile, "Файл постановления").Column("DEC_FILE_INFO_ID").Fetch();
            this.Reference(x => x.AgreementFile, "Файл соглашения").Column("AGR_FILE_INFO_ID").Fetch();
        }
    }
}
