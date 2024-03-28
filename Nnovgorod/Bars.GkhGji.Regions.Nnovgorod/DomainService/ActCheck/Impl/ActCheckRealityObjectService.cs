namespace Bars.GkhGji.Regions.Nnovgorod.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Nnovgorod.Entities;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;

    public class ActCheckRealityObjectService : GkhGji.DomainService.ActCheckRealityObjectService
    {
        public override IDataResult SaveParams(BaseParams baseParams)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var actObjectId = baseParams.Params.GetAs<long>("actObjectId");
                    var haveViolation = baseParams.Params.GetAs("haveViolation", YesNoNotSet.NotSet);
                    var description = baseParams.Params.GetAs<string>("description");
                    var notRevViol = baseParams.Params.GetAs<string>("notRevViol");

                    #region Обновление проверки

                    var service = Container.Resolve<IDomainService<ActCheckRealityObject>>();

                    //Обновляем родительскую запись (проверку)
                    var actCheckGjiRealityObject = service.Get(actObjectId);
                    actCheckGjiRealityObject.HaveViolation = haveViolation;
                    actCheckGjiRealityObject.Description = description;
                    actCheckGjiRealityObject.NotRevealedViolations = notRevViol;
                    service.Update(actCheckGjiRealityObject);

                    #endregion

                    SaveViolations(baseParams, actCheckGjiRealityObject, actObjectId);

                    transaction.Commit();
                    return new BaseDataResult();
                }
                catch (Exception e)
                {
                    transaction.Rollback();
                    return new BaseDataResult { Success = false, Message = e.Message };
                }
            }
        }

        // Сохранение нарушений
        protected override void SaveViolations(BaseParams baseParams, ActCheckRealityObject actCheckGjiRealityObject, long actObjectId)
        {
            //сериализуем JSON в объект ActViolationProxy
            var actViolationProxy = baseParams.Params.GetAs<List<ActViolationProxy>>("actViolationJson");

            //Получаем service для нарушений
            var serviceInspectionViol = Container.Resolve<IDomainService<InspectionGjiViol>>();
            var serviceActCheckViol = Container.Resolve<IDomainService<ActCheckViolation>>();
            var serviceInspection = Container.Resolve<IDomainService<InspectionGji>>();
            var serviceRobject = Container.Resolve<IDomainService<RealityObject>>();
            var serviceViolation = Container.Resolve<IDomainService<ViolationGji>>();
            var serviceInspectionGjiViolWording = Container.Resolve<IDomainService<InspectionGjiViolWording>>();

            #region Формируем словаь существующих записей

            //В этом словаре будет существующие нарушения
            //key - идентификатор Нарушения Акта проверки
            //value - объект Нарушения акта проверки
            //Получаем Dictionary<int, ActCheckViolation>
            var actViolationsQuery = serviceActCheckViol.GetAll().Where(x => x.ActObject.Id == actObjectId);

            var dictActViolations = actViolationsQuery.ToDictionary(x => x.Id);

            var violationWordingsDict = serviceInspectionGjiViolWording.GetAll()
                .Where(x => actViolationsQuery.Select(y => y.InspectionViolation.Id).Contains(x.InspectionViolation.Id))
                .ToDictionary(x => x.InspectionViolation.Id);

            #endregion

            #region Сохранение нарушений
            
            //пробегаем по списку если id = 0 то добавляем если id>0 то обновляем нарушение
            foreach (var actViolationProxyItem in actViolationProxy)
            {
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

                    if (!string.IsNullOrWhiteSpace(actViolationProxyItem.ViolationWording))
                    {
                        // Создаем формулировку нарушения
                        var violWording = new InspectionGjiViolWording
                        {
                            InspectionViolation = newViol,
                            Wording = actViolationProxyItem.ViolationWording
                        };

                        serviceInspectionGjiViolWording.Save(violWording);
                    }

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
                else
                {
                    if (dictActViolations.ContainsKey(actViolationProxyItem.Id))
                    {
                        //Необходимо проверить изменился ли срок устранения и если изменился то обновляем записи
                        var actViol = dictActViolations[actViolationProxyItem.Id];
                        if (actViol.DatePlanRemoval != actViolationProxyItem.DatePlanRemoval)
                        {
                            actViol.DatePlanRemoval = actViolationProxyItem.DatePlanRemoval;
                            serviceActCheckViol.Update(actViol);
                        }

                        //удаляем из словарика чтобы не Удалить из Базы
                        dictActViolations.Remove(actViolationProxyItem.Id);

                        //Если изменились формулировки нарушений то обновляем записи
                        if (violationWordingsDict.ContainsKey(actViol.InspectionViolation.Id))
                        {
                            var violationWording = violationWordingsDict[actViol.InspectionViolation.Id];

                            if (violationWording.Wording != actViolationProxyItem.ViolationWording)
                            {
                                violationWording.Wording = actViolationProxyItem.ViolationWording;
                                serviceInspectionGjiViolWording.Save(violationWording);
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrWhiteSpace(actViolationProxyItem.ViolationWording))
                            {
                                // Создаем формулировку нарушения
                                var violWording = new InspectionGjiViolWording
                                {
                                    InspectionViolation = actViol.InspectionViolation,
                                    Wording = actViolationProxyItem.ViolationWording
                                };

                                serviceInspectionGjiViolWording.Save(violWording);
                            }
                        }
                    }
                }
            }

            #endregion

            #region Удаляем нарушения которые считаются удаленными

            //Оставшиеся в словарике объекты удаляем из БД
            foreach (var keyValue in dictActViolations)
            {
                serviceActCheckViol.Delete(keyValue.Key);
            }

            #endregion
        }

        //Прокси класс для десериализации JSON
        private class ActViolationProxy
        {
            public long Id { get; set; }

            public long ViolationGjiId { get; set; }

            public DateTime? DatePlanRemoval { get; set; }

            public string ViolationWording { get; set; }
        }
    }
}