namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVSNILSFileMap : BaseEntityMap<SMEVSNILSFile>
    {
        
        public SMEVSNILSFileMap() : 
                base("Файл запроса к ВС МВД", "GJI_CH_SMEV_SNILS_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVSNILS, "ЗАПРОС").Column("SMEV_SNILS_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
