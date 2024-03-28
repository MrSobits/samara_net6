namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class MVDPassportFileMap : BaseEntityMap<MVDPassportFile>
    {
        
        public MVDPassportFileMap() : 
                base("Файл запроса к ВС МВД", "GJI_CH_SMEV_MVDPASSPORT_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.MVDPassport, "ЗАПРОС").Column("MVDPASSPORT_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
