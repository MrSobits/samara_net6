namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities.Dict;

    public class ChelyabinskProtocolSurveySubjectRequirement : BaseEntity
    {
        public virtual ChelyabinskProtocol Protocol { get; set; }

        public virtual SurveySubjectRequirement Requirement { get; set; }
    }
}