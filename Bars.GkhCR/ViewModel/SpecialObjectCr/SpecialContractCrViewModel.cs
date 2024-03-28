namespace Bars.GkhCr.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;
    using Gkh.Domain;

    public class SpecialContractCrViewModel : BaseViewModel<SpecialContractCr>
    {
        public override IDataResult List(IDomainService<SpecialContractCr> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var objectCrId = baseParams.Params.GetAsId("objectCrId");
            var twId = baseParams.Params.GetAsId("twId");

            if (objectCrId == 0)
            {
                objectCrId = loadParams.Filter.GetAs("objectCrId", 0l);
            }

            var data = domainService.GetAll()
                .Where(x => x.ObjectCr.Id == objectCrId)
                .WhereIf(twId > 0, x => x.TypeWork.Id == twId)
                .Select(x => new
                {
                    x.Id,
                    x.TypeContractObject,
                    x.DocumentName,
                    x.DocumentNum,
                    x.Description,
                    x.DateFrom,
                    x.SumContract,
                    FinanceSourceName = x.FinanceSource.Name,
                    ContragentName = x.Contragent.Name,
                    x.BudgetMo,
                    x.BudgetSubject,
                    x.OwnerMeans,
                    x.FundMeans,
                    x.File,
                    x.State
                })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}