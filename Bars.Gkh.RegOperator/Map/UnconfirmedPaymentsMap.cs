namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;


    /// <summary>Маппинг для "Неподтвержденные оплаты"</summary>
    public class UnconfirmedPaymentsMap : BaseEntityMap<UnconfirmedPayments>
    {
        
        public UnconfirmedPaymentsMap() : 
                base("Неподтвержденные оплаты", "REGOP_UNCONFIRMED_PAYMENTS")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PersonalAccount, "Л/С").Column("ACC_ID").NotNull().Fetch();
            Property(x => x.Sum, "Сумма оплаты").Column("PAYMENT_SUM");
            Property(x => x.Guid, "Guid оплаты").Column("PGUID").Length(40);
            Property(x => x.PaymentDate, "Дата оплаты").Column("PAYMENT_DATE").NotNull();
            Property(x => x.Description, "Описание").Column("DESCRIPTION").Length(400);
            Property(x => x.BankBik, "БИК Банка").Column("BANK_BIK").Length(250);
            Property(x => x.BankName, "Наименование банка").Column("BANK_NAME").Length(250);
            Property(x => x.IsConfirmed, "Признак подтверждения оплаты").Column("IS_CONFIRMED");
            Reference(x => x.File, "Ссылка на файл").Column("FILE_ID");
        }
    }
}
