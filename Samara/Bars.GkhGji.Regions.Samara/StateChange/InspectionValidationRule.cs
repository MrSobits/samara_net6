using System;
using System.Linq;
using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.States;
using Bars.Gkh.Enums;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Castle.Windsor;

namespace Bars.GkhGji.Regions.Samara.StateChange
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
                var inspectionViolService = Container.ResolveDomain<InspectionGjiViol>();
                var servActCheckRO = Container.ResolveDomain<ActCheckRealityObject>();
                var servInspectionRO = Container.ResolveDomain<InspectionGjiRealityObject>();
                var servDocumentGjiChildren = Container.ResolveDomain<DocumentGjiChildren>();
                var servActCheckViolation = Container.ResolveDomain<ActCheckViolation>();
                var inspection = statefulEntity as InspectionGji;

                try
                {
                    if (newState.FinalState)
                    {
                        var realObjsIds = servActCheckRO.GetAll()
                                          .Where(
                                              x =>
                                              x.ActCheck.Inspection.Id == inspection.Id)
                                          .Select(x => x.RealityObject.Id)
                                          .Distinct()
                                          .ToList();


                        //Если в проверке создан протокол из акта проверки и нет предписания, то не должна  проверятся следующая проверка
                        var actWithProtocolChildren = servDocumentGjiChildren.GetAll()
                            .Where(x => x.Parent.Inspection.Id == inspection.Id && x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                            .Select(x => new { x.Parent.Id, x.Children.TypeDocumentGji })
                            .AsEnumerable()
                            .GroupBy(x => x.Id)
                            .Where(y =>
                                    y.Any(x => x.TypeDocumentGji == TypeDocumentGji.Protocol) &&
                                    y.All(x => x.TypeDocumentGji != TypeDocumentGji.Prescription))
                            .Select(x => x.Key)
                            .ToList();

                        var actViolWithProtocolChildren = servActCheckViolation.GetAll()
                            .Where(x => actWithProtocolChildren.Contains(x.ActObject.ActCheck.Id));
                                                            

                        var violationWithoutDate = inspectionViolService.GetAll()
                                                 .Where(x =>
                                                     x.Inspection.Id == inspection.Id && !x.DateFactRemoval.HasValue)
                                                     .Where(x => !actViolWithProtocolChildren.Any(y => y.InspectionViolation.Id ==x.Id));

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
                    Container.Release(servDocumentGjiChildren);
                }
            }

            return ValidateResult.Yes();
        }
    }
}