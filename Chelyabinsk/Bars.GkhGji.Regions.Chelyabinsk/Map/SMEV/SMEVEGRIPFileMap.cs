namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVEGRIPFileMap : BaseEntityMap<SMEVEGRIPFile>
    {
        
        public SMEVEGRIPFileMap() : 
                base("Файл запроса к ВС ЕГРИП", "GJI_CH_SMEV_EGRIP_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVEGRIP, "ЗАПРОС").Column("SMEV_EGRIP_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
