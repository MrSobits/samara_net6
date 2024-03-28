namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    public class ViewMotivationConclusionMap : PersistentObjectMap<ViewMotivationConclusion>
    {
        
        public ViewMotivationConclusionMap() : 
                base("Реестр мотивировочных заключение", "VIEW_GJI_MOTIVATION_CONCLUSION")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.State, "Статус").Column("STATE_ID").Fetch();
            this.Property(x => x.InspectionId, "Id основания").Column("INSPECTION_ID");
            this.Property(x => x.MunicipalityId, "Муниципальный район").Column("MU_ID");
            this.Property(x => x.TypeBase, "Тип основания").Column("TYPE_BASE");
            this.Property(x => x.MunicipalityName, "Муниципальный район").Column("MUNICIPALITY");
            this.Property(x => x.InspectionBasis, "Основание").Column("INSPECTION_BASIS");
            this.Property(x => x.ContragentName, "Юридическое лицо").Column("CONTRAGENT_NAME");
            this.Property(x => x.PersonInspection, "Объект проверки").Column("PERSON_INSPECTION");
            this.Property(x => x.PhysicalPerson, "ФИО").Column("PHYSICAL_PERSON");
            this.Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            this.Property(x => x.RealityObjectCount, "Количество домов").Column("RO_COUNT");
            this.Property(x => x.RealityObjectIds, "Id домов").Column("RO_IDS");
            this.Property(x => x.DocumentNumber, "Номер документа").Column("DOCUMENT_NUMBER");
            this.Property(x => x.DocumentNum, "Номер").Column("DOCUMENT_NUM");
            this.Property(x => x.Inspectors, "Инспекторы").Column("INSPECTORS");
            this.Property(x => x.TypeDocumentGji, "Тип документа").Column("TYPE_DOC");
        }
    }
}
