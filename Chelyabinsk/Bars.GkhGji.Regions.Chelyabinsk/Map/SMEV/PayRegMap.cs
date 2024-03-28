namespace Bars.GkhGji.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Entities;

    /// <summary>Маппинг для оплат с ГИС ГМП</summary>
    public class PayRegMap : BaseEntityMap<PayReg>
    {
        
        public PayRegMap() : 
                base("Файл запроса платежей", "GJI_CH_PAY_REG")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.GisGmp, "Начисление").Column("GIS_GMP_ID").Fetch();
            Property(x => x.Amount, "Сумма").Column("AMOUNT");
            Property(x => x.Kbk, "КБК").Column("KBK");
            Property(x => x.OKTMO, "ОКТМО").Column("OKTMO");
            Property(x => x.PaymentDate, "Дата оплаты").Column("PAYMENT_DATE");
            Property(x => x.PaymentId, "ИД оплаты").Column("PAYMENT_ID");
            Property(x => x.Purpose, "Основание").Column("PURPOSE");
            Property(x => x.SupplierBillID, "УИН").Column("UIN");

            Property(x => x.PaymentOrg, "Тип организации, через которую проведена оплата").Column("PAYMENT_ORG");
            Property(x => x.PaymentOrgDescr, "Организация, через которую проведена оплата").Column("PAYMENT_ORG_DESCR");
            Property(x => x.PayerId, "Идентификатор плательщика").Column("PAYER_ID");
            Property(x => x.PayerAccount, "Счёт плательщика").Column("PAYER_ACCOUNT");
            Property(x => x.PayerName, "Наименование плательщика").Column("PAYER_NAME");
            Property(x => x.BdiStatus, "Статус плательщика (поле 101)").Column("BDI_STATUS");
            Property(x => x.BdiPaytReason, "Основание платежа (поле 106)").Column("BDI_PAYT_REASON");
            Property(x => x.BdiTaxPeriod, "Период платежа (поле 107)").Column("BDI_TAXPERIOD");
            Property(x => x.BdiTaxDocNumber, "Номер документа (поле 108)").Column("BDI_TAXDOCNUMBER");
            Property(x => x.BdiTaxDocDate, "Дата документа (поле 109)").Column("BDI_TAXDOCDATE");
            Property(x => x.AccDocDate, "Дата платежного документа").Column("ACCDOCDATE");
            Property(x => x.AccDocNo, "Номер платежного документа").Column("ACCDOCNO");
            Property(x => x.Status, "Статус платежа").Column("STATUS");
            Property(x => x.Reconcile, "Сквитировано").Column("RECONSILE");
            Property(x => x.IsPayFineAdded, "Добавлен в оплаты штрафов").Column("IS_PAYFINE_ADDED");
            //Reference(x => x.FileInfo, "Файл").Column("FILE_INFO_ID").Fetch();
        }
    }
}
