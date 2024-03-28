namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVEGRNFileMap : BaseEntityMap<SMEVEGRNFile>
    {
        
        public SMEVEGRNFileMap() : 
                base("Файл запроса к ВС Предоставления данных из ФГИС ЕГРП", "GJI_CH_SMEV_EGRN_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVEGRN, "ЗАПРОС").Column("SMEV_EGRN_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
