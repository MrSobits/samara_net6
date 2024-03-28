namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class CurrentPrioirityParamsViewModel : BaseViewModel<CurrentPrioirityParams>
    {
        public override IDataResult List(IDomainService<CurrentPrioirityParams> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            var queryable = domainService.GetAll().Filter(loadParam, Container);

            return new ListDataResult(queryable.Order(loadParam).ToList(), queryable.Count());
        }
    }
}