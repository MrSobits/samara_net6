namespace Bars.Gkh.Overhaul.Hmao.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class QuantPriorityParamViewModel : BaseViewModel<QuantPriorityParam>
    {
        public override IDataResult List(IDomainService<QuantPriorityParam> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var service = this.Container.Resolve<IDomainService<QuantPriorityParam>>();

            var code = baseParams.Params.GetAs<string>("code");

            var data = service.GetAll()
                .Where(x => x.Code == code)
                .Filter(loadParam, this.Container);

            return new ListDataResult(data.Order(loadParam).ToList(), data.Count());
        }
    }
}