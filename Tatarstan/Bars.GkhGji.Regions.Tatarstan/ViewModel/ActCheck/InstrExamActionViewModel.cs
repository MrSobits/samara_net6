namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActCheck
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InstrExamAction;

    public class InstrExamActionViewModel : BaseViewModel<InstrExamAction>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<InstrExamAction> domainService, BaseParams baseParams)
        {
            var instrExamAction = domainService.Get(baseParams.Params.GetAsId());

            return instrExamAction.IsNull()
                ? new BaseDataResult()
                : new BaseDataResult(new
                {
                    instrExamAction.Id,
                    instrExamAction.ActionType,
                    instrExamAction.Date,
                    instrExamAction.Number,
                    CreationPlace = instrExamAction.CreationPlace?.GetFiasProxy(this.Container),
                    instrExamAction.StartDate,
                    StartTime = instrExamAction.StartTime?.ToString("HH:mm"),
                    instrExamAction.EndDate,
                    EndTime = instrExamAction.EndTime?.ToString("HH:mm"),
                    ExecutionPlace = instrExamAction.ExecutionPlace?.GetFiasProxy(this.Container),
                    instrExamAction.ContrPersFio,
                    instrExamAction.ContrPersBirthDate,
                    instrExamAction.ContrPersBirthPlace,
                    instrExamAction.ContrPersRegistrationAddress,
                    instrExamAction.ContrPersLivingAddressMatched,
                    instrExamAction.ContrPersLivingAddress,
                    instrExamAction.ContrPersIsHirer,
                    instrExamAction.ContrPersPhoneNumber,
                    instrExamAction.ContrPersWorkPlace,
                    instrExamAction.ContrPersPost,
                    instrExamAction.IdentityDocType,
                    instrExamAction.IdentityDocSeries,
                    instrExamAction.IdentityDocNumber,
                    instrExamAction.IdentityDocIssuedOn,
                    instrExamAction.IdentityDocIssuedBy,
                    instrExamAction.RepresentFio,
                    instrExamAction.RepresentWorkPlace,
                    instrExamAction.RepresentPost,
                    instrExamAction.RepresentProcurationNumber,
                    instrExamAction.RepresentProcurationIssuedOn,
                    instrExamAction.RepresentProcurationValidPeriod,
                    instrExamAction.Territory,
                    instrExamAction.Premise,
                    instrExamAction.TerritoryAccessDenied,
                    instrExamAction.HasViolation,
                    instrExamAction.UsingEquipment,
                    instrExamAction.HasRemark
                });
        }
    }
}