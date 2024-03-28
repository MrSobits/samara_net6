namespace Bars.GkhGji.Regions.Khakasia.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Khakasia.Entities;

	/// <summary>
	/// Сервис для Жилой дом акта проверки
	/// </summary>
    public class ActCheckRealityObjectService : GkhGji.DomainService.ActCheckRealityObjectService
    {
		/// <summary>
		/// Сохранить параметры
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public override IDataResult SaveParams(BaseParams baseParams)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    var actObjectId = baseParams.Params.GetAs<long>("actObjectId");
                    var haveViolation = baseParams.Params.GetAs("haveViolation", YesNoNotSet.NotSet);
                    var description = baseParams.Params.GetAs<string>("description");
                    var notRevViol = baseParams.Params.GetAs<string>("notRevViol");

                    #region Обновление проверки

                    var service = this.Container.Resolve<IDomainService<ActCheckRealityObject>>();

                    //Обновляем родительскую запись (проверку)
                    var actCheckGjiRealityObject = service.Get(actObjectId);
                    actCheckGjiRealityObject.HaveViolation = haveViolation;
                    actCheckGjiRealityObject.Description = description;
                    actCheckGjiRealityObject.NotRevealedViolations = notRevViol;
                    service.Update(actCheckGjiRealityObject);

                    #endregion

                    this.SaveViolations(baseParams, actCheckGjiRealityObject, actObjectId);

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

        /// <summary>
		/// Сохранить нарушения
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <param name="actCheckGjiRealityObject">Жилой дом акта проверки</param>
		/// <param name="actObjectId">Идентификатор акта проверки</param>
        protected override void SaveViolations(BaseParams baseParams, ActCheckRealityObject actCheckGjiRealityObject, long actObjectId)
        {
            //сериализуем JSON в объект ActViolationProxy
            var actViolationProxy = baseParams.Params.GetAs<List<ActViolationProxy>>("actViolationJson");

            //Получаем service для нарушений
            var serviceInspectionViol = this.Container.Resolve<IDomainService<InspectionGjiViol>>();
            var serviceActCheckViol = this.Container.Resolve<IDomainService<ActCheckViolation>>();
            var serviceInspection = this.Container.Resolve<IDomainService<InspectionGji>>();
            var serviceRobject = this.Container.Resolve<IDomainService<RealityObject>>();
            var serviceViolation = this.Container.Resolve<IDomainService<ViolationGji>>();
            var serviceInspectionGjiViolWording = this.Container.Resolve<IDomainService<InspectionGjiViolWording>>();

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
                        Inspection = new InspectionGji { Id = actCheckGjiRealityObject.ActCheck.Inspection.Id },
                        RealityObject = actCheckGjiRealityObject.RealityObject != null ? new RealityObject { Id = actCheckGjiRealityObject.RealityObject.Id } : null,
                        Violation = new ViolationGji { Id = actViolationProxyItem.ViolationGjiId },
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