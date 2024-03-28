namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для файлов обмена с ГИС ГМП</summary>
    public class GISERPFileMap : BaseEntityMap<GISERPFile>
    {
        
        public GISERPFileMap() : 
                base("Файл запроса к ГИС ГМП", "GJI_CH_GIS_ERP_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.GISERP, "ЗАПРОС").Column("GIS_ERP_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
