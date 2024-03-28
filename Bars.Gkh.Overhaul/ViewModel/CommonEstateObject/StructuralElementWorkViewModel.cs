namespace Bars.Gkh.Overhaul.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Entities;

    public class StructuralElementWorkViewModel : BaseViewModel<StructuralElementWork>
    {
        public override IDataResult List(IDomainService<StructuralElementWork> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            var element = loadParam.Filter.GetAs<long>("element");

            var queryable = domainService.GetAll()
                .WhereIf(element > 0, x => x.StructuralElement.Id == element)
                .Select(x => new
                {
                    x.Id,
                    StructuralElement = x.StructuralElement.Id,
                    x.Job,
                    WorkName = x.Job.Work.Name,
                    x.Job.Work.UnitMeasure
                })
                .Filter(loadParam, Container);

            return new ListDataResult(queryable.Order(loadParam).ToList(), queryable.Count());
        }
    }
}