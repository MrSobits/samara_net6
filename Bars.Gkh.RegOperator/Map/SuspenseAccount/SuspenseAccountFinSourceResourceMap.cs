/// <mapping-converter-backup>
/// namespace Bars.Gkh.RegOperator.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class SuspenseAccountFinSourceResourceMap : BaseImportableEntityMap<SuspenseAccountFinSourceResource>
///     {
///         public SuspenseAccountFinSourceResourceMap()
///             : base("REGOP_SUSPACC_FIN_RES")
///         {
///             Map(x => x.DistributionType, "DISTRIB_TYPE");
/// 
///             References(x => x.FinSourceResource, "FIN_RESOURCE_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.SuspenseAccount, "SUSPACC_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.RegOperator.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.RegOperator.Entities.SuspenseAccountFinSourceResource"</summary>
    public class SuspenseAccountFinSourceResourceMap : BaseImportableEntityMap<SuspenseAccountFinSourceResource>
    {
        
        public SuspenseAccountFinSourceResourceMap() : 
                base("Bars.Gkh.RegOperator.Entities.SuspenseAccountFinSourceResource", "REGOP_SUSPACC_FIN_RES")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.SuspenseAccount, "SuspenseAccount").Column("SUSPACC_ID").NotNull().Fetch();
            Reference(x => x.FinSourceResource, "FinSourceResource").Column("FIN_RESOURCE_ID").NotNull().Fetch();
            Property(x => x.DistributionType, "DistributionType").Column("DISTRIB_TYPE");
        }
    }
}
