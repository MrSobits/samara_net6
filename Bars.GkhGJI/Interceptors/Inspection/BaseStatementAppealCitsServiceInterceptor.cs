namespace Bars.GkhGji.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.Entities;

    using Microsoft.Extensions.Logging;

    public class BaseStatementAppealCitsServiceInterceptor : EmptyDomainInterceptor<InspectionAppealCits>
    {
        public override IDataResult AfterCreateAction(IDomainService<InspectionAppealCits> service, InspectionAppealCits entity)
        {
            var servAction = Container.Resolve<IBaseStatementAction>();
            try
            {
                // после сохранения переводим статус обращения гражданина
                servAction.Create(entity);
                this.CreateReminders(entity);

                return this.Success();
            }
            catch (Exception exp)
            {
                Container.Resolve<ILogger>().LogError(exp, "Перевод статуса обращения после создания проверки");
                return this.Success();
            }
            finally
            {
                Container.Release(servAction);
            }
          
        }

        public override IDataResult BeforeDeleteAction(IDomainService<InspectionAppealCits> service, InspectionAppealCits entity)
        {
            // Удаляем напоминания по проверке
            var servReminder = Container.Resolve<IDomainService<Reminder>>();

            try
            {
                var reminderIds = servReminder.GetAll()
                                            .Where(x => x.InspectionGji.Id == entity.Id)
                                            .Select(x => x.Id)
                                            .ToList();
                foreach (var value in reminderIds)
                {
                    servReminder.Delete(value);
                }

                return this.Success();
            }
            finally
            {
                Container.Release(servReminder);
            }
            
        }

        public override IDataResult AfterUpdateAction(IDomainService<InspectionAppealCits> service, InspectionAppealCits entity)
        {
            this.CreateReminders(entity);

            return this.Success();
        }

        private void CreateReminders(InspectionAppealCits entity)
        {
            // Получаем правила формирования Напоминаний и запускаем метод создания напоминаний
            var servReminderRule = Container.ResolveAll<IReminderRule>();

            try
            {
                var rule = servReminderRule.FirstOrDefault(x => x.Id == "AppealCitsReminderRule");
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
