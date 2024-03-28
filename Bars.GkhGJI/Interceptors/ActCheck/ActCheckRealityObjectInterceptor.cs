namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using System.Collections.Generic;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    public class ActCheckRealityObjectInterceptor: EmptyDomainInterceptor<ActCheckRealityObject>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ActCheckRealityObject> service, ActCheckRealityObject entity)
        {
            var domainActViolation = Container.Resolve<IDomainService<ActCheckViolation>>();
            try
            {
                // Удаляем все дочерние Нарушения акта
                var violationIds = domainActViolation.GetAll()
                    .Where(x => x.ActObject.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList();

                foreach (var violId in violationIds)
                {
                    domainActViolation.Delete(violId);
                }
            }
            finally 
            {
                Container.Release(domainActViolation);
            }
            return base.BeforeDeleteAction(service, entity);
        }

        public override IDataResult AfterDeleteAction(IDomainService<ActCheckRealityObject> service, ActCheckRealityObject entity)
        {
            this.CreateReminders(entity);

            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiRealityObject, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.DocumentNumber + " " + entity.RealityObject.Address);
            }
            catch
            {

            }

            return this.Success();
        }

        public override IDataResult AfterUpdateAction(IDomainService<ActCheckRealityObject> service, ActCheckRealityObject entity)
        {
            this.CreateReminders(entity);

            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiRealityObject, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.DocumentNumber + " " + entity.RealityObject.Address);
            }
            catch
            {

            }

            return this.Success();
        }

        public override IDataResult AfterCreateAction(IDomainService<ActCheckRealityObject> service, ActCheckRealityObject entity)
        {
            this.CreateReminders(entity);

            var logEntityHistoryService = this.Container.Resolve<ILogEntityHistoryService>();
            var roDomain = Container.ResolveDomain<RealityObject>();
            try
            {
                logEntityHistoryService.UpdateLog(entity, TypeEntityLogging.DocumentGjiRealityObject, entity.Id, entity.GetType(), GetPropertyValues(), entity.ActCheck.Id.ToString() + " " + roDomain.Get(entity.RealityObject.Id).Address);
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
                { "ActCheck", "Акт проверки" },
                { "RealityObject", "Жилой дом" },
                { "Description", "Описание" },
                { "NotRevealedViolations", "Не выявленные нарушения" },
                { "PersonsWhoHaveViolated", "Сведения о лицах, допустивших нарушения" },
                { "OfficialsGuiltyActions", "Сведения, свидетельствующие, что нарушения допущены в результате виновных действий (бездействия) должностных лиц и (или) работников проверяемого лица" },
            };
            return result;
        }

        private void CreateReminders(ActCheckRealityObject entity)
        {
            // Получаем правила формирования Напоминаний и запускаем метод создания напоминаний
            var servReminderRule = Container.ResolveAll<IReminderRule>();

            try
            {
                var rule = servReminderRule.FirstOrDefault(x => x.Id == "InspectionReminderRule");
                if (rule != null)
                {
                    rule.Create(entity);
                }
            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                Container.Release(servReminderRule);
            }
        }
    }
}