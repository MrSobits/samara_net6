namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для оплат с ГИС ГМП</summary>
    public class GISGMPPaymentsMap : BaseEntityMap<GISGMPPayments>
    {
        
        public GISGMPPaymentsMap() : 
                base("Файл запроса к ГИС ГМП", "GJI_CH_GIS_GMP_PAYMENTS")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.GisGmp, "ЗАПРОС").Column("GIS_GMP_ID").NotNull().Fetch();
            Property(x => x.Amount, "Сумма").Column("AMOUNT");
            Property(x => x.Kbk, "КБК").Column("KBK");
            Property(x => x.OKTMO, "ОКТМО").Column("OKTMO");
            Property(x => x.PaymentDate, "Дата оплаты").Column("PAYMENT_DATE");
            Property(x => x.PaymentId, "ИД оплаты").Column("PAYMENT_ID");
            Property(x => x.Purpose, "Основание").Column("PRPOSE");
            Property(x => x.SupplierBillID, "УИН").Column("UIN");
            Property(x => x.Reconcile, "Сквитировано").Column("RECONSILE");
            Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").Fetch();
        }
    }
}
