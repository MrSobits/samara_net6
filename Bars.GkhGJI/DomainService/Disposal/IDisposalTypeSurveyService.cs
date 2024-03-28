namespace Bars.GkhGji.DomainService
{
    using B4;

    public interface IDisposalTypeSurveyService
    {
        IDataResult AddTypeSurveys(BaseParams baseParams);
        IDataResult AddTypeSurveys(long documentId, long[] ids);
    }
}