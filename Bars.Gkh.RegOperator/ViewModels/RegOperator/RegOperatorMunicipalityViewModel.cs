namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Entities;

    public class RegOperatorMunicipalityViewModel : BaseViewModel<RegOperatorMunicipality>
    {
        public override IDataResult List(IDomainService<RegOperatorMunicipality> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var regOperatorId = baseParams.Params.GetAs<long>("regOperatorId");

            var data = domainService.GetAll()
                .Where(x => x.RegOperator.Id == regOperatorId)
                .Select(x => new
                    {
                        x.Id,
                        Municipality = x.Municipality.Name
                    })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}