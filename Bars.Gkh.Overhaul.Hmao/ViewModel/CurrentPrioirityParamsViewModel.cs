namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams;

    public class CurrentPrioirityParamsViewModel : BaseViewModel<CurrentPrioirityParams>
    {
        public override IDataResult List(IDomainService<CurrentPrioirityParams> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var parameters = this.Container.ResolveAll<IProgrammPriorityParam>()
                .Select(x => new { x.Name, x.Code })
                .ToDictionary(x => x.Code, x => x.Name);

            var queryable = domainService.GetAll()
                .AsEnumerable()
                .Select(x => new 
                {
                    x.Id,
                    x.Code,
                    x.Order,
                    x.Municipality,
                    Name = parameters.Get(x.Code, string.Empty)
                })
                .AsQueryable()
                .Filter(loadParam, this.Container);

            return new ListDataResult(queryable.Order(loadParam).ToList(), queryable.Count());
        }
    }
}