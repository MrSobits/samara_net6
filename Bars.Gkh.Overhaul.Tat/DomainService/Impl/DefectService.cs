namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;

    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Enums;
    using Gkh.Domain;
    using Gkh.Entities.Dicts;
    using GkhCr.DomainService;
    using GkhCr.Entities;
    using GkhCr.Enums;
    using Overhaul.Entities;

    public class DefectService : IDefectService
    {
        protected IWindsorContainer Container;
        protected IDomainService<ObjectCr> ObjectCrService;
        protected IDomainService<ProgramCrChangeJournal> ProgramCrChangeService;

        public DefectService(
            IWindsorContainer container,
            IDomainService<ObjectCr> objectCrService,
            IDomainService<ProgramCrChangeJournal> programCrChangeService)
        {
            Container = container;
            ObjectCrService = objectCrService;
            ProgramCrChangeService = programCrChangeService;
        }

        public IDataResult CalcInfo(BaseParams baseParams)
        {
            var date = baseParams.Params.Get("date", DateTime.Now);
            var id = baseParams.Params.GetAs<long>("id", 0);
            var objectcr = baseParams.Params.GetAs<long>("objectcr", 0);
            var volume = baseParams.Params.Get("volume", 0m);
            var workId = baseParams.Params.GetAs<long>("work", 0);
            var costPerUnitVolume = baseParams.Params.Get("costPerUnitVolume", 0m);
            var sum = baseParams.Params.Get("sum", 0m);
            var config = Container.GetGkhConfig<OverhaulTatConfig>();
            var serviceCost = config.ServiceCost;

            var objectCrService = Container.Resolve<IDomainService<ObjectCr>>();
            var workService = Container.Resolve<IDomainService<Work>>();
            var workPriceService = Container.Resolve<IDomainService<WorkPrice>>();
            var seJobService = Container.Resolve<IDomainService<StructuralElementWork>>();
            var typeWorkCrService = Container.Resolve<IDomainService<TypeWorkCr>>();
            var defectListService = Container.Resolve<IDomainService<DefectList>>();
            var typeWorkVersSt1Service = Container.Resolve<ITypeWorkStage1Service>();

            var work = workService.Get(workId);
            var objectCr = objectCrService.Get(objectcr);

            try
            {
                if (objectCr == null || objectCr.RealityObject == null)
                {
                    return new BaseDataResult(false, "Невозможно определить дом!");
                }

                if (work == null)
                {
                    return new BaseDataResult(false, "Необходимо выбрать вид работ!");
                }

                var typeWorkCr = typeWorkCrService.GetAll().FirstOrDefault(x => x.Work.Id == workId && x.ObjectCr.Id == objectcr);

                if (typeWorkCr == null)
                {
                    return new BaseDataResult(false, "Невозможно определить вид работ!");
                }

                var versStage1 = typeWorkVersSt1Service.GetTypeWorkStage1(typeWorkCr);

                if (versStage1 == null)
                {
                    return new BaseDataResult(false, "Нет данных Долгосрочной программы по данному виду работ!");
                }

                var roSe = versStage1.Stage1Version.StructuralElement;

                var seQuery = seJobService.GetAll().Where(x => x.Job.Work.Id == workId);

                var jobIds = seQuery.Where(x => x.StructuralElement.Id == roSe.StructuralElement.Id);

                var muids = new List<long>
                {
                    objectCr.RealityObject.MoSettlement.Return(z => z.Id),
                    objectCr.RealityObject.Municipality.Id
                };

                var workPrice = workPriceService.GetAll()
                    .Where(x => x.Year == date.Year)
                    .Where(x => jobIds.Any(y => y.Job.Id == x.Job.Id))
                    .FirstOrDefault(x => muids.Contains(x.Municipality.Id));

                if (workPrice == null)
                {
                    return new BaseDataResult(false, string.Format("Не заданы расценки по выбранной работе за {0} год!", date.Year));
                }

                if (volume == 0)
                {
                    return new BaseDataResult(false, "Не задан объем!");
                }

                if (sum == 0 && costPerUnitVolume == 0)
                {
                    return new BaseDataResult(false, "Не задана стоимость на единицу объема по ведомости!");
                }

                var data = defectListService.GetAll()
                    .Where(x => x.ObjectCr.Id == objectcr && x.Id != id)
                    .Where(x => x.Work.Id == workId)
                    .Select(x => new
                    {
                        x.Volume,
                        x.Sum
                    })
                    .ToList();
                var volumeByWork = (data.Select(x => x.Volume).Sum() + volume).ToDecimal().RoundDecimal(2);
                var sumByWork = (data.Select(x => x.Sum).Sum() + sum).ToDecimal().RoundDecimal(2);

                if (versStage1.Volume < volumeByWork)
                {
                    return new BaseDataResult(false, string.Format("Объем ведомостей по данному виду работ превышает предельное значение! Объем: {0}", volumeByWork));
                }

                var marginalCost = versStage1.CalcBy == PriceCalculateBy.Volume 
                    ? workPrice.NormativeCost 
                    : workPrice.SquareMeterCost;

                // если сумма == 0 то рассчитываем Сумму, иначе расчитываем стоимость на единицу объема
                if (sum == 0)
                {
                    if (marginalCost < costPerUnitVolume)
                    {
                        return new BaseDataResult(false, "Указанная стоимость на единицу превышает предельное значение!");
                    }

                    sum = (volume * costPerUnitVolume * (1 + (serviceCost / 100))).RoundDecimal(2);

                    if (versStage1.Sum < sumByWork)
                    {
                        return new BaseDataResult(false, string.Format("Cумма ведомостей по данному виду работ превышает предельное значение! Сумма: {0}", sumByWork));
                    }
                }
                else
                {
                    if (versStage1.Sum < sumByWork)
                    {
                        return new BaseDataResult(false, string.Format("Cумма ведомостей по данному виду работ превышает предельное значение! Сумма: {0}", sumByWork));
                    }

                    costPerUnitVolume = (sum / (volume * (1 + (serviceCost / 100)))).RoundDecimal(2);

                    if (marginalCost < costPerUnitVolume)
                    {
                        return new BaseDataResult(false, "Рассчитанная стоимость на единицу превышает предельное значение!");
                    }
                }

                return new BaseDataResult(new DefectList
                {
                    Sum = sum,
                    CostPerUnitVolume = costPerUnitVolume
                });
            }
            finally
            {
                Container.Release(objectCrService);
                Container.Release(workService);
                Container.Release(workPriceService);
                Container.Release(seJobService);
                Container.Release(typeWorkCrService);
                Container.Release(defectListService);
                Container.Release(typeWorkVersSt1Service);
            }
        }

        public IDataResult CalcInfo(DefectList defectList)
        {
            return CalcInfo(new BaseParams
                {
                    Params = new DynamicDictionary
                        {
                            { "date", defectList.DocumentDate },
                            { "objectcr", defectList.ObjectCr.Id },
                            { "volume", defectList.Volume },
                            { "work", defectList.Work.Id },
                            { "costPerUnitVolume", defectList.CostPerUnitVolume},
                            { "sum", defectList.Sum},
                            { "id", defectList.Id}
                        }
                });
        }

        public IDataResult WorksForDefectList(BaseParams baseParams)
        {
            var typeWorkCrDomain = Container.Resolve<IDomainService<TypeWorkCr>>();
            var workDomain = Container.Resolve<IDomainService<Work>>();
            var loadParams = baseParams.GetLoadParam();

            try
            {
                var objCrId = baseParams.Params.GetAs<int>("objCrId");
                var ids = baseParams.Params.GetAs<string>("ids").ToLongArray();

                return workDomain.GetAll()
                    .WhereIf(objCrId > 0, y => typeWorkCrDomain.GetAll().Any(x => x.ObjectCr.Id == objCrId && x.Work.Id == y.Id))
                    .WhereIf(ids.Length > 0, y => ids.Contains(y.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.Code,
                        x.ReformCode,
                        x.GisGkhCode,
                        x.GisGkhGuid,
                        x.Description,
                        UnitMeasureName = x.UnitMeasure.Name,
                        x.Consistent185Fz,
                        x.IsPSD,
                        x.GisCode,
                        x.IsActual,
                    })
                    .ToListDataResult(loadParams);
            }
            finally
            {
                Container.Release(typeWorkCrDomain);
                Container.Release(workDomain);
            }
        }

        public IDataResult GetDefectListViewValue(BaseParams baseParams)
        {
            var objectCrId = baseParams.Params.GetAsId("objectCrId");

            var programCrChange = TypeChangeProgramCr.Manually;
            var objectCr = ObjectCrService.Get(objectCrId);
            
            if (objectCr.Return(x => x.ProgramCr).Return(x => x.Id) > 0)
            {
                programCrChange = ProgramCrChangeService.GetAll()
                    .Where(x => x.ProgramCr.Id == objectCr.ProgramCr.Id)
                    .OrderByDescending(x => x.ChangeDate)
                    .FirstOrDefault().Return(x => x.TypeChange);
            }

            if (programCrChange == TypeChangeProgramCr.Manually)
            {
                return new BaseDataResult();
            }

            return new BaseDataResult(Container.GetGkhConfig<GkhCrConfig>().DpkrConfig.TypeDefectListView);
        }
    }
}