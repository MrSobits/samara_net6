namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Entities;
    using Enums;
    using Gkh.Entities;

    public class PreventiveVisitResultViolationController : B4.Alt.DataController<PreventiveVisitResultViolation>
    {
        public override ActionResult Create(BaseParams baseParams)
        {
            var actViolation = baseParams.Params.GetAs<List<PreventiveVisitResultViolationProxy>>("records");
            if (actViolation == null)
            {
                return JsFailure("При сохранении нарушений произошла ошибка");
            }

            var serviceResult = Container.Resolve<IDomainService<PreventiveVisitResult>>();
            var serviceResultViolation = Container.Resolve<IDomainService<PreventiveVisitResultViolation>>();
            var serviceViolation = Container.Resolve<IDomainService<ViolationGji>>();

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var actViolationProxyItem in actViolation)
                    {

                        if (actViolationProxyItem.Id == 0)
                        {
                            var newViol = new PreventiveVisitResultViolation
                            {
                                PreventiveVisitResult = serviceResult.Load(actViolationProxyItem.PreventiveVisitResult),
                                ViolationGji = serviceViolation.Load(actViolationProxyItem.ViolationGji)
                            };

                            serviceResultViolation.Save(newViol);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return JsonNetResult.Failure("При сохранении изменений произошла ошибка");
                }
                finally
                {
                    Container.Release(serviceResultViolation);
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

            var serviceActCheckViol = Container.ResolveDomain<PreventiveVisitResultViolation>();
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
        private class PreventiveVisitResultViolationProxy
        {
            public int Id { get; set; }

            public long PreventiveVisitResult { get; set; }

            public long ViolationGji { get; set; }
        }
    }
}