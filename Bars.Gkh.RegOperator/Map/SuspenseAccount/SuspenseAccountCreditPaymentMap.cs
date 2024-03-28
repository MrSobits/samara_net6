/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class SuspenseAccountCreditPaymentMap : BaseImportableEntityMap<SuspenseAccountCreditPayment>
///     {
///         public SuspenseAccountCreditPaymentMap() : base("REGOP_SUSPACC_CREDIT")
///         {
///             Map(x => x.CreditPayment, "CPAYMENT", true, 0m);
///             Map(x => x.PercentPayment, "PPAYMENT", true, 0m);
///             References(x => x.Credit, "CREDIT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Account, "ACCOUNT_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities;
    using System;

    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Сущность связи между счетом нвс и кредитом"</summary>
    public class SuspenseAccountCreditPaymentMap : BaseImportableEntityMap<SuspenseAccountCreditPayment>
    {
        
        public SuspenseAccountCreditPaymentMap() : 
                base("Сущность связи между счетом нвс и кредитом", "REGOP_SUSPACC_CREDIT")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Account, "Счет нвс").Column("ACCOUNT_ID").NotNull().Fetch();
            Reference(x => x.Credit, "Кредит").Column("CREDIT_ID").NotNull().Fetch();
            Property(x => x.PercentPayment, "PercentPayment").Column("PPAYMENT").DefaultValue(0m).NotNull();
            Property(x => x.CreditPayment, "CreditPayment").Column("CPAYMENT").DefaultValue(0m).NotNull();
        }
    }
}
