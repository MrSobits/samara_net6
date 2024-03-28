/// <mapping-converter-backup>
/// namespace Bars.GkhCr.Modules.ClaimWork.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Entities;
/// 
///     public class BuilderContractClaimWorkMap : BaseJoinedSubclassMap<BuildContractClaimWork>
///     {
///         public BuilderContractClaimWorkMap()
///             : base("CLW_BUILD_CLAIM_WORK", "ID")
///         {
///             References(x => x.BuildContract, "CONTRACT_ID", ReferenceMapConfig.NotNullAndFetch);
///             Map(x => x.CreationType, "TYPE_CREATION", true, (object)10);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhCr.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Modules.ClaimWork.Entities;
    using Bars.GkhCr.Modules.ClaimWork.Enums;
    
    
    /// <summary>Маппинг для "Основание претензионно исковой работы для Договоров Подряда"</summary>
    public class BuildContractClaimWorkMap : JoinedSubClassMap<BuildContractClaimWork>
    {
        
        public BuildContractClaimWorkMap() : 
                base("Основание претензионно исковой работы для Договоров Подряда", "CLW_BUILD_CLAIM_WORK")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.BuildContract, "Договор подряда").Column("CONTRACT_ID").NotNull().Fetch();
            Property(x => x.CreationType, "тип создания записи").Column("TYPE_CREATION").DefaultValue(BuildContractCreationType.Auto).NotNull();
        }
    }
}
