namespace Bars.GkhGji.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class OSPViewModel : BaseViewModel<OSP>
    {
        public override IDataResult List(IDomainService<OSP> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.Municipality.Name,
                    x.Name,
                    x.ShortName,
                    x.Town,
                    x.Street,
                    CreditOrg = x.CreditOrg.Name,
                    OKTMO = x.CreditOrg.Oktmo,
                    x.BankAccount,
                    x.KBK
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}