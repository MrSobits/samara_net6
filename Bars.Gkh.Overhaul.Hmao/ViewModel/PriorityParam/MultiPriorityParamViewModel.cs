namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;
    using Gkh.Utils;

    public class MultiPriorityParamViewModel : BaseViewModel<MultiPriorityParam>
    {
        public override IDataResult List(IDomainService<MultiPriorityParam> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var code = baseParams.Params.GetAs<string>("code");

            var data = domainService.GetAll()
                .Where(x => x.Code == code)
                .Filter(loadParam, Container)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Point,
                    Codes = x.StoredValues != null
                        ? x.StoredValues.AggregateWithSeparator(y => y.Code, ",")
                        : null,
                    Names = x.StoredValues != null
                        ? x.StoredValues.AggregateWithSeparator(y => y.Name, ", ")
                        : null
                })
                .ToList();

            return new ListDataResult(data.AsQueryable().Order(loadParam).Paging(loadParam).ToList(), data.Count);
        }
    }
}
