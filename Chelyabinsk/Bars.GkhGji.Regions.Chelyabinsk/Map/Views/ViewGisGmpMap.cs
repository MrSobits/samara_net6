namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;


    /// <summary>Маппинг для "Bars.GkhGji.Entities.ViewDisposal"</summary>
    public class ViewGisGmpMap : PersistentObjectMap<ViewGisGmp>
    {
        
        public ViewGisGmpMap() : 
                base("Bars.GkhGji.Entities.ViewDisposal", "VIEW_GJI_CH_GISGMP")
        {
        }
        
        protected override void Map()
        {
         
            this.Property(x => x.AltPayerIdentifier, "Идентификатор плательщика").Column("ALT_PAYER_IDENTIFIER");
            this.Property(x => x.BillFor, "Назначение платежа").Column("BILL_FOR");
            this.Property(x => x.CalcDate, "Дата начисления").Column("REQ_DATE");
            this.Property(x => x.GisGmpChargeType, "Тип начисления").Column("CHARGE_TYPE");
            this.Property(x => x.GisGmpPaymentsType, "Тип запроса оплаты").Column("GIS_GMP_PAYMENTS_TYPE");
            this.Property(x => x.Inspector, "Инициатор").Column("FIO");
            this.Property(x => x.MessageId, "Id запроса в системе СМЭВ3").Column("MESSAGEID");
            this.Property(x => x.PaymentsAmount, "Оплачено").Column("PAYMENTS_AMMOUNT");
            this.Property(x => x.Reconciled, "Сквитировано").Column("RECONCILED");
            this.Property(x => x.RequestDate, "Дата запроса").Column("OBJECT_CREATE_DATE");
            this.Property(x => x.RequestState, "Состояние запроса").Column("REQUEST_STATE");
            this.Property(x => x.TotalAmount, "Сумма начисления").Column("TOTAL_AMMOUNT");
            this.Property(x => x.UIN, "УИН").Column("UIN");

        }
    }
}
