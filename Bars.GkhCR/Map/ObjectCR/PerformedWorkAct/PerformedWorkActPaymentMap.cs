namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Распоряжение к оплате акта."</summary>
    public class PerformedWorkActPaymentMap : BaseImportableEntityMap<PerformedWorkActPayment>
    {
        
        public PerformedWorkActPaymentMap() : 
                base("Распоряжение к оплате акта.", "CR_OBJ_PER_ACT_PAYMENT")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.PerformedWorkAct, "Акт выполненных работ, по которому было создано данное распоряжение").Column("ACT_ID").Fetch();
            this.Property(x => x.DateDisposal, "Дата распоряжения").Column("DATE_DISPOSAL");
            this.Property(x => x.DatePayment, "Дата оплаты").Column("DATE_PAYMENT");
            this.Property(x => x.TypeActPayment, "Вид оплаты").Column("TYPE_ACT_PAYMENT").NotNull();
            this.Reference(x => x.Document, "Документ").Column("DOCUMENT_ID").Fetch();
            this.Property(x => x.Sum, "Сумма к оплате, руб.").Column("SUM").NotNull();
            this.Property(x => x.Percent, "Percent").Column("PERCENT").NotNull();
            this.Property(x => x.Paid, "Сумма оплачено, руб").Column("SUM_PAID").NotNull();
            this.Property(x => x.TransferGuid, "Guid трансфера денег для оплаты акта").Column("TRANSFER_GUID").Length(250);
        }
    }
}
