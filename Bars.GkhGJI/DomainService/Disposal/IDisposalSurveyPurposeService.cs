namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IDisposalSurveyPurposeService
	{
        IDataResult AddSurveyPurposes(BaseParams baseParams);
        IDataResult AddSurveyPurposes(long documentId, long[] ids);
    }
}