namespace Bars.GkhGji.Interceptors
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DecisionControlSubjectsInterceptor : EmptyDomainInterceptor<DecisionControlSubjects>
    {
        public override IDataResult AfterCreateAction(IDomainService<DecisionControlSubjects> service, DecisionControlSubjects entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DecisionControlSubjects, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.Id.ToString() + " " + entity.PhysicalPerson);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<DecisionControlSubjects> service, DecisionControlSubjects entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DecisionControlSubjects, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.DocumentNumber + " " + entity.PhysicalPerson);
            }
            catch
            {

            }
            return this.Success();
        }

        public override IDataResult AfterDeleteAction(IDomainService<DecisionControlSubjects> service, DecisionControlSubjects entity)
        {
            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(null, TypeEntityLogging.DecisionControlSubjects, entity.Id, entity.GetType(), GetPropertyValues(), entity.Decision.DocumentNumber + " " + entity.PhysicalPerson);
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
                { "Decision", "Распоряжение" },
                { "PersonInspection", "Объект проверки" },
                { "Contragent", "Контрагент проверки" },
                { "PhysicalPerson", "Физическое лицо" },
                { "PhysicalPersonINN", "ИНН физического лица" },
                { "PhysicalPersonPosition", "Должность физ.лица" }
            };
            return result;
        }
    }
}