using Bars.B4.Utils;
using Bars.Gkh.Entities;

namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;
    using B4;
    using B4.DataAccess;

    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;

    public class ContragentBankViewModel : Gkh.ViewModel.ContragentBankViewModel
    {
        public override IDataResult List(IDomainService<ContragentBank> domain, BaseParams baseParams)
        {
            var regopId = baseParams.Params.GetAs<long>("regopid", ignoreCase: true);

            if (regopId == 0)
            {
                return base.List(domain, baseParams);
            }

            var loadParams = baseParams.GetLoadParam();

            var contragentId = Container.ResolveDomain<RegOperator>().Get(regopId).Return(x => x.Contragent.Id);

            var data = domain.GetAll()
                .Where(x => x.Contragent.Id == contragentId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Bik,
                    x.Okonh,
                    x.Okpo,
                    x.CorrAccount,
                    x.SettlementAccount,
                    x.Description
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}