namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVOwnershipProperty;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVOwnershipPropertyFileMap : BaseEntityMap<SMEVOwnershipPropertyFile>
    {
        
        public SMEVOwnershipPropertyFileMap() : 
                base("Файл запроса", "GJI_SMEV_OW_PROPERTY_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVOwnershipProperty, "ЗАПРОС").Column("SMEV_OW_PROP_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
