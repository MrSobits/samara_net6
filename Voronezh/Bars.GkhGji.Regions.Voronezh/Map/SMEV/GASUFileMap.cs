namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для файлов обмена с ГИС ГМП</summary>
    public class GASUFileMap : BaseEntityMap<GASUFile>
    {
        
        public GASUFileMap() : 
                base("Файл запроса к ГИС ГМП", "GJI_CH_CMEV_GASU_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.GASU, "ЗАПРОС").Column("GASU_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
