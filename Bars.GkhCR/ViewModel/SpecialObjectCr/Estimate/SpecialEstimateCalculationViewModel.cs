namespace Bars.GkhCr.DomainService
{
    using System.Linq;

    using B4;
    using B4.Utils;

    using Gkh.DataResult;
    using Entities;
    using Gkh.Domain;

    public class SpecialEstimateCalculationViewModel : BaseViewModel<SpecialEstimateCalculation>
    {
        public override IDataResult List(IDomainService<SpecialEstimateCalculation> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var programmCrId = baseParams.Params.GetAs<long>("programmCrId");
            var twId = baseParams.Params.GetAsId("twId");

            var filter = domainService.GetAll()
                .Where(x => x.TypeWorkCr.Id == twId);
            
            var data = this.Container.Resolve<ISpecialEstimateCalculationService>().GetFilteredByOperator()
                .WhereIf(programmCrId > 0, x => x.ProgramCrId == programmCrId)
                .WhereIf(twId > 0, z => filter.Any(x => x.Id == z.Id))
                .Select(x => 
                    new
                    {
                        x.Id,
                        x.ObjectCrId,
                        x.ProgramCrName,
                        Municipality = x.SettlementName == null ? x.Municipality : x.SettlementName,
                        x.RealityObjName,
                        x.TypeWorkCrCount,
                        x.EstimateCalculationsCount
                    })
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            var summaryTypeWork = data.Sum(x => x.TypeWorkCrCount);
            var summaryEstCalc = data.Sum(x => x.EstimateCalculationsCount);

            return new ListSummaryResult(
                data.Order(loadParams).Paging(loadParams).ToList(),
                totalCount,
                new
                {
                    TypeWorkCrCount = summaryTypeWork,
                    EstimateCalculationsCount = summaryEstCalc
                });
        }

        public override IDataResult Get(IDomainService<SpecialEstimateCalculation> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return obj != null ? new BaseDataResult(new
            {
                obj.Id,
                obj.ObjectCr,
                obj.ResourceStatmentDocumentName,
                TypeWorkCr = obj.TypeWorkCr != null ? new { obj.TypeWorkCr.Id, WorkName = obj.TypeWorkCr.Work.Name } : null,
                obj.EstimateDocumentName,
                obj.FileEstimateDocumentName,
                obj.ResourceStatmentDocumentNum,
                obj.EstimateDocumentNum,
                obj.FileEstimateDocumentNum,
                obj.ResourceStatmentDateFrom,
                obj.EstimateDateFrom,
                obj.FileEstimateDateFrom,
                obj.ResourceStatmentFile,
                obj.EstimateFile,
                obj.FileEstimateFile,
                obj.OtherCost,
                obj.TotalEstimate,
                obj.TotalDirectCost,
                obj.OverheadSum,
                obj.Nds,
                obj.EstimateProfit,
                obj.State,
                obj.IsSumWithoutNds,
				obj.EstimationType,
                obj.UsedInExport
            }) : new BaseDataResult();
        }
    }
}