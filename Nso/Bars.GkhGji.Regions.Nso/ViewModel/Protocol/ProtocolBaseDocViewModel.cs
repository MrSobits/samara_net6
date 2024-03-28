namespace Bars.GkhGji.Regions.Nso.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.Nso.Entities.Protocol;

    public class ProtocolBaseDocViewModel : BaseViewModel<ProtocolBaseDocument>
    {
        public override IDataResult List(IDomainService<ProtocolBaseDocument> domain, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = loadParam.Filter.GetAs("documentId", 0L);

            var data = domain
                .GetAll()
                .Where(x => x.Protocol.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject,
                    KindBaseDocument = x.KindBaseDocument.Name,
                    x.DateDoc,
                    x.NumDoc
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}