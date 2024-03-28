namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService
{
    using Bars.B4;

    public interface IAppealCitsAnswerRegistrationService
    {      
        IDataResult GetNextQuestion(BaseParams baseParams);
        IDataResult SkipAndGetNextQuestion(BaseParams baseParams);
        IDataResult RegisterAndSendAnswer(BaseParams baseParams);
        IDataResult RegisterAnswer(BaseParams baseParams);
        IDataResult GetList(BaseParams baseParams);
        IDataResult GetAnswer(BaseParams baseParams);
    }
}