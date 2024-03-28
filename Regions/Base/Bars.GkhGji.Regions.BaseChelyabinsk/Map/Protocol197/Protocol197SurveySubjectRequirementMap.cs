namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Protocol197
{
    using Bars.B4.DataAccess.ByCode;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class Protocol197SurveySubjectRequirementMap : BaseEntityMap<Protocol197SurveySubjectRequirement>
    {
		public Protocol197SurveySubjectRequirementMap()
            : base("GJI_PROTOCOL197_SUR_REQ")
        {
            this.References(x => x.Protocol197, "PROTOCOL_ID");
            this.References(x => x.Requirement, "REQUIREMENT_ID");
        }
    }
}