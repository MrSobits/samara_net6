namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActCheck
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.SurveyAction;

    public class SurveyActionViewModel : BaseViewModel<SurveyAction>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<SurveyAction> domainService, BaseParams baseParams)
        {
            var surveyAction = domainService.Get(baseParams.Params.GetAsId());

            return surveyAction.IsNull()
                ? new BaseDataResult()
                : new BaseDataResult(new
                {
                    surveyAction.Id,
                    surveyAction.ActionType,
                    surveyAction.Date,
                    surveyAction.Number,
                    CreationPlace = surveyAction.CreationPlace?.GetFiasProxy(this.Container),
                    surveyAction.StartDate,
                    StartTime = surveyAction.StartTime?.ToString("HH:mm"),
                    surveyAction.EndDate,
                    EndTime = surveyAction.EndTime?.ToString("HH:mm"),
                    ExecutionPlace = surveyAction.ExecutionPlace?.GetFiasProxy(this.Container),
                    surveyAction.ContrPersFio,
                    surveyAction.ContrPersBirthDate,
                    surveyAction.ContrPersBirthPlace,
                    surveyAction.ContrPersRegistrationAddress,
                    surveyAction.ContrPersLivingAddressMatched,
                    surveyAction.ContrPersLivingAddress,
                    surveyAction.ContrPersIsHirer,
                    surveyAction.ContrPersPhoneNumber,
                    surveyAction.ContrPersWorkPlace,
                    surveyAction.ContrPersPost,
                    surveyAction.IdentityDocType,
                    surveyAction.IdentityDocSeries,
                    surveyAction.IdentityDocNumber,
                    surveyAction.IdentityDocIssuedOn,
                    surveyAction.IdentityDocIssuedBy,
                    surveyAction.RepresentFio,
                    surveyAction.RepresentWorkPlace,
                    surveyAction.RepresentPost,
                    surveyAction.RepresentProcurationNumber,
                    surveyAction.RepresentProcurationIssuedOn,
                    surveyAction.RepresentProcurationValidPeriod,
                    surveyAction.ContinueDate,
                    ContinueStartTime = surveyAction.ContinueStartTime?.ToString("HH:mm"),
                    ContinueEndTime = surveyAction.ContinueEndTime?.ToString("HH:mm"),
                    surveyAction.ProtocolReaded,
                    surveyAction.HasRemark
                });
        }
    }
}