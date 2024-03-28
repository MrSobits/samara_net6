namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для смэв 2/summary>
    public class SMEVStayingPlaceFileMap : BaseEntityMap<SMEVStayingPlaceFile>
    {
        
        public SMEVStayingPlaceFileMap() : 
                base("Файл запроса", "GJI_CH_SMEV_STAYING_PLACE_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVStayingPlace, "ЗАПРОС").Column("SMEV_SP_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
