namespace Bars.GkhGji.Regions.BaseChelyabinsk.ReminderRule
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;

    using Castle.Windsor;
    using Entities.Reminder;
    /// <summary>
    /// Класс создания напоминания в случае если инициатором был обращение граждан 
    /// </summary>
    public class AppealCitsReminderRule : IReminderRule
    {
        public IWindsorContainer Container { get; set; }


        public string Id
        {
            get { return "AppealCitsReminderRule"; }
        }

        /// <summary>
        /// В результате выполнения этого метода необходимо создать 1 напоминалку по обращению
        /// либо удалить ту которая существовала
        /// 1. Если Обрашение находится не на конечном статусе мы добавляем напоминалку
        /// 2. Иначе мы удаляем существующую напоминалку
        /// </summary>
        public void Create(IEntity entity)
        {
            if (entity is AppealCits)
            {

                var servReminder = this.Container.Resolve<IDomainService<ChelyabinskReminder>>();
                var servAppCitsExecutant = this.Container.Resolve<IDomainService<AppealCitsExecutant>>();

                try
                {
                    var appealCits = entity as AppealCits;

                    servReminder.GetAll()
                        .Where(x => x.AppealCits.Id == appealCits.Id)
                        .Select(x => x.Id)
                        .ForEach(x => servReminder.Delete(x));

                    if (!appealCits.Return(x => x.State).Return(x => x.FinalState))
                    {                      
                        var appealCitsExecutants = servAppCitsExecutant.GetAll()
                            .Where(x => x.AppealCits.Id == appealCits.Id).ToList();
                       
                        foreach (var appealCitsExecutant in appealCitsExecutants)
                        {
                            var rem = new ChelyabinskReminder
                            {
                                Actuality = true,
                                TypeReminder = TypeReminder.Statement,
                                CategoryReminder = CategoryReminder.ExecutionStatemen,
                                Num = appealCits.NumberGji,
                                CheckDate = appealCits.ExtensTime ?? (appealCits.CheckTime != DateTime.MinValue ? appealCits.CheckTime : null),
                                AppealCits = appealCits,
                                AppealCitsExecutant = appealCitsExecutant,
                                Inspector = appealCitsExecutant.Controller != null? appealCitsExecutant.Controller: appealCitsExecutant.Executant
                            };

                        servReminder.Save(rem);
                        }
                    }
                }
                catch (Exception exc)
                {
                    throw exc;
                }
                finally
                {
                    this.Container.Release(servReminder);
                    this.Container.Release(servAppCitsExecutant);
                }
            }
        }
    }
}
