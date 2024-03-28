/// <mapping-converter-backup>
/// namespace Bars.Gkh.Decisions.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Decisions.Nso.Entities;
/// 
///     public class UltimateDecisionMap : BaseImportableEntityMap<UltimateDecision>
///     {
///         public UltimateDecisionMap() : base("DEC_ULTIMATE_DECISION")
///         {
///             Map(x => x.StartDate, "START_DATE", true);
///             Map(x => x.IsChecked, "IS_CHECKED", true);
/// 
///             References(x => x.Protocol, "PROTOCOL_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Decisions.Nso.Map
{
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Map;

    /// <summary>Маппинг для "Протокол решения собственников жилья"</summary>
    public class UltimateDecisionMap : BaseImportableEntityMap<UltimateDecision>
    {
        
        public UltimateDecisionMap() : 
                base("Протокол решения собственников жилья", "DEC_ULTIMATE_DECISION")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.StartDate, "Дата протокола").Column("START_DATE").NotNull();
            Property(x => x.IsChecked, "Проверен ли").Column("IS_CHECKED").NotNull();
            Reference(x => x.Protocol, "Решения по дому").Column("PROTOCOL_ID").NotNull().Fetch();
        }
    }
}
