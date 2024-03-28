namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IDisposalSurveyObjectiveService
	{
        IDataResult AddSurveyObjectives(BaseParams baseParams);
        IDataResult AddSurveyObjectives(long documentId, long[] ids);
    }
}