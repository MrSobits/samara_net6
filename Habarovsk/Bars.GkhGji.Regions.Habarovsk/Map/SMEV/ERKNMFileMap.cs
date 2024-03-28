namespace Bars.GkhGji.Regions.Habarovsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для файлов обмена с ГИС ГМП</summary>
    public class ERKNMFileMap : BaseEntityMap<ERKNMFile>
    {
        
        public ERKNMFileMap() : 
                base("Файл запроса к ГИС ГМП", "GJI_CH_GIS_ERKNM_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ERKNM, "ЗАПРОС").Column("GIS_ERKNM_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
