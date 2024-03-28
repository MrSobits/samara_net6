namespace Bars.GkhCr.Overhaul.ViewModels
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;
    using Gkh.Domain;

    public class TypeWorkCrViewModel : BaseViewModel<TypeWorkCr>
    {
        public override IDataResult List(IDomainService<TypeWorkCr> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
            var objectCrId = baseParams.Params.GetAs<long>("objectCrId");

            var objectCr = this.Container.Resolve<IDomainService<ObjectCr>>().Load(objectCrId);
            var federalLaw185 = baseParams.Params.GetAs("federalLaw185", false);

            var dictControlDate = this.Container.Resolve<IDomainService<ControlDate>>().GetAll()
               .Where(y => y.ProgramCr.Id == objectCr.ProgramCr.Id)
               .Select(x => new { x.Work.Id, x.Date })
               .ToDictionary(x => x.Id, x => x.Date);

            var stage1Dict =
                Container.Resolve<IDomainService<TypeWorkCrVersionStage1>>()
                         .GetAll()
                         .Where(x => x.TypeWorkCr.ObjectCr.Id == objectCrId)
                         .GroupBy(x => x.TypeWorkCr.Id)
                         .ToDictionary(x => x.Key, y => new
                                                            {
                                                                    y.First().CalcBy,
                                                                    y.First().UnitMeasure,
                                                                    Volume = y.Sum(x => x.Volume),
                                                                    Sum = y.Sum(x => x.Sum)

                                                            });

            var data = domainService.GetAll()
                .Where(x => x.ObjectCr.Id == objectCrId)
                .WhereIf(federalLaw185, x => x.FinanceSource.TypeFinance == TypeFinance.FederalLaw)
                .Select(x => new
                   {
                       x.Id,
                       WorkName = x.Work.Name,
                       WorkId = x.Work.Id,
                       x.Work.TypeWork,
                       UnitMeasureName = x.Work.UnitMeasure.Name,
                       FinanceSourceName = x.FinanceSource.Name,
                       x.HasPsd,
                       x.Volume,
                       x.SumMaterialsRequirement,
                       x.Sum,
                       x.Description,
                       x.DateStartWork,
                       x.DateEndWork,
                       x.VolumeOfCompletion,
                       x.ManufacturerName,
                       x.PercentOfCompletion,
                       x.CostSum,
                       x.CountWorker,
                       StageWorkCrName = x.StageWorkCr.Name,
                       x.AdditionalDate,
                       x.IsActive,
                       x.IsDpkrCreated,
                       x.YearRepair
                   })
                   .AsEnumerable()
                   .Select(x => new
                                    {
                                       x.Id,
                                       x.WorkName,
                                       x.TypeWork,
                                       x.UnitMeasureName,
                                       x.FinanceSourceName,
                                       x.HasPsd,
                                       x.Volume,
                                       x.SumMaterialsRequirement,
                                       x.Sum,
                                       x.Description,
                                       x.DateStartWork,
                                       x.DateEndWork,
                                       x.VolumeOfCompletion,
                                       x.ManufacturerName,
                                       x.PercentOfCompletion,
                                       x.CostSum,
                                       x.CountWorker,
                                       x.StageWorkCrName,
                                       x.AdditionalDate,
                                       x.IsActive,
                                       x.IsDpkrCreated,
                                       x.YearRepair,
                                       ControlDate = dictControlDate.ContainsKey(x.WorkId) ? dictControlDate[x.WorkId] : null,
                                       WorkFinSourceName = string.Format("{0} ({1})", x.WorkName, x.FinanceSourceName) ,//поле нужно для акта выполненных работ
                                       DpkrSum = stage1Dict.ContainsKey(x.Id) ? stage1Dict[x.Id].Sum : 0M,
                                       DpkrVolume = stage1Dict.ContainsKey(x.Id) ? stage1Dict[x.Id].Volume : 0M,
                                       DpkrUnitMeasure = stage1Dict.ContainsKey(x.Id) ? stage1Dict[x.Id].CalcBy != PriceCalculateBy.Volume ? 
                                                        stage1Dict[x.Id].CalcBy.GetEnumMeta().Display  : stage1Dict[x.Id].UnitMeasure.Name : null
                })
                .AsQueryable()
                .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<TypeWorkCr> domainService, BaseParams baseParams)
        {
            var obj = domainService.Get(baseParams.Params.GetAsId());

            return new BaseDataResult(new
            {
                obj.Id,
                obj.Work,
                RealityObject = new
                {
                    obj.ObjectCr.RealityObject.Id,
                    obj.ObjectCr.RealityObject.Address
                },
                ProgramCr = new
                {
                    obj.ObjectCr.ProgramCr.Id,
                    obj.ObjectCr.ProgramCr.Name
                },
                obj.FinanceSource,
                obj.YearRepair,
                obj.AdditionalDate,
                obj.CostSum,
                obj.CountWorker,
                obj.DateEndWork,
                obj.DateStartWork,
                obj.Description,
                obj.HasPsd,
                obj.IsActive,
                obj.IsDpkrCreated,
                obj.ManufacturerName,
                obj.PercentOfCompletion,
                StageWorkCr = obj.StageWorkCr.Return(x => x.Id),
                obj.Sum,
                obj.SumMaterialsRequirement,
                obj.Volume,
                obj.VolumeOfCompletion
            });
        }
    }
}