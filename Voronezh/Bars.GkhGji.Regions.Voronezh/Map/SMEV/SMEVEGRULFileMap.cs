namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVEGRULFileMap : BaseEntityMap<SMEVEGRULFile>
    {
        
        public SMEVEGRULFileMap() : 
                base("Файл запроса к ВС ЕГРЮЛ", "GJI_CH_SMEV_EGRUL_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVEGRUL, "ЗАПРОС").Column("SMEV_EGRUL_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
