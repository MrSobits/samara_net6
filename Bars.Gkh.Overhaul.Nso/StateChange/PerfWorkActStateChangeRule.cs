namespace Bars.Gkh.Overhaul.Nso.StateChange
{
    using System;
    using B4;
    using B4.Modules.States;
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
                            ('Износ', 'Год установки или последнего кап.ремонта') в паспорте жилого дома. 
                        В случае перевода на 'Утверждено' произходит установка полей новыми значениями, 
                            а вслучае перевода на 'Черновик' произходит возврат на исходные значения";
            }
        }

        /// <summary>
        /// Если переводят на Утверждено, то КЭ в Доме присваиваем новые ГодПоследнегоКР и обнуляем Износ
        /// Если переводят на начальный, то тогда из ДПКР возвращаем Год последнего КР который был при расчете и Износ
        /// </summary>
        /// <param name="statefulEntity"></param>
        /// <param name="oldState"></param>
        /// <param name="newState"></param>
        /// <returns></returns>
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var typeWorkVersSt1Service = this.Container.Resolve<ITypeWorkStage1Service>();
            var roSeService = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>();

            try
            {
                if (statefulEntity is PerformedWorkAct)
                {
                    var performedWorkAct = statefulEntity as PerformedWorkAct;

                    if (performedWorkAct.TypeWorkCr != null)
                    {
                        if (newState.Name == "Утверждено")
                        {
                            var versStage1 = typeWorkVersSt1Service.GetTypeWorkStage1(performedWorkAct.TypeWorkCr);
                            var roSe = versStage1.Stage1Version.StructuralElement;

                            roSe.LastOverhaulYear = performedWorkAct.DateFrom.HasValue
                                                    ? performedWorkAct.DateFrom.Value.Year
                                                    : DateTime.Now.Year;
                            roSe.Wearout = 0;
                            if (performedWorkAct.FactVolume.HasValue && performedWorkAct.FactVolume > 0)
                            {
                                roSe.Volume = performedWorkAct.FactVolume.Value;
                            }

                            roSeService.Update(roSe);
                        }
                        else if (newState.StartState && oldState != null)
                        {
                            var versStage1 = typeWorkVersSt1Service.GetTypeWorkStage1(performedWorkAct.TypeWorkCr);
                            var roSe = versStage1.Stage1Version.StructuralElement;

                            if (versStage1.Stage1Version.LastOverhaulYear > 0 || versStage1.Stage1Version.Wearout > 0)
                            {
                                roSe.LastOverhaulYear = versStage1.Stage1Version.LastOverhaulYear;
                                roSe.Wearout = versStage1.Stage1Version.Wearout;

                                roSeService.Update(roSe);
                            }

                        }
                    }
                }
            }
            finally
            {
                Container.Release(typeWorkVersSt1Service);
                Container.Release(roSeService);
            }

            return new ValidateResult { Success = true };
        }
    }
}