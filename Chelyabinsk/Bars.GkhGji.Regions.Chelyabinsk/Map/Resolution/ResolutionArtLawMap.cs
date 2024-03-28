namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;
    
    
    /// <summary>Маппинг для "Изменения статьи закона постановления ГЖИ"</summary>
    public class ResolutionArtLawMap : BaseEntityMap<ResolutionArtLaw>
    {
        
        public ResolutionArtLawMap() : 
                base("Изменение статьи закона постановления ГЖИ", "GJI_CH_RESOLUTION_ARTLAW")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.DocumentDate, "Дата документа").Column("DOCUMENT_DATE");
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(500);
            Reference(x => x.Resolution, "Постановление").Column("RESOLUTION_ID").NotNull().Fetch();
            Reference(x => x.ArticleLawGji, "Статья закона").Column("ARTLAW_ID").Fetch();
        }
    }
}
