namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVFNSLicRequestFileMap : BaseEntityMap<SMEVFNSLicRequestFile>
    {
        
        public SMEVFNSLicRequestFileMap() : 
                base("Файл запроса к ВС ФНС", "GJI_CH_SMEV_FNS_LIC_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVFNSLicRequest, "ЗАПРОС").Column("SMEV_FNS_LIC_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
