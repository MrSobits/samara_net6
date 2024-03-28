namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using Gkh.Entities;
    using Gkh.Enums;
    using Entities;
    using Enums;

    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    public class ActCheckRealityObjectService : IActCheckRealityObjectService
    {
        public IWindsorContainer Container { get; set; }

        public virtual IDataResult SaveParams(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var actObjectId = baseParams.Params.GetAs<long>("actObjectId");
                    var haveViolation = baseParams.Params.GetAs("haveViolation", YesNoNotSet.NotSet);
                    var description = baseParams.Params.GetAs<string>("description");
                    var officialsGuiltyActions = baseParams.Params.GetAs<string>("officialsGuiltyActions");
                    var personsWhoHaveViolated = baseParams.Params.GetAs<string>("personsWhoHaveViolated");

                    #region Обновление проверки

                    var service = Container.Resolve<IDomainService<ActCheckRealityObject>>();

                    //Обновляем родительскую запись (проверку)
                    var actCheckGjiRealityObject = service.Get(actObjectId);
                    actCheckGjiRealityObject.HaveViolation = haveViolation;
                    actCheckGjiRealityObject.Description = description;
                    actCheckGjiRealityObject.PersonsWhoHaveViolated = personsWhoHaveViolated;
                    actCheckGjiRealityObject.OfficialsGuiltyActions = officialsGuiltyActions;
                    service.Update(actCheckGjiRealityObject);

                    #endregion

                    SaveViolations(baseParams, actCheckGjiRealityObject, actObjectId);

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (ValidationException e)
                {
                    transaction.Rollback();
                    return new BaseDataResult {Success = false, Message = e.Message};
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    Container.Resolve<ILogger>().LogError(e, e.Message);
                    return new BaseDataResult {Success = false, Message = e.Message};
                }
            }
        }

        // Сохранение нарушений
        protected virtual void SaveViolations(BaseParams baseParams, ActCheckRealityObject actCheckGjiRealityObject, long actObjectId)
        {
            //сериализуем JSON в объект ActViolationProxy
            var actViolationProxy = baseParams.Params.GetAs<List<ActViolationProxy>>("actViolationJson");
            var changeCancelAndFactDates = baseParams.Params.GetAs<bool>("changeCancelAndFactDates");

            //Получаем service для нарушений
            var serviceInspectionViol = Container.Resolve<IDomainService<InspectionGjiViol>>();
            var serviceActCheckViol = Container.Resolve<IDomainService<ActCheckViolation>>();

            
           var serviceInspection = Container.Resolve<IDomainService<InspectionGji>>();
            var serviceRobject = Container.Resolve<IDomainService<RealityObject>>();
            var serviceViolation = Container.Resolve<IDomainService<ViolationGji>>();

            if (actViolationProxy != null)
            {
                #region Формируем словаь существующих записей

                //В этом словаре будет существующие нарушения
                //key - идентификатор Нарушения Акта проверки
                //value - объект Нарушения акта проверки
                //Получаем Dictionary<long, ActCheckViolation>
                var dictActViolations =
                    serviceActCheckViol.GetAll()
                        .Where(x => x.ActObject.Id == actObjectId)
                        .ToDictionary(x => x.Id);

                #endregion

                //пробегаем по списку если id = 0 то добавляем если id>0 то обновляем нарушение
                foreach (var actViolationProxyItem in actViolationProxy)
                {
                    if (actViolationProxyItem.Id == 0)
                    {
                        var newViol = new InspectionGjiViol
                        {
                            Inspection = serviceInspection.Load(actCheckGjiRealityObject.ActCheck.Inspection.Id),
                            RealityObject = actCheckGjiRealityObject.RealityObject != null ? serviceRobject.Load(actCheckGjiRealityObject.RealityObject.Id) : null,
                            Violation = serviceViolation.Load(actViolationProxyItem.ViolationGjiId),
                            DatePlanRemoval = actViolationProxyItem.DatePlanRemoval
                        };

                        if (changeCancelAndFactDates)
                        {
                            newViol.DateFactRemoval = actViolationProxyItem.DateFactRemoval;
                            newViol.DateCancel = actViolationProxyItem.DateCancel;
                        }

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

                        if (changeCancelAndFactDates)
                        {
                            newObj.DateFactRemoval = actViolationProxyItem.DateFactRemoval;
                        }

                        serviceActCheckViol.Save(newObj);
                    }
                    else
                    {
                        if (dictActViolations.ContainsKey(actViolationProxyItem.Id))
                        {
                            var actViol = dictActViolations[actViolationProxyItem.Id];

                            actViol.DatePlanRemoval = actViolationProxyItem.DatePlanRemoval;
                            if (changeCancelAndFactDates)
                            {
                                actViol.DateFactRemoval = actViolationProxyItem.DateFactRemoval;
                                actViol.InspectionViolation.DateFactRemoval = actViolationProxyItem.DateFactRemoval;
                                actViol.InspectionViolation.DateCancel = actViolationProxyItem.DateCancel;
                            }
                            serviceActCheckViol.Update(actViol);

                            //удаляем из словарика чтобы не Удалить из Базы
                            dictActViolations.Remove(actViolationProxyItem.Id);
                        }
                    }
                }

                #region Удаляем нарушения которые считаются удаленными

                //Оставшиеся в словарике объекты удаляем из БД
                foreach (var keyValue in dictActViolations)
                {
                    serviceActCheckViol.Delete(keyValue.Key);
                }

                #endregion

            }
        }

        //Прокси класс для десериализации JSON
        private class ActViolationProxy
        {
            public long Id { get; set; }

            public long ViolationGjiId { get; set; }

            public DateTime? DatePlanRemoval { get; set; }
            
            public DateTime? DateFactRemoval { get; set; }

            public DateTime? DateCancel { get; set; }
        }
    }
}