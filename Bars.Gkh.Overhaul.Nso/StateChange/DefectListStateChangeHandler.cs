namespace Bars.Gkh.Overhaul.Nso.StateChange
{
    using System;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.States;

    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using DomainService;
    using Gkh.Enums;
    using GkhCr.Entities;
    using GkhCr.Enums;

    /// <summary>
    /// Обработчик события смены статуса дефектной ведомости
    /// </summary>
    public class DefectListStateChangeHandler : IStateChangeHandler
    {
        public IWindsorContainer Container { get; set; }

        public void OnStateChange(IStatefulEntity entity, State oldState, State newState)
        {
            if (entity.State.TypeId != "cr_obj_defect_list")
            {
                return;
            }

            var defectList = entity as DefectList;
            if (defectList == null)
            {
                throw new InvalidCastException("Не удалось привести к типу DefectList");
            }
            var defectListService = Container.ResolveDomain<DefectList>();
            var typeWorkCrService = Container.ResolveDomain<TypeWorkCr>();
            var typeWorkVersSt1Service = Container.Resolve<ITypeWorkStage1Service>();
            try
            {
                var typeCheckWork = Container.GetGkhConfig<GkhCrConfig>().DpkrConfig.TypeCheckWork;
                if (typeCheckWork != TypeCheckWork.WithDefectList || (defectList.TypeDefectList == TypeDefectList.Customer))
                {
                    return;
                }

                var typeWorkCr =
                    typeWorkCrService.GetAll()
                        .FirstOrDefault(
                            x => x.Work.Id == defectList.Work.Id && x.ObjectCr.Id == defectList.ObjectCr.Id);
                var versStage1 = typeWorkVersSt1Service.GetTypeWorkStage1(typeWorkCr);
                var isCalcByVolume = versStage1 != null && versStage1.CalcBy == PriceCalculateBy.Volume;

                if (typeWorkCr == null)
                {
                    return;
                }

                if (newState.FinalState)
                {
                    var defectLists = defectListService.GetAll()
                        .Where(x => x.ObjectCr.Id == defectList.ObjectCr.Id)
                        .Where(x => x.Work.Id == defectList.Work.Id)
                        .Where(x => x.TypeDefectList != TypeDefectList.Customer)
                        .Where(x => x.State != null && x.State.FinalState)
                        .Select(x => new {x.Volume, x.Sum})
                        .ToArray();

                    typeWorkCr.Volume = isCalcByVolume ? defectLists.Sum(x => x.Volume) + defectList.Volume : typeWorkCr.Volume;
                    typeWorkCr.Sum = defectLists.Sum(x => x.Sum) + defectList.Sum;

                }

                if (oldState.FinalState)
                {
                    typeWorkCr.Volume -= isCalcByVolume ? defectList.Volume : 0;
                    typeWorkCr.Sum -= defectList.Sum;
                }
            }
            finally
            {
                Container.Release(defectListService);
                Container.Release(typeWorkCrService);
                Container.Release(typeWorkVersSt1Service);
            }
        }
    }
}