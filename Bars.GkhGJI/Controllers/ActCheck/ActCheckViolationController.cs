namespace Bars.GkhGji.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.DataAccess;
    using Entities;
    using Enums;
    using Gkh.Entities;

    public class ActCheckViolationController : ActCheckViolationController<ActCheckViolation>
    {
    }

    public class ActCheckViolationController<T> : B4.Alt.DataController<T>
    where T : ActCheckViolation
    {
        public override ActionResult Create(BaseParams baseParams)
        {
            var actViolation = baseParams.Params.GetAs<List<ActCheckViolationProxy>>("records");
            if (actViolation == null)
            {
                return JsFailure("При сохранении нарушений произошла ошибка");
            }

            var serviceActCheckRealityObject = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var serviceActCheckViol = Container.Resolve<IDomainService<ActCheckViolation>>();
            var serviceInspectionViol = Container.Resolve<IDomainService<InspectionGjiViol>>();
            var serviceInspection = Container.Resolve<IDomainService<InspectionGji>>();
            var serviceRobject = Container.Resolve<IDomainService<RealityObject>>();
            var serviceViolation = Container.Resolve<IDomainService<ViolationGji>>();

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var actViolationProxyItem in actViolation)
                    {
                        var actCheckGjiRealityObject =
                            serviceActCheckRealityObject.Get(Convert.ToInt32(actViolationProxyItem.ActObject));
                        if (actCheckGjiRealityObject == null)
                        {
                            return JsFailure("Не найден дом акта проверки");
                        }

                        if (actViolationProxyItem.Id == 0)
                        {
                            var newViol = new InspectionGjiViol
                            {
                                Inspection = serviceInspection.Load(actCheckGjiRealityObject.ActCheck.Inspection.Id),
                                RealityObject = serviceRobject.Load(actCheckGjiRealityObject.RealityObject.Id),
                                Violation = serviceViolation.Load(actViolationProxyItem.ViolationGjiId),
                                DatePlanRemoval = actViolationProxyItem.DatePlanRemoval
                            };

                            serviceInspectionViol.Save(newViol);

                            //Формируем список нарушений и ставим тип TypeViolationStage.Detection (Выявление)
                            var newObj = new ActCheckViolation
                            {
                                TypeViolationStage = TypeViolationStage.Detection,
                                InspectionViolation = newViol,
                                ActObject = actCheckGjiRealityObject,
                                Document = actCheckGjiRealityObject.ActCheck,
                                DatePlanRemoval = actViolationProxyItem.DatePlanRemoval
                            };

                            serviceActCheckViol.Save(newObj);
                        }
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    return JsonNetResult.Failure("При сохранении изменений произошла ошибка");
                }
                finally
                {
                    Container.Release(serviceActCheckViol);
                    Container.Release(serviceInspection);
                    Container.Release(serviceRobject);
                    Container.Release(serviceViolation);
                }
            }

            return JsSuccess();
        }

        public override ActionResult Delete(BaseParams baseParams)
        {
            var actViolationIds = baseParams.Params.GetAs<List<long>>("records");
            if (actViolationIds.Count == 0)
            {
                return JsSuccess();
            }

            var serviceActCheckViol = Container.ResolveDomain<ActCheckViolation>();
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var actViolationId in actViolationIds)
                    {
                        var actCheckViolation = serviceActCheckViol.GetAll().FirstOrDefault(x => x.Id == actViolationId);
                        if (actCheckViolation == null)
                        {
                            return JsSuccess();
                        }

                        serviceActCheckViol.Delete(actViolationId);
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    return JsonNetResult.Failure("При сохранении изменений произошла ошибка");
                }
                finally
                {
                    Container.Release(serviceActCheckViol);
                }
            }

            return JsSuccess();
        }

        //Прокси класс для десериализации JSON
        private class ActCheckViolationProxy
        {
            public int Id { get; set; }

            public int ActObject { get; set; }

            public int ViolationGjiId { get; set; }

            public DateTime? DatePlanRemoval { get; set; }
        }
    }
}