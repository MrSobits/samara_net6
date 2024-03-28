namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVComplaintsRequestFileMap : BaseEntityMap<SMEVComplaintsRequestFile>
    {
        
        public SMEVComplaintsRequestFileMap() : 
                base("", "SMEV_CH_COMPLAINTS_REQUEST_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVComplaintsRequest, "ЗАПРОС").Column("SMEV_REQUEST_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
