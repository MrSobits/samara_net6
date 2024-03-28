namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class MunicipalitySourceFinancingViewModel : BaseViewModel<MunicipalitySourceFinancing>
    {
        public override IDataResult List(IDomainService<MunicipalitySourceFinancing> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

            var data = domain.GetAll()
                .Where(x => x.Municipality.Id == municipalityId)
                .Select(x => new
                {
                    x.Id,
                    x.SourceFinancing
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}