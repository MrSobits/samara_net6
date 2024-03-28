using System;
using System.Linq;
using Bars.B4;
using Bars.B4.Modules.States;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.Gkh.Enums;
using Bars.GkhGji.Entities;
using Castle.Windsor;

namespace Bars.GkhGji.Regions.Nnovgorod.StateChange
{
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
                var inspectionViolService = Container.Resolve<IDomainService<InspectionGjiViolStage>>();
                var servActCheck = Container.Resolve<IDomainService<ActCheck>>();
                var servActCheckRo = Container.Resolve<IDomainService<ActCheckRealityObject>>();
                var servInspectionRo = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

                var inspection = statefulEntity as InspectionGji;

                try
                {
                    if (newState.FinalState)
                    {
                        var actChecks =
                            servActCheck.GetAll().Where(x => x.Inspection.Id == inspection.Id).ToList();

                        if (actChecks.Count > 0 && actChecks.All(x => x.ActToPres))
                        {
                            return ValidateResult.Yes();
                        }

                        if (inspection.Contragent != null &&
                            inspection.Contragent.ContragentState == ContragentState.Liquidated)
                        {

                            var violationDates = inspectionViolService.GetAll()
                                                     .Where(x => x.InspectionViolation.Inspection.Id == inspection.Id && x.DateFactRemoval.HasValue)
                                                     .Select(x => x.DatePlanRemoval)
                                                     .ToList();

                            if (violationDates.Any() &&
                                violationDates.Select(x => x.ToDateTime()).SafeMax(x => x) <=
                                inspection.Contragent.DateTermination.ToDateTime())
                            {
                                return ValidateResult.No("Для успешного изменения статуса необходимо чтобы контрагент основания проверки был ликвидирован и его дата прекращения деятельности была меньше самой поздней даты устранения нарушения.");
                            }
                        }


                        var realObjsIds = servActCheckRo.GetAll()
                                          .Where(
                                              x =>
                                              x.ActCheck.Inspection.Id == inspection.Id
                                              && x.HaveViolation == YesNoNotSet.No)
                                          .Select(x => x.RealityObject.Id)
                                          .ToList();

                        realObjsIds.AddRange(inspectionViolService.GetAll()
                                                 .Where(x => x.InspectionViolation.Inspection.Id == inspection.Id && x.DateFactRemoval.HasValue)
                                                 .Select(x => x.InspectionViolation.RealityObject.Id)
                                                 .ToList());

                        var violationWithoutDate = inspectionViolService.GetAll()
                                                 .Where(x =>
                                                     x.InspectionViolation.Inspection.Id == inspection.Id && !x.DateFactRemoval.HasValue);

                        var realObjWithoutViolation = servInspectionRo.GetAll()
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
                    Container.Release(servActCheckRo);
                    Container.Release(servInspectionRo);
                    Container.Release(servActCheck);
                }
            }

            return ValidateResult.Yes();
        }
    }
}