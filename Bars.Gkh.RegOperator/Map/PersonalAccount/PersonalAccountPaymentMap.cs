/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class PersonalAccountPaymentMap : BaseImportableEntityMap<PersonalAccountPayment>
///     {
///         public PersonalAccountPaymentMap()
///             : base("REGOP_PERS_ACC_PAYMENT")
///         {
///             Map(x => x.PaymentDate, "PAYMENT_DATE", true);
///             Map(x => x.Sum, "PAYMENT_SUM", true);
///             Map(x => x.Type, "PAYMENT_TYPE", true);
///             Map(x => x.Guid, "PGUID", false, 40);
///             References(x => x.BasePersonalAccount, "PERS_ACC_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Оплата Л/С"</summary>
    public class PersonalAccountPaymentMap : BaseImportableEntityMap<PersonalAccountPayment>
    {
        
        public PersonalAccountPaymentMap() : 
                base("Оплата Л/С", "REGOP_PERS_ACC_PAYMENT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.PaymentDate, "Дата оплаты").Column("PAYMENT_DATE").NotNull();
            Property(x => x.Sum, "Сумма оплаты").Column("PAYMENT_SUM").NotNull();
            Reference(x => x.BasePersonalAccount, "Лицевой счет").Column("PERS_ACC_ID").NotNull().Fetch();
            Property(x => x.Type, "Тип оплаты").Column("PAYMENT_TYPE").NotNull();
            Property(x => x.Guid, "Guid (для связи с неподтвержденной оплатой)").Column("PGUID").Length(40);
        }
    }
}
