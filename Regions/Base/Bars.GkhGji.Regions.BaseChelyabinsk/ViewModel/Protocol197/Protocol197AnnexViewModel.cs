namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Protocol197
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;

    public class Protocol197AnnexViewModel : BaseViewModel<Protocol197Annex>
    {
        public override IDataResult List(IDomainService<Protocol197Annex> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
				.Where(x => x.Protocol197.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.Name,
                    x.Description,
                    x.File,
                    x.SignedFile,
                    x.TypeAnnex,
                    x.MessageCheck,
                    x.DocumentSend,
                    x.DocumentDelivered
              

                })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}