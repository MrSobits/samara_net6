namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActCheck
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    public class ActCheckActionViewModel : BaseViewModel<ActCheckAction>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<ActCheckAction> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var documentId = baseParams.Params.GetAsId("documentId");

            return domainService
                .GetAll()
                .WhereIf(documentId > 0, x => x.ActCheck.Id == documentId)
                .ToListDataResult(loadParams, this.Container);
        }
    }
}