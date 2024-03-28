namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVCertInfoMap : BaseEntityMap<SMEVCertInfo>
    {
        
        public SMEVCertInfoMap() : 
                base("", "SMEV_CH_CERT_INFO")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Property(x => x.RequestDate, "Дата запроса").Column("REQUEST_DATE").NotNull();
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");
            Reference(x => x.FileInfo, "File").Column("FILE_INFO_ID").NotNull().Fetch();
        }
    }
}
