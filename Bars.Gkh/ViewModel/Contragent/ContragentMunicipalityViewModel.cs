namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class ContragentMunicipalityViewModel : BaseViewModel<ContragentMunicipality>
    {
        public override IDataResult List(IDomainService<ContragentMunicipality> domainService, BaseParams baseParams)
        {
            var contrId = baseParams.Params.GetAs<long>("contragentId");
            var loadParams = baseParams.GetLoadParam();

            var data = domainService.GetAll()
                .Where(x => x.Contragent.Id == contrId)
                .Select(x => new
                {
                    x.Id, 
                    x.Municipality.Name
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}