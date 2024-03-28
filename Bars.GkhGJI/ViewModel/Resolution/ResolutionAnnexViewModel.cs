namespace Bars.GkhGji.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    public class ResolutionAnnexViewModel : BaseViewModel<ResolutionAnnex>
    {
        public override IDataResult List(IDomainService<ResolutionAnnex> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.Resolution.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.Name,
                    x.Description,
                    x.File,
                    x.TypeAnnex,
                    x.SignedFile,
                    x.MessageCheck,
                    x.DocumentSend,
                    x.DocumentDelivered
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}