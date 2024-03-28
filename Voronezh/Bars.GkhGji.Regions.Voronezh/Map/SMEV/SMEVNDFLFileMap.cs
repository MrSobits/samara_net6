namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    public class SMEVNDFLFileMap : BaseEntityMap<SMEVNDFLFile>
    {
        
        public SMEVNDFLFileMap() : 
                base("Файл запроса к ВС", "GJI_CH_SMEV_NDFL_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVNDFL, "ЗАПРОС").Column("SMEV_NDFL_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
