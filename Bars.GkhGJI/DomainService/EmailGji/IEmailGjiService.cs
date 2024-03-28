namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IEmailGjiService
    {      
        IDataResult GetNextQuestion(BaseParams baseParams);
        IDataResult SkipAndGetNextQuestion(BaseParams baseParams);
        IDataResult SaveAndGetNextQuestion(BaseParams baseParams);
        IDataResult PrintReport(BaseParams baseParams);
        IDataResult DeclineEmail(BaseParams baseParams);
        IDataResult RegisterEmail(BaseParams baseParams);
        IDataResult GetListAttachments(BaseParams baseParams);
    }
}