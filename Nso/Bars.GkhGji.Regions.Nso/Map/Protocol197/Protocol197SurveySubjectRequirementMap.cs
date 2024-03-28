namespace Bars.GkhGji.Regions.Nso.Map
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GkhGji.Regions.Nso.Entities;

    public class Protocol197SurveySubjectRequirementMap : BaseEntityMap<Protocol197SurveySubjectRequirement>
    {
		public Protocol197SurveySubjectRequirementMap()
            : base("GJI_PROTOCOL197_SUR_REQ")
        {
            References(x => x.Protocol197, "PROTOCOL_ID");
            References(x => x.Requirement, "REQUIREMENT_ID");
        }
    }
}