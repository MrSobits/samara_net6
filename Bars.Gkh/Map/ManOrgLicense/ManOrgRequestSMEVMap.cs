namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    
    
    /// <summary>Маппинг для "Предоставляемый документ заявки на лицензию"</summary>
    public class ManOrgRequestSMEVMap : BaseEntityMap<ManOrgRequestSMEV>
    {

        public ManOrgRequestSMEVMap() : 
                base("Заявка по лицензированию", "GKH_MANORG_REQ_SMEV")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.LicRequest, "Заявка на лицензию").Column("LIC_REQUEST_ID").NotNull();
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull();
            Property(x => x.Date, "Дата запроса").Column("RPGU_REQ_DATE");
            Property(x => x.SMEVRequestState, "Статус запроса").Column("SMEV_STATE");
            Property(x => x.RequestSMEVType, "Тип запроса").Column("SMEV_TYPE");
            Property(x => x.RequestId, "Энтити запроса").Column("SMEV_ID");
        }
    }
}
