namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActCheck
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.DocRequestAction;

    public class DocRequestActionViewModel : BaseViewModel<DocRequestAction>
    {
        /// <inheritdoc />
        public override IDataResult Get(IDomainService<DocRequestAction> domainService, BaseParams baseParams)
        {
            var docRequestAction = domainService.Get(baseParams.Params.GetAsId());

            return docRequestAction.IsNull()
                ? new BaseDataResult()
                : new BaseDataResult(new
                {
                    docRequestAction.Id,
                    docRequestAction.ActionType,
                    docRequestAction.Date,
                    docRequestAction.Number,
                    CreationPlace = docRequestAction.CreationPlace?.GetFiasProxy(this.Container),
                    docRequestAction.StartDate,
                    StartTime = docRequestAction.StartTime?.ToString("HH:mm"),
                    docRequestAction.EndDate,
                    EndTime = docRequestAction.EndTime?.ToString("HH:mm"),
                    ExecutionPlace = docRequestAction.ExecutionPlace?.GetFiasProxy(this.Container),
                    docRequestAction.ContrPersFio,
                    docRequestAction.ContrPersRegistrationAddress,
                    docRequestAction.RepresentFio,
                    docRequestAction.RepresentProcurationNumber,
                    docRequestAction.RepresentProcurationIssuedOn,
                    docRequestAction.ContrPersType,
                    docRequestAction.ContrPersContragent,
                    docRequestAction.DocProvidingPeriod,
                    DocProvidingAddress = docRequestAction.DocProvidingAddress?.GetFiasProxy(this.Container),
                    docRequestAction.ContrPersEmailAddress,
                    docRequestAction.PostalOfficeNumber,
                    docRequestAction.EmailAddress,
                    docRequestAction.CopyDeterminationDate,
                    docRequestAction.LetterNumber
                });
        }
    }
}