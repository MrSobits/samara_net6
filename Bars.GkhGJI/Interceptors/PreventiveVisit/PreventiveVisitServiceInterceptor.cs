namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class PreventiveVisitServiceInterceptor : PreventiveVisitServiceInterceptor<PreventiveVisit>
    {
        // Внимание !!! override делать в Generic классе
    }
    
    public class PreventiveVisitServiceInterceptor<T> : DocumentGjiInterceptor<T>
        where T : PreventiveVisit
    {  

        public override IDataResult BeforeDeleteAction(IDomainService<T> service, T entity)
        {
            var annexService = this.Container.Resolve<IDomainService<PreventiveVisitAnnex>>();
            var roService = this.Container.Resolve<IDomainService<PreventiveVisitRealityObject>>();
            var witnessService = this.Container.Resolve<IDomainService<PreventiveVisitWitness>>();

            try
            {

                var pvRo = roService.GetAll().Where(x => x.PreventiveVisit == entity).Select(x => x.Id).ToList();
                foreach (var roId in pvRo)
                {
                    roService.Delete(roId);
                }
                var result = base.BeforeDeleteAction(service, entity);

                if (!result.Success)
                {
                    return Failure(result.Message);
                }

                var refFuncs = new List<Func<long, string>>
                {
                    id => annexService.GetAll().Any(x => x.PreventiveVisit.Id == id) ? "Приложения" : null,                  
                    id => witnessService.GetAll().Any(x => x.PreventiveVisit.Id == id) ? "Лица, присутсвующие в акте" : null
                };

                var refs = refFuncs.Select(x => x(entity.Id)).Where(x => x != null).ToArray();

                var message = string.Empty;

                if (refs.Length > 0)
                {
                    message = refs.Aggregate(message, (current, str) => current + string.Format(" {0}; ", str));
                    message = string.Format("Существуют связанные записи в следующих таблицах: {0}", message);
                    return Failure(message);
                }             


                return result;
            }
            finally 
            {
                Container.Release(annexService);              
                Container.Release(witnessService);             
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
                { "TypePreventiveAct", "Тип акта" },
                { "Contragent", "Контрагент" },
                { "PersonInspection", "Объект проверки" },
                { "PhysicalPerson", "Физическое лицо" },
                { "PhysicalPersonInfo", "Реквизиты физ.лица" },
                { "PhysicalPersonAddress", "Адрес Физическое лицо" },
                { "ActAddress", "Место составления акта" },
                { "UsedDistanceTech", "Использованы дистационные методы" },
                { "DistanceDescription", "Описание дистационного мероприятия" },
                { "DistanceCheckDate", "Дата дистационного мероприятия" },
                { "ERKNMID", "Номер в ЕРКНМ" },
                { "KindKND", "Вид контроля/надзора" }
            };
            return result;
        }
    }
}