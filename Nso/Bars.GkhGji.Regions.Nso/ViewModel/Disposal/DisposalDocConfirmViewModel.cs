namespace Bars.GkhGji.Regions.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.Nso.Entities.Disposal;

    public class DisposalDocConfirmViewModel : BaseViewModel<DisposalDocConfirm>
    {
        public override IDataResult List(IDomainService<DisposalDocConfirm> domainService, BaseParams baseParams)
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
