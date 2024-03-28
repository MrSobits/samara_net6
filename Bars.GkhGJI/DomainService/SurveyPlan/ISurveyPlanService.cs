namespace Bars.GkhGji.DomainService.SurveyPlan
{
    using Bars.B4;
    using Bars.GkhGji.Entities.SurveyPlan;

    public interface ISurveyPlanService
    {
        IDataResult AddCandidates(BaseParams baseParams);

        IDataResult CreateCandidates(BaseParams baseParams);

        void CreateOrUpdateSurvey(SurveyPlanContragent contragent);

        void CreateSurveys(SurveyPlan surveyPlan);
    }
}