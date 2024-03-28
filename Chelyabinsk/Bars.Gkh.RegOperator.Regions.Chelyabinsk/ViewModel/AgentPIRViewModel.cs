namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.ViewModel
{
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using System.Linq;

    using B4;
    using Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Authentification;

    public class AgentPIRViewModel : BaseViewModel<AgentPIR>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }
        public override IDataResult List(IDomainService<AgentPIR> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            Operator thisOperator = UserManager.GetActiveOperator();
            var contragent = thisOperator.Contragent;
            var contragentList = OperatorContragentDomain.GetAll()
             .Where(x => x.Contragent != null)
             .Where(x => x.Operator == thisOperator)
             .Select(x => x.Contragent.Id).Distinct().ToList();
            if (contragent != null)
            {
                if (!contragentList.Contains(contragent.Id))
                {
                    contragentList.Add(contragent.Id);
                }
            }
            if (thisOperator?.Inspector == null)
            {
                var data = domain.GetAll()
                .Where(x => contragentList.Contains(x.Contragent.Id))
                .Select(x => new
                {
                    x.Id,
                    Contragent = x.Contragent != null ? x.Contragent.Name : "",
                    INN = x.Contragent != null ? x.Contragent.Inn : "",
                    x.ContractDate,
                    x.ContractNumber,
                    x.DateEnd,
                    x.DateStart,
                    x.FileInfo
                })
                .Filter(loadParams, Container);

                

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            else
            {

                var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Contragent = x.Contragent != null ? x.Contragent.Name : "",
                    INN = x.Contragent != null ? x.Contragent.Inn : "",
                    x.ContractDate,
                    x.ContractNumber,
                    x.DateEnd,
                    x.DateStart,
                    x.FileInfo
                })
                .Filter(loadParams, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
        }
    }
}