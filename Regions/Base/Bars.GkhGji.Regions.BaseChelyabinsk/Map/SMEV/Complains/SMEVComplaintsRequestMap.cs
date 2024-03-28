namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVComplaintsRequestMap : BaseEntityMap<SMEVComplaintsRequest>
    {
        
        public SMEVComplaintsRequestMap() : 
                base("", "SMEV_CH_COMPLAINTS_REQUEST")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.ComplaintId, "Результат").Column("COMPLAINT_ID");
            Property(x => x.TypeComplainsRequest, "Результат").Column("TYPE_REQUEST").NotNull();
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");
            Property(x => x.TextReq, "TextReq").Column("REQUEST_TEXT");
        }
    }
}
