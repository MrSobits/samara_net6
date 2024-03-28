namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для файлов обмена с ГИС ГМП</summary>
    public class GisGmpFileMap : BaseEntityMap<GisGmpFile>
    {
        
        public GisGmpFileMap() : 
                base("Файл запроса к ГИС ГМП", "GJI_CH_GIS_GMP_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.GisGmp, "ЗАПРОС").Column("GIS_GMP_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
