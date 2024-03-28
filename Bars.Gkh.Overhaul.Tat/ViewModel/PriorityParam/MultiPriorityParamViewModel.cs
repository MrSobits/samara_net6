namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;

    public class MultiPriorityParamViewModel : BaseViewModel<MultiPriorityParam>
    {
        public override IDataResult List(IDomainService<MultiPriorityParam> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var code = baseParams.Params.GetAs<string>("code");

            var data = domainService.GetAll()
                .Where(x => x.Code == code)
                .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
        }
    }
}
