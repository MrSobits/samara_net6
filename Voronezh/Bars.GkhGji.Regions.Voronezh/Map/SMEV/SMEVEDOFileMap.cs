namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVEDOFileMap : BaseEntityMap<SMEVEDOFile>
    {
        
        public SMEVEDOFileMap() : 
                base("Файл запроса к ДО", "GJI_CH_SMEV_DO_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVEDO, "ЗАПРОС").Column("SMEV_DO_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
