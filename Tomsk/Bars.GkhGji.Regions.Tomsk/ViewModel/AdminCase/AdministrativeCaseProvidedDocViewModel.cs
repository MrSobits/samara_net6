namespace Bars.GkhGji.Regions.Tomsk.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class AdministrativeCaseProvidedDocViewModel : BaseViewModel<AdministrativeCaseProvidedDoc>
    {
        public override IDataResult List(IDomainService<AdministrativeCaseProvidedDoc> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = domainService.GetAll()
                .Where(x => x.AdministrativeCase.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    ProvidedDoc = x.ProvidedDoc.Name
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}