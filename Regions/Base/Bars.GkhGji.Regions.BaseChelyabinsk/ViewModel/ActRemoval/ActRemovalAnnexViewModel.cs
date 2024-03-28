namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.ActRemoval
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    public class ActRemovalAnnexViewModel : BaseViewModel<ActRemovalAnnex>
    {
        public override IDataResult List(IDomainService<ActRemovalAnnex> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.ActRemoval.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.Name,
                    x.Description,
                    x.File,
                    x.SignedFile
                })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}