namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class BelayOrganizationViewModel : BaseViewModel<BelayOrganization>
    {
        public override IDataResult List(IDomainService<BelayOrganization> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var showNotValid = baseParams.Params.ContainsKey("showNotValid") && baseParams.Params["showNotValid"].ToBool();

            var data = domain.GetAll()
                .WhereIf(!showNotValid, x => x.ActivityGroundsTermination == GroundsTermination.NotSet)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Contragent.Municipality.Name,
                    ContragentName = x.Contragent.Name,
                    x.Contragent.Inn,
                    x.ActivityGroundsTermination
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}