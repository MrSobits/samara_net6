namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActCheck
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.ExplanationAction;

    public class ExplanationActionViewModel : BaseViewModel<ExplanationAction>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<ExplanationAction> domainService, BaseParams baseParams)
        {
            var explanationAction = domainService.Get(baseParams.Params.GetAsId());

            return explanationAction.IsNull()
                ? new BaseDataResult()
                : new BaseDataResult(new
                {
                    explanationAction.Id,
                    explanationAction.ActionType,
                    explanationAction.Date,
                    explanationAction.Number,
                    CreationPlace = explanationAction.CreationPlace?.GetFiasProxy(this.Container),
                    explanationAction.StartDate,
                    StartTime = explanationAction.StartTime?.ToString("HH:mm"),
                    explanationAction.EndDate,
                    EndTime = explanationAction.EndTime?.ToString("HH:mm"),
                    ExecutionPlace = explanationAction.ExecutionPlace?.GetFiasProxy(this.Container),
                    explanationAction.ContrPersFio,
                    explanationAction.ContrPersBirthDate,
                    explanationAction.ContrPersBirthPlace,
                    explanationAction.ContrPersRegistrationAddress,
                    explanationAction.ContrPersLivingAddressMatched,
                    explanationAction.ContrPersLivingAddress,
                    explanationAction.ContrPersWorkPlace,
                    explanationAction.ContrPersPost,
                    explanationAction.IdentityDocType,
                    explanationAction.IdentityDocSeries,
                    explanationAction.IdentityDocNumber,
                    explanationAction.IdentityDocIssuedOn,
                    explanationAction.IdentityDocIssuedBy,
                    explanationAction.ContrPersType,
                    explanationAction.ContrPersContragent,
                    explanationAction.AttachmentName,
                    explanationAction.AttachmentFile,
                    explanationAction.Explanation
                });
        }
    }
}