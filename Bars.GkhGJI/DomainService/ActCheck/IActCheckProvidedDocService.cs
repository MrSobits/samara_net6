namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IActCheckProvidedDocService
    {
        IDataResult AddProvidedDocs(BaseParams baseParams);
        IDataResult AddCTListAnswers(BaseParams baseParams);
        IDataResult GetNextQuestion(BaseParams baseParams);
        IDataResult SaveAndGetNextQuestion(BaseParams baseParams);
        IDataResult PrintReport(BaseParams baseParams);
    }
}