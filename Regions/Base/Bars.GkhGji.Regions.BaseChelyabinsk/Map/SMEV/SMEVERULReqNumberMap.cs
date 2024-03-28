namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class SMEVERULReqNumberMap : BaseEntityMap<SMEVERULReqNumber>
    {
        
        public SMEVERULReqNumberMap() : 
                base("Запрос к ВС ЕРУЛ", "GJI_CH_SMEV_ERUL_LICNUMBER")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Reference(x => x.ManOrgLicense, "Лицензия").Column("LICENSE_ID").NotNull().Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.ERULRequestType, "Тип запроса ЕРУЛ").Column("REQUEST_TYPE");
            Property(x => x.MessageId, "MessageId").Column("MESSAGE_ID");


        }
    }
}
