namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using Bars.Gkh.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;

    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Contracts.Params;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.GkhCr.Utils;

    using Castle.Windsor;
    using Config;
    using DataResult;
    using Entities;
    using Enums;
    using Gkh.Domain;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.DomainService.Dict.RealEstateType;
    using Gkh.Entities;
    using GkhCr.DomainService;
    using GkhCr.Entities;
    using GkhCr.Enums;
    using Overhaul.Entities;

    public class OverhaulViewModels : IOverhaulViewModels
    {
        public  IWindsorContainer Container { get; set; }

        public IDataResult<IEnumerable<FinanceSourceResourceProxy>> FinanceSourceResList(IDomainService<FinanceSourceResource> domainService, BaseParams baseParams)
        {
            var typeWorkSt1Domain = this.Container.ResolveDomain<TypeWorkCrVersionStage1>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var objectCrId = baseParams.Params.GetAsId("objectCrId");

                if (objectCrId == 0)
                {
                    objectCrId = loadParams.Filter.GetAsId("objectCrId");
                }

                var dpkrSumDict = typeWorkSt1Domain.GetAll()
                    .Where(x => x.TypeWorkCr.ObjectCr.Id == objectCrId)
                    .GroupBy(x => x.TypeWorkCr.Id)
                    .ToDictionary(x => x.Key, y => y.Sum(x => x.Sum));

                var data = domainService.GetAll()
                    .Where(x => x.ObjectCr.Id == objectCrId)
                    .Select(x => new
                    {
                        x.Id,
                        FinanceSourceId = x.FinanceSource != null ? (long?)x.FinanceSource.Id : null,
                        FinanceSourceName = x.FinanceSource != null ? x.FinanceSource.Name : string.Empty,
                        x.BudgetMu,
                        x.BudgetSubject,
                        x.OwnerResource,
                        x.FundResource,
                        x.BudgetMuIncome,
                        x.BudgetSubjectIncome,
                        x.FundResourceIncome,
                        x.Year,
                        TypeWorkCr = x.TypeWorkCr.Work != null ? x.TypeWorkCr.Work.Name : string.Empty,
                        TypeWorkCrId = (long?) x.TypeWorkCr.Id,
                        BudgetMuPercent = x.BudgetMu > 0 ? x.BudgetMuIncome / x.BudgetMu * 100 : 0,
                        BudgetSubjectPercent = x.BudgetSubject > 0 ? x.BudgetSubjectIncome / x.BudgetSubject * 100 : 0,
                        FundResourcePercent = x.FundResource > 0 ? x.FundResourceIncome / x.FundResource * 100 : 0,
                    });

                var result = data
                    .AsEnumerable()
                    .Select(x => new FinanceSourceResourceProxy
                    {
                        Id = x.Id,
                        FinanceSourceId = x.FinanceSourceId,
                        FinanceSourceName = x.FinanceSourceName,
                        BudgetMu = x.BudgetMu,
                        BudgetSubject = x.BudgetSubject,
                        OwnerResource = x.OwnerResource,
                        FundResource = x.FundResource,
                        BudgetMuIncome = x.BudgetMuIncome,
                        BudgetSubjectIncome = x.BudgetSubjectIncome,
                        FundResourceIncome = x.FundResourceIncome,
                        Year = x.Year,
                        GroupField = x.Year != null ? x.Year.ToString() : x.FinanceSourceName,
                        TypeWorkCr = x.TypeWorkCr,
                        TypeWorkCrId = x.TypeWorkCrId,
                        BudgetMuPercent = x.BudgetMuPercent,
                        BudgetSubjectPercent = x.BudgetSubjectPercent,
                        FundResourcePercent = x.FundResourcePercent,
                        OtherResource =
                            dpkrSumDict.Get(x.TypeWorkCrId.ToLong()) - x.BudgetMu - x.BudgetSubject - x.OwnerResource -
                            x.FundResource,
                        Bool = x.FundResourcePercent.HasValue
                    })
                    .AsQueryable()
                    .Filter(loadParams, this.Container)
                    .Order(loadParams)
                    .OrderBy(x => x.GroupField)
                    .ThenByDescending(x => x.Bool);

                var totalCount = data.Count();

                return new GenericListResult<FinanceSourceResourceProxy>(result.ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(typeWorkSt1Domain);
            }
        }

        public IDataResult FinanceSourceResGet(IDomainService<FinanceSourceResource> domainService, BaseParams baseParams)
        {
            var typeWorkSt1Domain = this.Container.ResolveDomain<TypeWorkCrVersionStage1>();

            try
            {
                var id = baseParams.Params.GetAsId();
                var record = domainService.Get(id);

                var dpkrSum = record.TypeWorkCr != null ? typeWorkSt1Domain.GetAll()
                    .Where(x => x.TypeWorkCr.Id == record.TypeWorkCr.Id)
                    .SafeSum(x => x.Sum) : 0;

                return
                    new BaseDataResult(new
                    {
                        record.Id,
                        record.ObjectCr,
                        TypeWorkCr = record.TypeWorkCr != null ? new { record.TypeWorkCr.Id, WorkName = record.TypeWorkCr.Work.Name } : null,
                        Year = new { record.Year },
                        record.FinanceSource,
                        record.BudgetMu,
                        record.BudgetSubject,
                        record.OwnerResource,
                        record.FundResource,
                        record.BudgetMuIncome,
                        record.BudgetSubjectIncome,
                        record.FundResourceIncome,
                        OtherResource = dpkrSum - record.BudgetMu - record.BudgetSubject - record.OwnerResource -
                            record.FundResource
                    });
            }
            finally
            {
                this.Container.Release(typeWorkSt1Domain);
            }
        }

        public IDataResult DefectListList(IDomainService<DefectList> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var objectCrId = baseParams.Params.GetAs<long>("objectCrId");
            var twId = baseParams.Params.GetAsId("twId");

            if (objectCrId == 0)
            {
                objectCrId = loadParams.Filter.GetAsId("objectCrId");
            }

            if (twId == 0)
            {
                twId = loadParams.Filter.GetAsId("twId");
            }

            var service = this.Container.ResolveDomain<DefectList>();

            try
            {
                var data = service.GetAll()
                    .Where(x => x.ObjectCr.Id == objectCrId)
                    .WhereIf(twId > 0, x => x.TypeWork.Id == twId)
                    .Select(x => new
                    {
                        x.Id,
                        WorkName = x.Work.Name,
                        x.DocumentName,
                        x.DocumentDate,
                        x.State,
                        x.File,
                        x.Volume,
                        x.Sum
                    })
                    .Filter(loadParams, this.Container);

                var summarySum = data.Sum(x => x.Sum);

                var totalCount = data.Count();

                return new ListSummaryResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount,
                    new {Sum = summarySum});
            }
            finally
            {
                this.Container.Release(service);
            }
        }

        public IDataResult DefectListGet(IDomainService<DefectList> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id", 0);
            var obj = domainService.Get(id);

            var seJobService = this.Container.Resolve<IDomainService<StructuralElementWork>>();
            var workPriceService = this.Container.Resolve<IDomainService<HmaoWorkPrice>>();
            var typeWorkVersSt1Service = this.Container.Resolve<ITypeWorkStage1Service>();
            var gkhParamsService = this.Container.Resolve<IGkhParams>();
            var realEstTypeService = this.Container.Resolve<IRealEstateTypeService>();
            var realObjDomain = this.Container.ResolveDomain<RealityObject>();
            var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
            try
            {
                var gkhParams = gkhParamsService.GetParams();
                var versStage1 = typeWorkVersSt1Service.GetTypeWorkStage1(obj.Work, obj.ObjectCr);

                WorkPrice workPrice = null;

                if (versStage1 != null)
                {
                    var seQuery = seJobService.GetAll().Where(x => x.Job.Work.Id == obj.Work.Id);

                    var jobQuery =
                        seQuery.Where(
                            x =>
                                x.StructuralElement.Id ==
                                versStage1.Stage1Version.StructuralElement.StructuralElement.Id);

                    var moLevel = (MoLevel) gkhParams.GetAs<int>("MoLevel");
                    var workPriceDetermineType = config.WorkPriceDetermineType;

                    List<long> realEstTypes = null;

                    if (workPriceDetermineType == WorkPriceDetermineType.WithRealEstType)
                    {
                        realEstTypes = realEstTypeService.GetRealEstateTypes(
                            realObjDomain.GetAll().Where(x => x.Id == obj.ObjectCr.RealityObject.Id))
                            .Select(x => x.Value)
                            .FirstOrDefault() ?? new List<long>();
                    }

                    workPrice =
                        workPriceService.GetAll()
                            .WhereIf(moLevel == MoLevel.MunicipalUnion,
                                x => x.Municipality.Id == obj.ObjectCr.RealityObject.Municipality.Id)
                            .WhereIf(moLevel == MoLevel.Settlement && obj.ObjectCr.RealityObject.MoSettlement != null,
                                x => x.Municipality.Id == obj.ObjectCr.RealityObject.MoSettlement.Id)
                            .WhereIf(
                                workPriceDetermineType == WorkPriceDetermineType.WithCapitalGroup &&
                                obj.ObjectCr.RealityObject.CapitalGroup != null,
                                x => x.CapitalGroup.Id == obj.ObjectCr.RealityObject.CapitalGroup.Id)
                            .WhereIf(workPriceDetermineType == WorkPriceDetermineType.WithRealEstType,
                                x => realEstTypes.Contains(x.RealEstateType.Id))
                            .FirstOrDefault(
                                x =>
                                    x.Year ==
                                    (obj.DocumentDate.HasValue ? obj.DocumentDate.Value.Year : DateTime.Now.Year) &&
                                    jobQuery.Any(y => y.Job.Id == x.Job.Id));
                }

                return
                    new BaseDataResult(new
                    {
                        obj.Id,
                        obj.ObjectCr,
                        obj.State,
                        obj.Work,
                        obj.File,
                        obj.DocumentDate,
                        obj.DocumentName,
                        obj.Volume,
                        obj.CostPerUnitVolume,
                        obj.Sum,
                        CalculateBy = versStage1 != null ? (PriceCalculateBy?) versStage1.CalcBy : null,
                        DpkrVolume = versStage1 != null ? versStage1.Volume : 0M,
                        MargCost = versStage1 != null && workPrice != null
                            ? versStage1.CalcBy == PriceCalculateBy.Volume
                                ? workPrice.NormativeCost
                                : workPrice.SquareMeterCost
                            : 0M,
                        DpkrCost = versStage1 != null ? versStage1.Sum : 0M,
                        obj.UsedInExport
                    });
            }
            finally
            {
                this.Container.Release(seJobService);
                this.Container.Release(workPriceService);
                this.Container.Release(typeWorkVersSt1Service);
                this.Container.Release(gkhParamsService);
                this.Container.Release(realEstTypeService);
                this.Container.Release(realObjDomain);

            }
        }

        public IDataResult TypeWorkCrList(IDomainService<TypeWorkCr> domainService, BaseParams baseParams)
        {
            var dpkrTypeWorkService = this.Container.Resolve<IDpkrTypeWorkService>();
            var objectCrDomain = this.Container.Resolve<IDomainService<ObjectCr>>();
            var controlDateDomain = this.Container.Resolve<IDomainService<ControlDate>>();
            var controlDateMunicipalityLimitDateDomain = this.Container.ResolveDomain<ControlDateMunicipalityLimitDate>();

            try
            {
                var loadParams = baseParams.GetLoadParam();
                var objectCrId = baseParams.Params.GetAs<long>("objectCrId");
                var twId = baseParams.Params.GetAsId("twId");
                var onlyServices = baseParams.Params.GetAs<bool>("onlyServices");
                var withoutSk = baseParams.Params.GetAs<bool>("withoutSk");

                if (objectCrId == 0)
                {
                    objectCrId = loadParams.Filter.GetAs("objectCrId", 0l);
                }

                if (twId == 0)
                {
                    twId = loadParams.Filter.GetAs("twId", 0l);
                }

                var programCrId = objectCrDomain
                    .GetAll()
                    .Where(x => x.Id == objectCrId)
                    .Select(x => x.ProgramCr.Id)
                    .FirstOrDefault();

                var federalLaw185 = baseParams.Params.GetAs("federalLaw185", false);

                var dictControlDate = controlDateDomain
                    .GetAll()
                    .Where(y => y.ProgramCr.Id == programCrId)
                    .Select(x => new {x.Work.Id, x.Date})
                    .ToDictionary(x => x.Id, x => x.Date);

                var dictMunicipalityControlLimitDate = controlDateMunicipalityLimitDateDomain
                    .GetAll()
                    .Where(x => x.ControlDate.ProgramCr.Id == programCrId)
                    .Select(x => new
                    {
                        WorkId = x.ControlDate.Work.Id,
                        x.LimitDate,
                        MunicipalityId = x.Municipality.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.WorkId)
                    .ToDictionary(x => x.Key,
                        x => x.ToDictionary(y => y.MunicipalityId, y => y.LimitDate));

                var stage1Dict = dpkrTypeWorkService.GetWorksByObjectCr(objectCrId);
                                               
                var data = domainService.GetAll()
                    .Where(x => x.ObjectCr.Id == objectCrId)
                    .WhereIf(twId > 0, x => x.Id == twId)
                    .WhereIf(federalLaw185, x => x.FinanceSource.TypeFinance == TypeFinance.FederalLaw)
                    .WhereIf(withoutSk, x=> !x.Work.IsAdditionalWork)
                    .WhereIf(onlyServices, x => x.Work.TypeWork == TypeWork.Service)
                    .Select(
                        x => new
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
                            x.YearRepair,
                            MunicipalityId = x.ObjectCr.RealityObject.Municipality.Id
                        })
                    .AsEnumerable()
                    .Select(
                        x => new
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
                            ControlDate = ControlDateUtils.GetControlDate(dictMunicipalityControlLimitDate, dictControlDate,
                                x.WorkId, x.MunicipalityId),
                            WorkFinSourceName = string.Format("{0} ({1})", x.WorkName, x.FinanceSourceName), //поле нужно для акта выполненных работ
                            DpkrSum = stage1Dict.ContainsKey(x.Id) ? stage1Dict[x.Id].Sum : 0M,
                            DpkrVolume = stage1Dict.ContainsKey(x.Id) ? stage1Dict[x.Id].Volume : 0M,
                            DpkrUnitMeasure = stage1Dict.ContainsKey(x.Id)
                                ? stage1Dict[x.Id].CalcBy != PriceCalculateBy.Volume
                                    ? stage1Dict[x.Id].CalcBy.GetEnumMeta().Display
                                    : stage1Dict[x.Id].UnitMeasure
                                : null
                        })
                    .AsQueryable()
                    .Filter(loadParams, this.Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(dpkrTypeWorkService);
                this.Container.Release(objectCrDomain);
                this.Container.Release(controlDateDomain);
                this.Container.Release(controlDateMunicipalityLimitDateDomain);
            }
        }

        public IDataResult TypeWorkStage1List (BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var typeWorkId = loadParams.Filter.GetAsId("typeWorkId"); 

            var typeWorkVersSt1Service = this.Container.ResolveDomain<TypeWorkCrVersionStage1>();
            try
            {
                var data = typeWorkVersSt1Service.GetAll()
                    .Where(x => x.TypeWorkCr.Id == typeWorkId)
                    .Select(x => new
                    {
                        x.Id,
                        StructuralElement = x.Stage1Version.StructuralElement.StructuralElement.Name,
                        x.Stage1Version.Volume,
                        x.Stage1Version.Sum,
                        Year = x.TypeWorkCr.YearRepair,
                        TypeWorkCr = x.TypeWorkCr.Id,
                        WorkName = x.TypeWorkCr.Work.Name

                    })
                    .Filter(loadParams, this.Container)
                    .Order(loadParams);

                var totalCount = data.Count();

                return new ListDataResult(data.ToList(), totalCount);
            }
            finally
            {
                this.Container.Release(typeWorkVersSt1Service);
            }
        }
    }
}