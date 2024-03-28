namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;

    public class ContragentBankViewModel : BaseViewModel<ContragentBank>
    {
        public override IDataResult List(IDomainService<ContragentBank> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            
            //contragentId - идентификатор контрагента
            var contragentId = baseParams.Params.GetAs<long>("contragentId");
            var manorgId = baseParams.Params.GetAs<long>("manorgId");

            //manorgId - идентификатор управляющей организации
            //если передан идентификатор управляющей организации
            if (manorgId > 0)
                contragentId = Container.Resolve<IDomainService<ManagingOrganization>>().Get(manorgId).Contragent.Id;

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