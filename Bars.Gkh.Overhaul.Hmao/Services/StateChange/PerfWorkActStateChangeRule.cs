namespace Bars.Gkh.Overhaul.Hmao.StateChange
{
    using System;
    using System.Linq;
    using B4;
    using B4.Modules.States;
    using Bars.Gkh.Enums;
    using Castle.Windsor;
    using DomainService;
    using Gkh.Overhaul.Entities;
    using GkhCr.Entities;

    /// <summary>
    /// Правило перехода статуса акта выполненных работ
    /// </summary>
    public class PerfWorkActStateChangeRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public string Id { get { return "cr_perf_work_act_update_se_rule"; } }

        public string Name { get { return "Обновление данных конструктивного элемента в паспорте жилого дома"; } }

        public string TypeId { get { return "cr_obj_performed_work_act"; } }

        public string Description
        {
            get
            {
                return @"Данное правило обновляет поля конструктивного элемента 
                            ('Износ', 'Год установки или последнего кап.ремонта') в паспорте жилого дома при смене статуса акта выполненных работ";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is PerformedWorkAct)
            {
                var performedWorkAct = statefulEntity as PerformedWorkAct;

                var typeWorkVersSt1Service = this.Container.Resolve<ITypeWorkStage1Service>();
                var roSeService = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>();

                if (performedWorkAct.TypeWorkCr != null)
                {
                    var versStage1 = typeWorkVersSt1Service.GetTypeWorkStage1(performedWorkAct.TypeWorkCr);

                   // if (versStage1 == null)
                      if (1 == 1)
                      {
                        //ищем конструктив по работе
                        var strElWorksDomain = this.Container.Resolve<IDomainService<StructuralElementWork>>();
                        var elementlist = strElWorksDomain.GetAll()
                            .Where(x=> x.Job.Work == performedWorkAct.TypeWorkCr.Work).Select(x=> x.StructuralElement.Id).Distinct().ToList();
                        if (elementlist.Count > 0)
                        {
                            var roSeByWork = roSeService.GetAll().FirstOrDefault(x => x.RealityObject != null 
                            && x.RealityObject.Id == performedWorkAct.ObjectCr.RealityObject.Id
                            && elementlist.Contains(x.StructuralElement.Id) && x.State.StartState);
                            if (roSeByWork != null)
                            {
                                roSeByWork.LastOverhaulYear = performedWorkAct.DateFrom.HasValue
                                             ? performedWorkAct.DateFrom.Value.Year
                                             : DateTime.Now.Year;

                                if (performedWorkAct.FactVolume.HasValue && performedWorkAct.FactVolume != 0 && roSeByWork.Volume < performedWorkAct.FactVolume.Value)
                                    roSeByWork.Volume = performedWorkAct.FactVolume.Value;

                                roSeByWork.Wearout = 0;

                                roSeService.Update(roSeByWork);
                                return new ValidateResult { Success = true };
                            }

                        }

                        return ValidateResult.No(@"Невозможно актуализировать сведения о конструктивных характеристиках,
                                                т.к. данный вид работы отсутствует в  версии Долгосрочной программы капитального ремонта");
                    }

                    var roSe = versStage1.Stage1Version.StructuralElement;
                    roSe.LastOverhaulYear = performedWorkAct.DateFrom.HasValue
                                                ? performedWorkAct.DateFrom.Value.Year
                                                : DateTime.Now.Year;

                    if (performedWorkAct.FactVolume.HasValue && performedWorkAct.FactVolume != 0 && roSe.Volume < performedWorkAct.FactVolume.Value)
                        roSe.Volume = performedWorkAct.FactVolume.Value;

                    roSe.Wearout = 0;

                    roSeService.Update(roSe);
                }
            }

            return new ValidateResult { Success = true };
        }
    }
}