namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.AppealCits
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.AppealCitsExecutant"</summary>
    public class AppealOrderFileMap : BaseEntityMap<AppealOrderFile>
    {
        
        public AppealOrderFileMap() : 
                base("Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealOrder", "GJI_APPCITORDER_FILE")
        {
        }
        
        protected override void Map()
        {           
            this.Property(x => x.Description, "Description").Column("DESCRIPTION").Length(500);
            this.Reference(x => x.AppealOrder, "AppealOrder").Column("APPCITORDER_ID").NotNull().Fetch();
            this.Reference(x => x.FileInfo, "FileInfo").Column("FILE_ID").NotNull().Fetch();          
        }
    }
}