/// <mapping-converter-backup>
/// namespace Bars.Gkh.Modules.ClaimWork.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Bars.Gkh.Modules.ClaimWork.Entities;
/// 
///     public class JurInstitutionRealObjMap : BaseEntityMap<JurInstitutionRealObj>
///     {
///         public JurInstitutionRealObjMap()
///             : base("CLW_JUR_INST_REAL_OBJ")
///         {
///             References(x => x.JurInstitution, "JUR_INST_ID", ReferenceMapConfig.NotNullAndFetch);
///             References(x => x.RealityObject, "REAL_OBJ_ID", ReferenceMapConfig.NotNullAndFetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    
    
    /// <summary>Маппинг для "Связь Учреждения в судебной практике и Жилого дома"</summary>
    public class JurInstitutionRealObjMap : BaseEntityMap<JurInstitutionRealObj>
    {
        
        public JurInstitutionRealObjMap() : 
                base("Связь Учреждения в судебной практике и Жилого дома", "CLW_JUR_INST_REAL_OBJ")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.JurInstitution, "Учреждение в судебной практике").Column("JUR_INST_ID").NotNull().Fetch();
            Reference(x => x.RealityObject, "Жилой дом").Column("REAL_OBJ_ID").NotNull().Fetch();
        }
    }
}
