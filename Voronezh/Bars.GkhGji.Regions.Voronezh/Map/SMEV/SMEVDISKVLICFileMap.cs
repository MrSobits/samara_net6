namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    public class SMEVDISKVLICFileMap : BaseEntityMap<SMEVDISKVLICFile>
    {
        
        public SMEVDISKVLICFileMap() : 
                base("Файл запроса к ВС ФНС", "GJI_CH_SMEV_DISKVLIC_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVDISKVLIC, "ЗАПРОС").Column("SMEV_DISKVLIC_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
