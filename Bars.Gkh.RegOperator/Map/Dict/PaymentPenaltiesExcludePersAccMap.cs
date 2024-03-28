/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map.Dict
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities.Dict;
/// 
///     public class PaymentPenaltiesExcludePersAccMap : BaseImportableEntityMap<PaymentPenaltiesExcludePersAcc>
///     {
///         public PaymentPenaltiesExcludePersAccMap()
///             : base("REGOP_PAY_PEN_EXC_PA")
///         {
///             References(x => x.PaymentPenalties, "PAY_PENALTIES_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.PersonalAccount, "PERS_ACC_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.Dict;
    
    
    /// <summary>Маппинг для "Справочник расчетов пеней"</summary>
    public class PaymentPenaltiesExcludePersAccMap : BaseImportableEntityMap<PaymentPenaltiesExcludePersAcc>
    {
        
        public PaymentPenaltiesExcludePersAccMap() : 
                base("Справочник расчетов пеней", "REGOP_PAY_PEN_EXC_PA")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.PaymentPenalties, "Количество дней").Column("PAY_PENALTIES_ID").NotNull().Fetch();
            Reference(x => x.PersonalAccount, "Количество дней").Column("PERS_ACC_ID").NotNull().Fetch();
        }
    }
}
