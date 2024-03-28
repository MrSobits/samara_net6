namespace Bars.Gkh.Overhaul.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;
    using Gkh.Domain;

    public class PaysizeRealEstateTypeViewModel : BaseViewModel<PaysizeRealEstateType>
    {
        public override IDataResult List(IDomainService<PaysizeRealEstateType> domainService, BaseParams baseParams)
        {
            var recordId = baseParams.Params.GetAsId("recordId");
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Where(x => x.Record.Id == recordId)
                .Select(x => new
                {
                    x.Id,
                    x.RealEstateType.Name,
                    RealEstateType = x.RealEstateType.Id,
                    Record = x.Record.Id,
                    x.Value
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}