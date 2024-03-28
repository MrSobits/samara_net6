using Bars.B4;
using Bars.Gkh.InspectorMobile.Entities;
using Bars.Gkh.Utils;
using System.Linq;

namespace Bars.Gkh.InspectorMobile.ViewModels
{
    public class MpRoleViewModel : BaseViewModel<MpRole>
    {
        public override IDataResult List(IDomainService<MpRole> domainService, BaseParams baseParams)
        {
            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Role.Name
                })
                .ToListDataResult(GetLoadParam(baseParams), this.Container);
        }
    }
}