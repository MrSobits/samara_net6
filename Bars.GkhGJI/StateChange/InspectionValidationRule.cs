namespace Bars.GkhGji.StateChange
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    public class InspectionValidationRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "gji_inspection_validation_rule"; }
        }

        public string Name
        {
            get { return "Проверка заполненности инспекционных проверок"; }
        }

        public string TypeId
        {
            get { return "gji_inspection"; }
        }

        public string Description
        {
            get { return "Данное правило проверяет заполненность необходимых полей инспекционных проверок"; }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is InspectionGji)
            {
                var inspectionViolService = Container.Resolve<IDomainService<InspectionGjiViol>>();
                var servActCheckRO = Container.Resolve<IDomainService<ActCheckRealityObject>>();
                var servInspectionRO = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

                var inspection = statefulEntity as InspectionGji;

                try
                {
                    if (newState.FinalState)
                    {
                        var realObjsIds = servActCheckRO.GetAll()
                                          .Where(
                                              x =>
                                              x.ActCheck.Inspection.Id == inspection.Id
                                              && x.HaveViolation == YesNoNotSet.No)
                                          .Where(x => x.RealityObject != null)
                                          .Select(x => x.RealityObject.Id)
                                          .ToList();

                        realObjsIds.AddRange(inspectionViolService.GetAll()
                                                 .Where(x => x.Inspection.Id == inspection.Id && x.DateFactRemoval.HasValue)
                                                 .Where(x => x.RealityObject != null)
                                                 .Select(x => x.RealityObject.Id)
                                                 .ToList());

                        var violationWithoutDate = inspectionViolService.GetAll()
                                                 .Where(x =>
                                                     x.Inspection.Id == inspection.Id && !x.DateFactRemoval.HasValue);

                        var realObjWithoutViolation = servInspectionRO.GetAll()
                                            .Where(
                                                x =>
                                                x.Inspection.Id == inspection.Id
                                                && !realObjsIds.Contains(x.RealityObject.Id));

                        if (violationWithoutDate.Any() || realObjWithoutViolation.Any())
                        {
                            return
                                ValidateResult.No(" Не указаны результаты проверки, либо не каждое нарушение имеет дату фактического исполнения.");
                        }
                    }

                }
                catch (Exception exc)
                {
                    throw exc;
                }
                finally
                {
                    Container.Release(inspectionViolService);
                    Container.Release(servActCheckRO);
                    Container.Release(servInspectionRO);
                }
            }

            return ValidateResult.Yes();
        }
    }
}