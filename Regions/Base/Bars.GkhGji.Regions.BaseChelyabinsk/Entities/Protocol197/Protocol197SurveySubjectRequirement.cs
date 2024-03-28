namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities.Dict;

    public class Protocol197SurveySubjectRequirement : BaseEntity
    {
        public virtual Protocol197 Protocol197 { get; set; }
        public virtual SurveySubjectRequirement Requirement { get; set; }
    }
}