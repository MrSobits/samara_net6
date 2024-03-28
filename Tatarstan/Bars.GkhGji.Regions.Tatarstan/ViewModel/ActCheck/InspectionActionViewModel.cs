namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActCheck
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InspectionAction;

    public class InspectionActionViewModel : BaseViewModel<InspectionAction>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<InspectionAction> domainService, BaseParams baseParams)
        {
            var inspectionAction = domainService.Get(baseParams.Params.GetAsId());

            return inspectionAction.IsNull()
                ? new BaseDataResult()
                : new BaseDataResult(new
                {
                    inspectionAction.Id,
                    inspectionAction.ActionType,
                    inspectionAction.Date,
                    inspectionAction.Number,
                    CreationPlace = inspectionAction.CreationPlace?.GetFiasProxy(this.Container),
                    inspectionAction.StartDate,
                    StartTime = inspectionAction.StartTime?.ToString("HH:mm"),
                    inspectionAction.EndDate,
                    EndTime = inspectionAction.EndTime?.ToString("HH:mm"),
                    ExecutionPlace = inspectionAction.ExecutionPlace?.GetFiasProxy(this.Container),
                    inspectionAction.ContrPersFio,
                    inspectionAction.ContrPersBirthDate,
                    inspectionAction.ContrPersBirthPlace,
                    inspectionAction.ContrPersRegistrationAddress,
                    inspectionAction.ContrPersLivingAddressMatched,
                    inspectionAction.ContrPersLivingAddress,
                    inspectionAction.ContrPersIsHirer,
                    inspectionAction.ContrPersPhoneNumber,
                    inspectionAction.ContrPersWorkPlace,
                    inspectionAction.ContrPersPost,
                    inspectionAction.IdentityDocType,
                    inspectionAction.IdentityDocSeries,
                    inspectionAction.IdentityDocNumber,
                    inspectionAction.IdentityDocIssuedOn,
                    inspectionAction.IdentityDocIssuedBy,
                    inspectionAction.RepresentFio,
                    inspectionAction.RepresentWorkPlace,
                    inspectionAction.RepresentPost,
                    inspectionAction.RepresentProcurationNumber,
                    inspectionAction.RepresentProcurationIssuedOn,
                    inspectionAction.RepresentProcurationValidPeriod,
                    inspectionAction.ContinueDate,
                    ContinueStartTime = inspectionAction.ContinueStartTime?.ToString("HH:mm"),
                    ContinueEndTime = inspectionAction.ContinueEndTime?.ToString("HH:mm"),
                    inspectionAction.HasViolation,
                    inspectionAction.HasRemark
                });
        }
    }
}