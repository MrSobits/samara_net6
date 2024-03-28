namespace Bars.Gkh.Overhaul.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Entities;

    public class CreditOrgViewModel : BaseViewModel<CreditOrg>
    {
        public override IDataResult List(IDomainService<CreditOrg> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Inn,
                        x.Kpp,
                        x.Address,
                        x.Ogrn,
                        x.Okpo,
                        x.Bik,
                        x.CorrAccount,
                        x.MailingAddress
                    })
                .Filter(loadParams, Container);

            data = data.OrderIf(loadParams.Order.Length == 0, true, x => x.Name);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}