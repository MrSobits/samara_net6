namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using Entities;
    using System.Linq;
    using B4;
    using B4.Utils;

    public class ResolutionFizViewModel : BaseViewModel<ResolutionFiz>
    {
        public override IDataResult List(IDomainService<ResolutionFiz> domainService, BaseParams baseParams)
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
                    x.DocumentNumber,
                    x.DocumentSerial,
                    PhysicalPersonDocType = x.PhysicalPersonDocType.Name,
                    x.IsRF,
                    x.PayerCode
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}