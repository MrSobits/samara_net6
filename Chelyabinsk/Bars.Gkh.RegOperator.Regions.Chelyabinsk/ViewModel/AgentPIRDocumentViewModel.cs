namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.ViewModel
{
    using System.Linq;
    using B4;
    using Bars.Gkh.RegOperator.Regions.Chelyabinsk.Entities;

    public class AgentPIRDocumentViewModel : BaseViewModel<AgentPIRDocument>
    {
        public override IDataResult List(IDomainService<AgentPIRDocument> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var agentPIRDebId = loadParams.Filter.GetAs("agentPIRDebtorId", 0L);
            var agentPIRDebtor = this.Container.Resolve<IDomainService<AgentPIRDebtor>>()
                .GetAll().Where(x => x.Id == agentPIRDebId)
                .FirstOrDefault();
            //TODO не фильтруются документы по пиру + кнопка скачать 
            var data = domain.GetAll()
                .Where(x => x.AgentPIRDebtor.Id == agentPIRDebId)
                .Select(x => new
                {
                    x.Id,
                    x.Number,
                    x.DebtSum,
                    x.PeniSum,
                    x.Duty,
                    x.DocumentDate,
                    x.DocumentType,
                    x.File,
                    x.Repaid,
                    YesNo = x.YesNo ? "Да" : "Нет"
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}