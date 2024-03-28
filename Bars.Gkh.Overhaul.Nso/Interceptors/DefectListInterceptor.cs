namespace Bars.Gkh.Overhaul.Nso.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Nso.DomainService;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    public class DefectListInterceptor : EmptyDomainInterceptor<DefectList>
    {
        public override IDataResult BeforeCreateAction(IDomainService<DefectList> service, DefectList entity)
        {
            var seJobService = Container.Resolve<IDomainService<StructuralElementWork>>();
            var workPriceService = Container.Resolve<IDomainService<WorkPrice>>();
            var typeWorkVersSt1Service = Container.Resolve<ITypeWorkStage1Service>();
            var config = Container.GetGkhConfig<GkhCrConfig>();
            try
            {
                var typeCheckWork = config.DpkrConfig.TypeCheckWork;
                var formFinanceSource = config.DpkrConfig.TypeDefectListView;

                if (typeCheckWork != TypeCheckWork.WithDefectList || formFinanceSource == TypeDefectListView.WithoutOverhaulData || entity.TypeDefectList == TypeDefectList.Customer)
                {
                    return Success();
                }

                var versStage1 = typeWorkVersSt1Service.GetTypeWorkStage1(entity.Work, entity.ObjectCr);

                // стоимость на единицу объема = предельной стоимости при первом сохранении
                if (versStage1 == null)
                {
                    return Success();
                }

                var seQuery = seJobService.GetAll().Where(x => x.Job.Work.Id == entity.Work.Id);

                var jobQuery =
                    seQuery.Where(
                        x =>
                            x.StructuralElement.Id ==
                            versStage1.Stage1Version.StructuralElement.StructuralElement.Id);

                var workPrice =
                    workPriceService.GetAll().FirstOrDefault(x => x.Year
                                                                  == (entity.DocumentDate.HasValue
                                                                      ? entity.DocumentDate.Value.Year
                                                                      : DateTime.Now.Year) &&
                                                                  jobQuery.Any(y => y.Job.Id == x.Job.Id)
                                                                  &&
                                                                  x.Municipality.Id ==
                                                                  entity.ObjectCr.RealityObject.Municipality.Id);

                entity.CostPerUnitVolume = workPrice != null
                    ? versStage1.CalcBy == PriceCalculateBy.Volume
                        ? workPrice.NormativeCost
                        : workPrice.SquareMeterCost
                    : 0M;

                if (service.GetAll()
                    .Any(x => x.ObjectCr.Id == entity.ObjectCr.Id &&
                        x.Work.Id == entity.Work.Id &&
                        (!x.TypeDefectList.HasValue || x.TypeDefectList != TypeDefectList.Customer)))
                {
                    return Success();
                }

                entity.Volume = versStage1.Volume;
                entity.Sum = versStage1.Sum;

                return Success();
            }
            finally
            {
                Container.Release(seJobService);
                Container.Release(workPriceService);
                Container.Release(typeWorkVersSt1Service);
            }
        }

        public override IDataResult BeforeUpdateAction(IDomainService<DefectList> service, DefectList entity)
        {
            if (entity.State == null || !entity.State.FinalState)
            {
                return Success();
            }

            var config = Container.GetGkhConfig<GkhCrConfig>();
            var useTypeDefectList = config.General.DefectListUsage == DefectListUsage.Use;

            if (!useTypeDefectList)
            {
                return Success();
            }

            var typeWorkVersSt1Service = Container.Resolve<ITypeWorkStage1Service>();

            var versionSt1 = typeWorkVersSt1Service.GetTypeWorkStage1(entity.Work, entity.ObjectCr);

            if (versionSt1 == null)
            {
                return Success();
            }

            if ((entity.CostPerUnitVolume.HasValue || entity.Sum.HasValue) && entity.Volume.HasValue && versionSt1.CalcBy == PriceCalculateBy.Volume)
            {
                var res = Container.Resolve<IDefectService>().CalcInfo(entity);
                if (res.Success)
                {
                    var defList = (DefectList)res.Data;
                    entity.CostPerUnitVolume = defList.CostPerUnitVolume;
                    entity.Sum = defList.Sum;
                }

                return res;
            }

            var defectLists = service.GetAll()
                    .Where(x => !x.TypeDefectList.HasValue || x.TypeDefectList != TypeDefectList.Customer)
                    .Where(x => x.ObjectCr.Id == entity.ObjectCr.Id)
                    .Where(x => x.Work.Id == entity.Work.Id)
                    .Where(x => x.Id != entity.Id)
                    .Where(x => x.State != null && x.State.FinalState)
                    .Select(x => new { x.Sum, x.Volume })
                    .ToArray();
            var defListSum = defectLists.Sum(x => x.Sum).ToDecimal();

            if (!entity.TypeDefectList.HasValue || entity.TypeDefectList != TypeDefectList.Customer)
            {
                defListSum += entity.Sum.ToDecimal();
                if (versionSt1.Sum < defListSum)
                {
                    return Failure("Указанная сумма превышает предельное значение!");
                }
            }

            var typeWorkCrService = Container.ResolveDomain<TypeWorkCr>();
            var typeWorkCr =
                    typeWorkCrService.GetAll()
                        .FirstOrDefault(
                            x => x.Work.Id == entity.Work.Id && x.ObjectCr.Id == entity.ObjectCr.Id);
            if (typeWorkCr == null)
            {
                return Success();
            }

            var isCalcByVolume = versionSt1.CalcBy == PriceCalculateBy.Volume;
            typeWorkCr.Volume = isCalcByVolume ? defectLists.Sum(x => x.Volume) : typeWorkCr.Volume;
            typeWorkCr.Sum = defListSum;
            typeWorkCrService.Update(typeWorkCr);
            return Success();
        }

        public override IDataResult BeforeDeleteAction(IDomainService<DefectList> service, DefectList entity)
        {
            if (!entity.State.FinalState || entity.TypeDefectList == TypeDefectList.Customer)
            {
                return Success();
            }

            var typeWorkCrService = Container.ResolveDomain<TypeWorkCr>();
            var typeWorkVersSt1Service = Container.Resolve<ITypeWorkStage1Service>();

            using (Container.Using(typeWorkCrService, typeWorkVersSt1Service))
            {
                var typeWorkCr = typeWorkCrService.GetAll().FirstOrDefault(x => x.Work.Id == entity.Work.Id && x.ObjectCr.Id == entity.ObjectCr.Id);
                var versStage1 = typeWorkVersSt1Service.GetTypeWorkStage1(typeWorkCr);
                var isCalcByVolume = versStage1 != null && versStage1.CalcBy == PriceCalculateBy.Volume;

                if (typeWorkCr == null)
                {
                    return Success();
                }

                typeWorkCr.Volume -= isCalcByVolume ? entity.Volume : 0;
                typeWorkCr.Sum -= entity.Sum;

                return Success();
            }
        }
    }
}