namespace Bars.GkhGji.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Entities.Dict;

    public class AuditPurposeGjiViewModel : BaseViewModel<AuditPurposeGji>
    {
        public override IDataResult List(IDomainService<AuditPurposeGji> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Code)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}