namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ActSurveyServiceInterceptor : ActSurveyServiceInterceptor<ActSurvey>
    {
    }

    public class ActSurveyServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T: ActSurvey
    {
        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var annexService = this.Container.Resolve<IDomainService<ActSurveyAnnex>>();
            var partService = this.Container.Resolve<IDomainService<ActSurveyInspectedPart>>();
            var ownerService = this.Container.Resolve<IDomainService<ActSurveyOwner>>();
            var photoService = this.Container.Resolve<IDomainService<ActSurveyPhoto>>();
            var domainServiceObject = Container.Resolve<IDomainService<ActSurveyRealityObject>>();

            // Перед удалением проверяем есть ли дочерние документы
            try
            {
                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return Failure(result.Message);
                }

                var refFuncs = new List<Func<long, string>>
                               {
                                  id => annexService.GetAll().Any(x => x.ActSurvey.Id == id) ? "Приложения" : null,
                                  id => partService.GetAll().Any(x => x.ActSurvey.Id == id) ? "Инспектируемые части" : null,
                                  id => ownerService.GetAll().Any(x => x.ActSurvey.Id == id) ? "Сведения о собсвенниках" : null,
                                  id => photoService.GetAll().Any(x => x.ActSurvey.Id == id) ? "Фото в акте обследования ГЖИ" : null
                               };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return Failure(message);
                }

                // Удаляем всех дочерних Дома
                var objectIds = domainServiceObject.GetAll().Where(x => x.ActSurvey.Id == entity.Id)
                .Select(x => x.Id).ToList();

                foreach (var objectId in objectIds)
                {
                    domainServiceObject.Delete(objectId);
                }

                return result;
            }
            finally 
            {
                Container.Release(annexService);
                Container.Release(partService);
                Container.Release(ownerService);
                Container.Release(photoService);
                Container.Release(domainServiceObject);
            }
        }

        public override IDataResult AfterCreateAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.Id.ToString());
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<T> service, T entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DocumentGji, entity.Id, entity.GetType(), GetPropertyValues(), entity.DocumentNumber);
            }
            catch
            {

            }
            return this.Success();
        }

        private Dictionary<string, string> GetPropertyValues()
        {
            var result = new Dictionary<string, string>
            {
                { "Inspection", "Проверка ГЖИ" },
                { "Stage", "Этап проверки" },
                { "TypeDocumentGji", "Тип документа ГЖИ" },
                { "DocumentDate", "Дата документа" },
                { "DocumentNumber", "Номер документа" },
                { "DocumentSubNum", "Дополнительный номер документа (порядковый номер если документов одного типа несколько)" },
                { "State", "Статус" },
                { "Area", "Обследованная площадь" },
                { "Flat", "Квартира" },
                { "Reason", "Причина" },
                { "Description", "Выводы по результату" },
                { "FactSurveyed", "Факт обследования" }
            };
            return result;
        }
    }
}