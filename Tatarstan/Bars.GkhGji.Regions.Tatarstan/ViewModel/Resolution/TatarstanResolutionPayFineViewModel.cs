namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Resolution
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;

    public class TatarstanResolutionPayFineViewModel : ResolutionPayFineViewModel<TatarstanResolutionPayFine>
    {
        public override IDataResult List(IDomainService<TatarstanResolutionPayFine> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;

            var data = domainService.GetAll()
                .Where(x => x.Resolution.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    x.DocumentNum,
                    x.TypeDocumentPaid,
                    x.Amount,
                    x.GisUip,
                    x.AdmissionType
                })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}