using System.Linq;
using Bars.B4;
using Bars.B4.Modules.Security;
using Castle.Windsor;

namespace Bars.Gkh.UserActionRetention.DomainService.Impl
{
    public class UserLoginService : IUserLoginService
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<User> UserDomainService { get; set; }

        public IDataResult ListWithoutPaging(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var data = UserDomainService.GetAll()
                             .Select(x => new
                             {
                                 UserId = x.Id,
                                 UserLogin = x.Login
                             })
                             .OrderBy(x => x.UserLogin)
                             .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).ToList(), totalCount);
        }
    }
}
