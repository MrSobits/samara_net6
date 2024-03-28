namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    public class SaldoRefreshViewModel : BaseViewModel<SaldoRefresh>
    {
        public override IDataResult List(IDomainService<SaldoRefresh> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            return domainService.GetAll()
                .Select(x => new
                    {
                        x.Id,
                        Group = x.Group.Name
                    })
                .ToListDataResult(loadParams, this.Container);
        }
    }
}