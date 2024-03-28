namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    using Castle.Windsor;
    using Gkh.Utils;

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
            this.Container = container;
            this.ObjectCrService = objectCrService;
            this.ProgramCrChangeService = programCrChangeService;
        }


        public IDataResult CalcInfo(BaseParams baseParams)
        {
            var date = baseParams.Params.Get("date", DateTime.Now);
            var id = baseParams.Params.GetAs<long>("id");
            var objectcr = baseParams.Params.GetAs<long>("objectcr");
            var volume = baseParams.Params.Get("volume", 0m);
            var workId = baseParams.Params.GetAs<long>("work");
            var costPerUnitVolume = baseParams.Params.Get("costPerUnitVolume", 0m);
            var sum = baseParams.Params.Get("sum", 0m);

            var config = Container.GetGkhConfig<OverhaulNsoConfig>();
            var serviceCost = config.ServiceCost;

            var objectCrService = Container.Resolve<IDomainService<ObjectCr>>();
            var workService = Container.Resolve<IDomainService<Work>>();
            var workPriceService = Container.Resolve<IDomainService<WorkPrice>>();
            var seJobService = Container.Resolve<IDomainService<StructuralElementWork>>();
            var typeWorkCrService = Container.Resolve<IDomainService<TypeWorkCr>>();
            var typeWorkVersSt1Service = Container.Resolve<ITypeWorkStage1Service>();
            var work = workService.Get(workId);
            var objectCr = objectCrService.Get(objectcr);
            
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

            var workPrice =
                workPriceService.GetAll().FirstOrDefault(x => x.Year == date.Year && jobIds.Any(y => y.Job.Id == x.Job.Id)
                                                                && x.Municipality.Id == objectCr.RealityObject.Municipality.Id);

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

            var data = Container.Resolve<IDomainService<DefectList>>().GetAll()
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

            var marginalCost = versStage1.CalcBy == PriceCalculateBy.Volume ? workPrice.NormativeCost :
                                                             workPrice.SquareMeterCost;

            // если сумма == 0 то рассчитываем Сумму, иначе расчитываем стоимость на единицу объема
            if (sum == 0)
            {
            if (marginalCost < costPerUnitVolume)
            {
                return new BaseDataResult(false, "Указанная стоимость на единицу превышает предельное значение!");
            }

                sum = (volume * costPerUnitVolume * (1 + serviceCost / 100)).RoundDecimal(2);

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

                costPerUnitVolume = (sum / (volume * (1 + serviceCost / 100))).RoundDecimal(2);

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

        public IDataResult CalcInfo(DefectList defectList)
        {
           return
                 CalcInfo(
                    new BaseParams
                        {
                            Params =
                                new DynamicDictionary
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
            var loadParams = baseParams.GetLoadParam();

            var typeWorkCrDomain = Container.Resolve<IDomainService<TypeWorkCr>>(); 
            var objCrId = baseParams.Params.GetAs<long>("objCrId");

            var data = Container.Resolve<IDomainService<Work>>().GetAll()
                .WhereIf(objCrId > 0, y => typeWorkCrDomain.GetAll().Any(x => x.ObjectCr.Id == objCrId && x.Work.Id == y.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Code,
                    x.ReformCode,
                    x.Description,
                    UnitMeasureName = x.UnitMeasure.Name,
                    x.Consistent185Fz,
                    x.IsPSD
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public IDataResult GetDefectListViewValue(BaseParams baseParams)
        {
            var objectCrId = baseParams.Params.GetAsId("objectCrId");

            var programCrChange = TypeChangeProgramCr.Manually;
            var objectCr = this.ObjectCrService.Get(objectCrId);
            if (objectCr.Return(x => x.ProgramCr).Return(x => x.Id) > 0)
            {
                programCrChange = this.ProgramCrChangeService
                    .GetAll()
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