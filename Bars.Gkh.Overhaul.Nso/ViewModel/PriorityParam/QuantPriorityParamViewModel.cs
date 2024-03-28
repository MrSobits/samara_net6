namespace Bars.Gkh.Overhaul.Nso.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;

    public class QuantPriorityParamViewModel : BaseViewModel<QuantPriorityParam>
    {
        public override IDataResult List(IDomainService<QuantPriorityParam> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var service = Container.Resolve<IDomainService<QuantPriorityParam>>();

            var code = baseParams.Params.GetAs<string>("code");

            var data = service.GetAll()
                .Where(x => x.Code == code)
                .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
        }
    }
}
