namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для файлов обмена с ГИС ГМП</summary>
    public class ERKNMDictFileMap : BaseEntityMap<ERKNMDictFile>
    {
        
        public ERKNMDictFileMap() : 
                base("Файл запроса справочника ЕРКНМ", "GJI_CH_GIS_ERKNM_DICT_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ERKNMDict, "Запрос").Column("GIS_ERKNM_DICT_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
