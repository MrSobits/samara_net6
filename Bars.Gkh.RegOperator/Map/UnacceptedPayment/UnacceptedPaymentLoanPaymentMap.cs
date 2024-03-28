/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class UnacceptedPaymentLoanPaymentMap : BaseImportableEntityMap<UnacceptedPaymentLoanPayment>
///     {
///         public UnacceptedPaymentLoanPaymentMap()
///             : base("REGOP_UNACCEPT_PAY_LOAN")
///         {
///             Map(x => x.PaymentSum, "PSUM", true, 0m);
///             References(x => x.LoanPayment, "LOAN_PAYMENT_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.UnacceptedPayment, "UNACC_PAYMENT_ID", ReferenceMapConfig.NotNullAndFetch);
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

    /// <summary>Маппинг для "Связь между неподтвержденной оплатой и оплатой займа"</summary>
    public class UnacceptedPaymentLoanPaymentMap : BaseImportableEntityMap<UnacceptedPaymentLoanPayment>
    {
        
        public UnacceptedPaymentLoanPaymentMap() : 
                base("Связь между неподтвержденной оплатой и оплатой займа", "REGOP_UNACCEPT_PAY_LOAN")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.UnacceptedPayment, "Неподтвержденная оплата").Column("UNACC_PAYMENT_ID").NotNull().Fetch();
            Reference(x => x.LoanPayment, "Оплата займа").Column("LOAN_PAYMENT_ID").NotNull().Fetch();
            Property(x => x.PaymentSum, "Сумма, которая пошла на оплату займа").Column("PSUM").DefaultValue(0m).NotNull();
        }
    }
}
