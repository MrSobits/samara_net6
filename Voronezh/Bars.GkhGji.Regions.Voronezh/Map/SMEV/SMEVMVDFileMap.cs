namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVMVDFileMap : BaseEntityMap<SMEVMVDFile>
    {
        
        public SMEVMVDFileMap() : 
                base("Файл запроса к ВС МВД", "GJI_CH_SMEV_MVD_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVMVD, "ЗАПРОС").Column("SMEV_MVD_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
