namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Disposal
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    public class DisposalAdditionalDocViewModel : BaseViewModel<DisposalAdditionalDoc>
    {
        public override IDataResult List(IDomainService<DisposalAdditionalDoc> domainService, BaseParams baseParams)
        {
            var dispId = baseParams.Params.GetAs<long>("documentId");

            var loadParam = baseParams.GetLoadParam();

            var data = domainService.GetAll()
                        .Where(x => x.Disposal.Id == dispId)
                        .Select(x => new
                        {
                            x.Id,
                            x.DocumentName
                        })
                        .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}
