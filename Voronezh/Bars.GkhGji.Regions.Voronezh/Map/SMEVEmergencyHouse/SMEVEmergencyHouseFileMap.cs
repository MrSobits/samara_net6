namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVEmergencyHouse;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVEmergencyHouseFileMap : BaseEntityMap<SMEVEmergencyHouseFile>
    {
        
        public SMEVEmergencyHouseFileMap() : 
                base("Файл запроса", "GJI_CH_SMEV_EMERGENCY_HOUSE_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVEmergencyHouse, "ЗАПРОС").Column("SMEV_EM_HOUSE_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
