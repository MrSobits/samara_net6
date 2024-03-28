/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Hmao.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Entities;
/// 
///     /// <summary>
///     /// Маппинг для сущности "Доля финансирвоания работ"
///     /// </summary>
///     public class ShareFinancingCeoMap : BaseImportableEntityMap<ShareFinancingCeo>
///     {
///         public ShareFinancingCeoMap()
///             : base("OVRHL_SHARE_FINANC_CEO")
///         {
///             Map(x => x.Share, "CSHARE", true, 0);
///             References(x => x.CommonEstateObject, "CEO_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Hmao.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    
    
    /// <summary>Маппинг для "Доля финанскирования ООИ"</summary>
    public class ShareFinancingCeoMap : BaseImportableEntityMap<ShareFinancingCeo>
    {
        
        public ShareFinancingCeoMap() : 
                base("Доля финанскирования ООИ", "OVRHL_SHARE_FINANC_CEO")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.CommonEstateObject, "ООИ").Column("CEO_ID").NotNull().Fetch();
            Property(x => x.Share, "Доля").Column("CSHARE").NotNull();
        }
    }
}
