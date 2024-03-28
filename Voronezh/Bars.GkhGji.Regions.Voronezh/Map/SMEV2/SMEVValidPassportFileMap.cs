namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для смэв 2/summary>
    public class SMEVValidPassportFileMap : BaseEntityMap<SMEVValidPassportFile>
    {
        
        public SMEVValidPassportFileMap() : 
                base("Файл запроса", "GJI_CH_SMEV_VALID_PASSPORT_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVValidPassport, "ЗАПРОС").Column("SMEV_VP_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
