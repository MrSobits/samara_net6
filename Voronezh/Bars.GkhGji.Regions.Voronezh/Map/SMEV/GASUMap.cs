namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для гис ЕРП</summary>
    public class GASUMap : BaseEntityMap<GASU>
    {
        
        public GASUMap() : 
                base("Обмен данными с ГИС ЕРП", "GJI_CH_CMEV_GASU")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.MessageId, "Id запроса в системе СМЭВ3").Column("MESSAGEID");
            Property(x => x.RequestDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.DateFrom, "Дата запроса").Column("DATE_FROM").NotNull();
            Property(x => x.DateTo, "Дата запроса").Column("DATE_TO").NotNull();
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.GasuMessageType, "Тип запроса").Column("MESSAGE_TYPE");
            Property(x => x.RequestState, "Статус запроса").Column("REQUEST_STATE");
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").Fetch();
          
        }
    }
}
