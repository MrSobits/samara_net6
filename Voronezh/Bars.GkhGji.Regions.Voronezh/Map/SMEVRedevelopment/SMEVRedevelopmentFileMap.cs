namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVRedevelopment;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVRedevelopmentFileMap : BaseEntityMap<SMEVRedevelopmentFile>
    {
        
        public SMEVRedevelopmentFileMap() : 
                base("Файл запроса", "GJI_CH_SMEV_REDEVELOPMENT_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVRedevelopment, "ЗАПРОС").Column("SMEV_REDEV_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
