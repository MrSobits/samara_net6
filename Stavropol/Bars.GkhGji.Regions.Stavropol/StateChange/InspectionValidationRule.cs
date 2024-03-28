namespace Bars.GkhGji.Regions.Stavropol.StateChange
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;

    public class StavropolInspectionValidationRule : Bars.GkhGji.StateChange.InspectionValidationRule
    {
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            /*
             Если в акте проверки в поле Передано в прокуратуру стоит "Да" и указана Дата в поле "Дата передачи"
             то такие акты при переводе статуса не рассматривать
             */
            if (statefulEntity is InspectionGji)
            {
                var inspectionViolService = Container.Resolve<IDomainService<InspectionGjiViol>>();
                var servActCheckRO = Container.Resolve<IDomainService<ActCheckRealityObject>>();
                var servActCheckViol = Container.Resolve<IDomainService<ActCheckViolation>>();
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
                                          .Select(x => x.RealityObject.Id)
                                          .ToList();

                        // Вообщем нам не нужны акты у которых в Результатах проверки = Да и выставлена ДатаПередачи
                        var inspectionQuery =
                            inspectionViolService.GetAll()
                                                 .Where(
                                                     x =>
                                                     x.Inspection.Id == inspection.Id && !x.DateFactRemoval.HasValue)
                                                 .Where(
                                                     x =>
                                                     !servActCheckViol.GetAll()
                                                                      .Where(y => y.InspectionViolation.Id == x.Id)
                                                                      .Any(
                                                                          y =>
                                                                          y.ActObject.HaveViolation == YesNoNotSet.Yes
                                                                          && y.ActObject.ActCheck.ToProsecutor == YesNoNotSet.Yes
                                                                          && y.ActObject.ActCheck.DateToProsecutor
                                                                              .HasValue));

                        realObjsIds.AddRange(inspectionQuery
                                                 .Select(x => x.RealityObject.Id)
                                                 .ToList());

                        var realObjWithoutViolation = servInspectionRO.GetAll()
                                            .Where(
                                                x =>
                                                x.Inspection.Id == inspection.Id
                                                && !realObjsIds.Contains(x.RealityObject.Id));

                        if (inspectionQuery.Any() || realObjWithoutViolation.Any())
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
                    Container.Release(servActCheckViol);
                }
            }

            return ValidateResult.Yes();
        }
    }
}