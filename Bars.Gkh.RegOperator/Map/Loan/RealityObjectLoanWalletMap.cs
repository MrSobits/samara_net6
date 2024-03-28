/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.Loan
/// {
///     using B4.DataAccess.ByCode;
///     using Entities.Loan;
/// 
///     public class RealityObjectLoanWalletMap : BaseImportableEntityMap<RealityObjectLoanWallet>
///     {
///         public RealityObjectLoanWalletMap() : base("REGOP_RO_LOAN_WALLET")
///         {
///             References(x => x.Loan, "REGOP_RO_LOAN_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Wallet, "TARGET_WALLET_ID", ReferenceMapConfig.NotNullAndFetch);
/// 
///             Map(x => x.Sum, "SUM", true);
///             Map(x => x.ReturnedSum, "RETURNED_SUM", true);
///             Map(x => x.TypeSourceLoan, "TYPE_SOURCE_LOAN", true);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.Loan
{
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.Loan;
    
    
    /// <summary>Маппинг для "Связь кошельков домов для займа"</summary>
    public class RealityObjectLoanWalletMap : BaseImportableEntityMap<RealityObjectLoanWallet>
    {
        
        public RealityObjectLoanWalletMap() : 
                base("Связь кошельков домов для займа", "REGOP_RO_LOAN_WALLET")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Loan, "Займ").Column("REGOP_RO_LOAN_ID").NotNull().Fetch();
            Reference(x => x.Wallet, "Получатель").Column("TARGET_WALLET_ID").NotNull().Fetch();
            Property(x => x.TypeSourceLoan, "Тип источника").Column("TYPE_SOURCE_LOAN").NotNull();
            Property(x => x.Sum, "Сумма").Column("SUM").NotNull();
            Property(x => x.ReturnedSum, "Погашенная сумма").Column("RETURNED_SUM").NotNull();
        }
    }
}
