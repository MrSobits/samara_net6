/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Chelyabinsk.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Chelyabinsk.Entities;
/// 
///     public class ChelyabinskProtocolSurveySubjectRequirementMap : BaseEntityMap<ChelyabinskProtocolSurveySubjectRequirement>
///     {
///         public ChelyabinskProtocolSurveySubjectRequirementMap()
///             : base("GJI_NSO_PROTO_SUR_REQ")
///         {
///             References(x => x.Protocol, "PROTOCOL_ID");
///             References(x => x.Requirement, "REQUIREMENT_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Protocol
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol;

    /// <summary>Маппинг для "Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskProtocolSurveySubjectRequirement"</summary>
    public class ChelyabinskProtocolSurveySubjectRequirementMap : BaseEntityMap<ChelyabinskProtocolSurveySubjectRequirement>
    {
        
        public ChelyabinskProtocolSurveySubjectRequirementMap() : 
                base("Bars.GkhGji.Regions.Chelyabinsk.Entities.ChelyabinskProtocolSurveySubjectRequirement", "GJI_NSO_PROTO_SUR_REQ")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.Protocol, "Protocol").Column("PROTOCOL_ID");
            this.Reference(x => x.Requirement, "Requirement").Column("REQUIREMENT_ID");
        }
    }
}
