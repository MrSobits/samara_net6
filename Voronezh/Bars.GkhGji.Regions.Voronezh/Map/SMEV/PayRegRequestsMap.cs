namespace Bars.GkhGji.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для задач по расчету категории риска</summary>
    public class PayRegRequestsMap : BaseEntityMap<PayRegRequests>
    {
        
        public PayRegRequestsMap() : 
                base("Реестр платежей", "GJI_CH_PAY_REG_REQUESTS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.MessageId, "Id запроса в системе СМЭВ3").Column("MESSAGEID");
            Reference(x => x.Inspector, "Инспектор").Column("INSPECTOR_ID").NotNull().Fetch();
            Property(x => x.CalcDate, "Дата запроса").Column("REQ_DATE").NotNull();
            Property(x => x.Answer, "Результат").Column("ANSWER");
            Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            Property(x => x.PayRegPaymentsKind, "Тип информации о платеже").Column("PAY_REG_PAYMENTS_KIND");
            Property(x => x.PayRegPaymentsType, "Тип запроса оплаты").Column("PAY_REG_PAYMENTS_TYPE");
            Property(x => x.GetPaymentsStartDate, "Дата оплат с").Column("PAY_START_DATE");
            Property(x => x.GetPaymentsEndDate, "Дата оплат по").Column("PAY_END_DATE");
        }
    }
}
