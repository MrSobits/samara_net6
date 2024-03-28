using System.Linq;
using Bars.B4;
using Bars.B4.Modules.Security;
using Bars.B4.Utils;

namespace Bars.Gkh.UserActionRetention.ViewModel
{
    public class UserLoginViewModel : BaseViewModel<User>
    {
        public override IDataResult List(IDomainService<User> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var ids = baseParams.Params.GetAs("Id", string.Empty);

            var listIds = !string.IsNullOrEmpty(ids) ? ids.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var data = domain.GetAll()
                             .WhereIf(listIds.Length > 0, x => listIds.Contains(x.Id))
                             .Select(x => new
                                 {
                                     x.Id,
                                     Name = x.Login
                                 })
                             .OrderBy(x => x.Name)
                             .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
