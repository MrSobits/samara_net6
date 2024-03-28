namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using B4;
    using Gkh.DataResult;
    using Entities;

    public class ResourceStatementViewModel : BaseViewModel<ResourceStatement>
    {
        public override IDataResult List(IDomainService<ResourceStatement> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var estimateCalculationId = baseParams.Params.GetAs("estimateCalculationId", 0l);

            if (estimateCalculationId == 0)
            {
                estimateCalculationId = loadParams.Filter.GetAs("estimateCalculationId", 0l);
            }

            var data = domainService.GetAll()
                .Where(x => x.EstimateCalculation.Id == estimateCalculationId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Number,
                    x.UnitMeasure,
                    x.Reason,
                    x.TotalCount,
                    x.TotalCost,
                    x.OnUnitCost
                })
                .Filter(loadParams, Container);

#warning Пример по получению сумм вместо того что ниже используется.
            //Попробуйте протестировать. Это работает за один запрос, вместо трех в Ваших примерах.
            //var summaryResult = data.GroupBy(x => 1)
            //    .Select(g => new
            //        {
            //            TotalCount = g.Sum(y => y.TotalCount),
            //            TotalCost = g.Sum(y => y.TotalCost),
            //            OnUnitCost = g.Sum(y => y.OnUnitCost),
            //        }).FirstOrDefault();

            var summaryTotalCount = data.Sum(x => x.TotalCount);
            var summaryTotalCost = data.Sum(x => x.TotalCost);
            var summaryOnUnitCost = data.Sum(x => x.OnUnitCost);

            var totalCount = data.Count();
            return new ListSummaryResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount, new { TotalCount = summaryTotalCount, TotalCost = summaryTotalCost, OnUnitCost = summaryOnUnitCost });
        }
    }
}