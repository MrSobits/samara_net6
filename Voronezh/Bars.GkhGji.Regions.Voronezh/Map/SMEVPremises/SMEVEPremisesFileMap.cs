namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Voronezh.Entities.SMEVPremises;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVEPremisesFileMap : BaseEntityMap<SMEVPremisesFile>
    {
        
        public SMEVEPremisesFileMap() : 
                base("Файл запроса", "GJI_SMEV_PREMISES_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVPremises, "ЗАПРОС").Column("SMEV_PREMISES_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
