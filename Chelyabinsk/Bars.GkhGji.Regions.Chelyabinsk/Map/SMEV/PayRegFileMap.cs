namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для файлов обмена с ГИС ГМП</summary>
    public class PayRegFileMap : BaseEntityMap<PayRegFile>
    {
        
        public PayRegFileMap() : 
                base("Файл запроса платежей", "GJI_CH_PAY_REG_FILE")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PayRegRequests, "ЗАПРОС").Column("PAY_REG_REQUESTS_ID").NotNull().Fetch();
            Property(x => x.SMEVFileType, "Тип файла").Column("FILE_TYPE").NotNull();
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
