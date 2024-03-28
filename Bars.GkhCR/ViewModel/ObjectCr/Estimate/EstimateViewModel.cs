namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;
    using Gkh.DataResult;
    using Entities;

    public class EstimateViewModel : BaseViewModel<Estimate>
    {
        public override IDataResult List(IDomainService<Estimate> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var estimateCalculationId = baseParams.Params.GetAs("estimateCalculationId", 0L);

            if (estimateCalculationId == 0)
            {
                estimateCalculationId = loadParams.Filter.GetAs("estimateCalculationId", 0L);
            }

            //значение чекбокса "основные позиции". если true, то показываем только те записи, у которых поле "номер" содержит значение
            var showPrimary = baseParams.Params.GetAs("showPrimary", false);

            if (!showPrimary)
            {
                showPrimary = loadParams.Filter.GetAs("showPrimary", false);
            }

            var data = domainService.GetAll()
               .WhereIf(showPrimary, x => x.Number != null && x.Number != "")
               .Where(x => x.EstimateCalculation.Id == estimateCalculationId)
               .Select(x => new
                   {
                       x.Id,
                       x.Name,
                       x.Number,
                       x.UnitMeasure,
                       x.Reason,
                       x.MechanicSalary,
                       x.BaseSalary,
                       x.MechanicWork,
                       x.BaseWork,
                       x.TotalCount,
                       x.TotalCost,
                       x.OnUnitCount,
                       x.OnUnitCost,
                       x.MaterialCost,
                       x.MachineOperatingCost
                   })
               .Filter(loadParams, Container);

            var summaryOnUnitCount = data.Sum(x => x.OnUnitCount);
            var summaryTotalCount = data.Sum(x => x.TotalCount);
            var summaryOnUnitCost = data.Sum(x => x.OnUnitCost);
            var summaryTotalCost = data.Sum(x => x.TotalCost);
            var summaryBaseSalary = data.Sum(x => x.BaseSalary);
            var summaryMachineOperatingCost = data.Sum(x => x.MachineOperatingCost);
            var summaryMechanicSalary = data.Sum(x => x.MechanicSalary);
            var summaryMaterialCost = data.Sum(x => x.MaterialCost);
            var summaryBaseWork = data.Sum(x => x.BaseWork);
            var summaryMechanicWork = data.Sum(x => x.MechanicWork);

            var totalCount = data.Count();

            return new ListSummaryResult(
                data.Order(loadParams).Paging(loadParams).ToList(),
                totalCount,
                new
                    {
                        OnUnitCount = summaryOnUnitCount,
                        TotalCount = summaryTotalCount,
                        OnUnitCost = summaryOnUnitCost,
                        TotalCost = summaryTotalCost,
                        BaseSalary = summaryBaseSalary,
                        MachineOperatingCost = summaryMachineOperatingCost,
                        MechanicSalary = summaryMechanicSalary,
                        MaterialCost = summaryMaterialCost,
                        BaseWork = summaryBaseWork,
                        MechanicWork = summaryMechanicWork
                    });
        }
    }
}