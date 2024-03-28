using System.Linq;
using Bars.B4;
using Bars.B4.Utils;
using Bars.Gkh.RegOperator.Entities.Dict;

namespace Bars.Gkh.RegOperator.ViewModels
{
    public class RegopServiceLogViewModel : BaseViewModel<RegopServiceLog>
    {
        public override IDataResult List(IDomainService<RegopServiceLog> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .OrderIf(loadParams.Order.Length == 0, false, x => x.DateExecute)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
