/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.RegOperator.Entities;
/// 
///     public class CashPaymentCenterMunicipalityMap : BaseImportableEntityMap<CashPaymentCenterMunicipality>
///     {
///         public CashPaymentCenterMunicipalityMap()
///             : base("REGOP_CASHPAYM_CENTER_MU")
///         {
///             References(x => x.CashPaymentCenter, "CASHPAYM_CENTER_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.Municipality, "MUNICIPALITY_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Связь расчетно-кассвого центра с МО"</summary>
    public class CashPaymentCenterMunicipalityMap : BaseImportableEntityMap<CashPaymentCenterMunicipality>
    {
        
        public CashPaymentCenterMunicipalityMap() : 
                base("Связь расчетно-кассвого центра с МО", "REGOP_CASHPAYM_CENTER_MU")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.CashPaymentCenter, "Расчетно-кассовый центр").Column("CASHPAYM_CENTER_ID").NotNull().Fetch();
            Reference(x => x.Municipality, "МО").Column("MUNICIPALITY_ID").NotNull().Fetch();
        }
    }
}
