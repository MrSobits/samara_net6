using Bars.B4;
using Bars.Gkh.Entities;
using Bars.Gkh.Utils;
using System.Linq;

namespace Bars.Gkh.ViewModel
{
    public class ASSberbankClientViewModel : BaseViewModel<ASSberbankClient>
    {
        public override IDataResult List(IDomainService<ASSberbankClient> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.ClientCode
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}
