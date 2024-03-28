namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVCertInfoFileMap : BaseEntityMap<SMEVCertInfoFile>
    {
        
        public SMEVCertInfoFileMap() : 
                base("", "SMEV_CH_CERT_INFO_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SMEVCertInfo, "ЗАПРОС").Column("CERT_INFO_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
