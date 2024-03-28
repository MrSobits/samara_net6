namespace Bars.GkhGji.Regions.Tomsk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class AdministrativeCaseDocViewModel : BaseViewModel<AdministrativeCaseDoc>
    {
        public override IDataResult List(IDomainService<AdministrativeCaseDoc> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var loadParams = GetLoadParam(baseParams);
            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data =
                domainService.GetAll()
                    .Where(x => x.AdministrativeCase.Id == documentId)
                     .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}
