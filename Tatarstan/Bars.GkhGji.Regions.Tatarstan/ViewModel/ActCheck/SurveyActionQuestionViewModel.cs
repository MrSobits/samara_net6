namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActCheck
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.SurveyAction;

    public class SurveyActionQuestionViewModel : BaseViewModel<SurveyActionQuestion>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<SurveyActionQuestion> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var actCheckActionId = baseParams.Params.GetAsId("actCheckActionId");

            return domainService
                .GetAll()
                .WhereIf(actCheckActionId > 0, x => x.SurveyAction.Id == actCheckActionId)
                .ToListDataResult(loadParams, this.Container);
        }
    }
}