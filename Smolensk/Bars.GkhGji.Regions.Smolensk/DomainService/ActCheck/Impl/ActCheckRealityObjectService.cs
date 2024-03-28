namespace Bars.GkhGji.Regions.Smolensk.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Smolensk.Entities;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums;

    public class ActCheckRealityObjectService : GkhGji.DomainService.ActCheckRealityObjectService
    {

        public IDomainService<InspectionGjiViol> serviceInspectionViol { get; set; }
        public IDomainService<ActCheckViolation> serviceActCheckViol { get; set; }
        public IDomainService<InspectionGji> serviceInspection { get; set; }
        public IDomainService<RealityObject> serviceRobject { get; set; }
        public IDomainService<ViolationGji> serviceViolation { get; set; }
        public IDomainService<InspectionGjiViolWording> serviceInspectionGjiViolWording { get; set; }
        public IDomainService<ActCheckRealityObject> service { get; set; }
        public IDomainService<InspectionGjiRealityObject> serviceInspectionRo { get; set; }
        public IDomainService<ActCheck> serviceActCheck { get; set; }

        private ActCheckRealityObject CurrentObject { get; set; }

        public override IDataResult SaveParams(BaseParams baseParams)
        {
            var actId = baseParams.Params.GetAs("actId", 0L);
            var actObjectId = baseParams.Params.GetAs("actObjectId", 0L);
            var roId = baseParams.Params.GetAs("roId", 0L);
            var haveViolation = baseParams.Params.GetAs("haveViolation", YesNoNotSet.NotSet);
            var description = baseParams.Params.GetAs("description", string.Empty);
            var notRevViol = baseParams.Params.GetAs("notRevViol", string.Empty);

            if (roId > 0 && service.GetAll().Any(x => x.Id != actObjectId && x.ActCheck.Id == actId && x.RealityObject.Id == roId))
            {
                return new BaseDataResult(false, "Результаты проверки по данному дому уже указаны");
            }

            if (roId == 0 && service.GetAll().Any(x => x.Id != actObjectId && x.ActCheck.Id == actId && x.RealityObject == null))
            {
                return new BaseDataResult(false, "Запись уже создана");
            }

            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var actCheck = serviceActCheck.GetAll().FirstOrDefault(x => x.Id == actId);

                    // Обновляем родительскую запись (проверку)
                    ActCheckRealityObject actCheckGjiRealityObject = null;

                    if (actObjectId > 0)
                    {
                        actCheckGjiRealityObject = service.FirstOrDefault(x => x.Id == actObjectId);
                    }
                    else
                    {
                        actCheckGjiRealityObject = new ActCheckRealityObject()
                                                       {
                                                           RealityObject =
                                                               roId > 0
                                                                   ? new RealityObject { Id = roId }
                                                                   : null,
                                                           ActCheck = actCheck
                                                       };
                    }
                    
                    actCheckGjiRealityObject.HaveViolation = haveViolation;
                    actCheckGjiRealityObject.Description = description;
                    actCheckGjiRealityObject.NotRevealedViolations = notRevViol;

                    if (actCheckGjiRealityObject.Id > 0)
                    {
                        service.Update(actCheckGjiRealityObject);
                    }
                    else
                    {
                        service.Save(actCheckGjiRealityObject);
                    }
                    
                    // если такого дома в проверке нет, то добавляем его в основание проверки
                    if (roId > 0 && !serviceInspectionRo.GetAll().Any(x => x.Inspection.Id == actCheck.Inspection.Id && x.RealityObject.Id == roId))
                    {
                        var inspectionRo = new InspectionGjiRealityObject
                            {
                                Inspection = actCheck.Inspection,
                                RealityObject = new RealityObject { Id = roId }
                            };

                        serviceInspectionRo.Save(inspectionRo);
                    }

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
            // сериализуем JSON в объект ActViolationProxy
            var actViolationProxy = baseParams.Params.GetAs<List<ActViolationProxy>>("actViolationJson");

            var dictActViolations = new Dictionary<long, ActCheckViolation>();
            var violationWordingsDict = new Dictionary<long, InspectionGjiViolWording>();

            if (actObjectId > 0)
            {
                // В этом словаре будет существующие нарушения
                // key - идентификатор Нарушения Акта проверки
                // value - объект Нарушения акта проверки
                // Получаем Dictionary<int, ActCheckViolation>
                var actViolationsQuery = serviceActCheckViol.GetAll().Where(x => x.ActObject.Id == actObjectId);

                dictActViolations = actViolationsQuery.ToDictionary(x => x.Id);

                violationWordingsDict = serviceInspectionGjiViolWording.GetAll()
                    .Where(x => actViolationsQuery.Select(y => y.InspectionViolation.Id).Contains(x.InspectionViolation.Id))
                    .ToDictionary(x => x.InspectionViolation.Id);    
            }

            // пробегаем по списку если id = 0 то добавляем если id>0 то обновляем нарушение
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

                    // Формируем список нарушений и ставим тип TypeViolationStage.Detection (Выявление)
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
                        // Необходимо проверить изменился ли срок устранения и если изменился то обновляем записи
                        var actViol = dictActViolations[actViolationProxyItem.Id];
                        if (actViol.DatePlanRemoval != actViolationProxyItem.DatePlanRemoval)
                        {
                            actViol.DatePlanRemoval = actViolationProxyItem.DatePlanRemoval;
                            serviceActCheckViol.Update(actViol);
                        }

                        // удаляем из словарика чтобы не Удалить из Базы
                        dictActViolations.Remove(actViolationProxyItem.Id);

                        // Если изменились формулировки нарушений то обновляем записи
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

            //Оставшиеся в словарике объекты удаляем из БД
            foreach (var keyValue in dictActViolations)
            {
                serviceActCheckViol.Delete(keyValue.Key);
            }
            
        }

        // Прокси класс для десериализации JSON
        private class ActViolationProxy
        {
            public long Id { get; set; }

            public long ViolationGjiId { get; set; }

            public DateTime? DatePlanRemoval { get; set; }

            public string ViolationWording { get; set; }
        }
    }
}