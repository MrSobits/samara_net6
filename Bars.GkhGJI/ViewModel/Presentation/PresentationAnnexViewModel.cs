namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class PresentationAnnexViewModel : BaseViewModel<PresentationAnnex>
    {
        public override IDataResult List(IDomainService<PresentationAnnex> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.Presentation.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.Name,
                    x.Description,
                    x.File,
                    x.SignedFile,
                    x.MessageCheck
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}