namespace Bars.Gkh.DomainService.Impl
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;

    public class UnitMeasureService : IUnitMeasureService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult ListNoPaging(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var data = Container.Resolve<IDomainService<UnitMeasure>>().GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name
                })
                .Filter(loadParams, Container)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Name);

            return new ListDataResult(data.ToList(), data.Count());
        }
    }
}