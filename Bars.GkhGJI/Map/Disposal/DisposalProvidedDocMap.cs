
namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Предоставляемые документы рапоряжения ГЖИ"</summary>
    public class DisposalProvidedDocMap : BaseEntityMap<DisposalProvidedDoc>
    {
        
        public DisposalProvidedDocMap() : 
                base("Предоставляемые документы рапоряжения ГЖИ", "GJI_DISPOSAL_PROVDOC")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Description, "Примечание").Column("DESCRIPTION").Length(2000);
            Property(x => x.ErknmGuid, "Гуид ЕРКНМ").Column("ERKNM_GUID").Length(36);
            Reference(x => x.Disposal, "Распоряжение").Column("DISPOSAL_ID").NotNull().Fetch();
            Reference(x => x.ProvidedDoc, "Предоставляемый документа").Column("PROVIDED_DOC_ID").NotNull().Fetch();
        }
    }
}
