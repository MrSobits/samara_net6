namespace Bars.GkhGji.Regions.Nso.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities.Dict;

	public class NsoProtocolSurveySubjectRequirement : BaseEntity
    {
        public virtual NsoProtocol Protocol { get; set; }

        public virtual SurveySubjectRequirement Requirement { get; set; }
    }
}