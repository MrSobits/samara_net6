/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Tomsk.Map
/// {
///     using Bars.B4.DataAccess;
///     using Bars.GkhGji.Regions.Tomsk.Entities;
/// 
///     public class ActCheckVerificationResultMap : BaseEntityMap<ActCheckVerificationResult> 
///     {
///         public ActCheckVerificationResultMap()
///             : base("GJI_TOMSK_AC_VERIFRESULT")
///         {
///             Map(x => x.TypeVerificationResult, "TYPE_VERIF_RESULT");
/// 
///             References(x => x.ActCheck, "ACT_CHECK_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Tomsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Tomsk.Entities.ActCheckVerificationResult"</summary>
    public class ActCheckVerificationResultMap : BaseEntityMap<ActCheckVerificationResult>
    {
        
        public ActCheckVerificationResultMap() : 
                base("Bars.GkhGji.Regions.Tomsk.Entities.ActCheckVerificationResult", "GJI_TOMSK_AC_VERIFRESULT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.TypeVerificationResult, "TypeVerificationResult").Column("TYPE_VERIF_RESULT");
            Reference(x => x.ActCheck, "ActCheck").Column("ACT_CHECK_ID").NotNull().Fetch();
        }
    }
}
