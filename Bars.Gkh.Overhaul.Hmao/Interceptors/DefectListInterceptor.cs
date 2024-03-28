namespace Bars.Gkh.Overhaul.Hmao.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService.GkhParam;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    public class DefectListInterceptor : EmptyDomainInterceptor<DefectList>
    {
        public override IDataResult BeforeUpdateAction(IDomainService<DefectList> service, DefectList entity)
        {
            var typeWorkVersSt1Service = Container.Resolve<ITypeWorkStage1Service>();
            var defectService = Container.Resolve<IDefectService>();

            using (Container.Using(typeWorkVersSt1Service, defectService))
            {
                var versionSt1 = typeWorkVersSt1Service.GetTypeWorkStage1(entity.Work, entity.ObjectCr);

                if (versionSt1 == null ||
                    entity.TypeDefectList == TypeDefectList.Customer)
                {
                    return Success();
                }

                if ((entity.CostPerUnitVolume.HasValue || entity.Sum.HasValue) &&
                    entity.Volume.HasValue &&
                    versionSt1.CalcBy == PriceCalculateBy.Volume)
                {
                    var res = defectService.CalcInfo(entity);
                    if (res.Success)
                    {
                        var defList = (DefectList)res.Data;
                        entity.CostPerUnitVolume = defList.CostPerUnitVolume;
                        entity.Sum = defList.Sum;
                    }

                    return res;
                }

                var defListSum =
                    service.GetAll()
                        .Where(x => x.ObjectCr.Id == entity.ObjectCr.Id)
                        .Where(x => x.Work.Id == entity.Work.Id)
                        .Where(x => x.Id != entity.Id)
                        .Where(x => x.State != null && x.State.FinalState)
                        .Sum(x => x.Sum);

                return versionSt1.Sum < defListSum.ToDecimal() + entity.Sum.ToDecimal()
                    ? Failure("Указанная сумма превышает предельное значение!")
                    : Success();
            }
        }

        public override IDataResult BeforeDeleteAction(IDomainService<DefectList> service, DefectList entity)
        {
            // обновление если ведомость не в финальном состоянии не требуется.
            if (!entity.State.FinalState)
            {
                return Success();
            }

            // игнорим проверки для ведомости заказчика
            if (entity.TypeDefectList == TypeDefectList.Customer)
            {
                return Success();
            }

            var typeWorkCrService = Container.Resolve<IDomainService<TypeWorkCr>>();
            var typeWorkVersSt1Service = Container.Resolve<ITypeWorkStage1Service>();

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