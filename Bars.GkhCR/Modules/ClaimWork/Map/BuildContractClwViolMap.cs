/// <mapping-converter-backup>
/// using Bars.GkhCr.Entities;
/// 
/// namespace Bars.GkhCr.Modules.ClaimWork.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
/// 
///     public class BuildContractClwViolMap : BaseEntityMap<BuildContractClwViol>
///     {
///         public BuildContractClwViolMap()
///             : base("CLW_BUILD_CLAIM_WORK_VIOL")
///         {
///             References(x => x.Violation, "VIOL_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.ClaimWork, "CLAIM_WORK_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.Note, "NOTE", false,  1000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhCr.Entities;
    
    
    /// <summary>Маппинг для "Нарушение в реестре оснований"</summary>
    public class BuildContractClwViolMap : BaseEntityMap<BuildContractClwViol>
    {
        
        public BuildContractClwViolMap() : 
                base("Нарушение в реестре оснований", "CLW_BUILD_CLAIM_WORK_VIOL")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.ClaimWork, "Основание ПИР").Column("CLAIM_WORK_ID").NotNull().Fetch();
            Reference(x => x.Violation, "Нарушение").Column("VIOL_ID").NotNull().Fetch();
            Property(x => x.Note, "Примечание").Column("NOTE").Length(1000);
        }
    }
}
