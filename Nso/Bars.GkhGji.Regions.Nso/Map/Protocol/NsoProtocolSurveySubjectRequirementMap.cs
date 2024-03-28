/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Regions.Nso.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.GkhGji.Regions.Nso.Entities;
/// 
///     public class NsoProtocolSurveySubjectRequirementMap : BaseEntityMap<NsoProtocolSurveySubjectRequirement>
///     {
///         public NsoProtocolSurveySubjectRequirementMap()
///             : base("GJI_NSO_PROTO_SUR_REQ")
///         {
///             References(x => x.Protocol, "PROTOCOL_ID");
///             References(x => x.Requirement, "REQUIREMENT_ID");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Regions.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.GkhGji.Regions.Nso.Entities.NsoProtocolSurveySubjectRequirement"</summary>
    public class NsoProtocolSurveySubjectRequirementMap : BaseEntityMap<NsoProtocolSurveySubjectRequirement>
    {
        
        public NsoProtocolSurveySubjectRequirementMap() : 
                base("Bars.GkhGji.Regions.Nso.Entities.NsoProtocolSurveySubjectRequirement", "GJI_NSO_PROTO_SUR_REQ")
        {
        }
        
        protected override void Map()
        {
            Reference(x => x.Protocol, "Protocol").Column("PROTOCOL_ID");
            Reference(x => x.Requirement, "Requirement").Column("REQUIREMENT_ID");
        }
    }
}
