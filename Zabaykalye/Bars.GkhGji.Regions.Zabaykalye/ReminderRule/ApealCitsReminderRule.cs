using System;
using System.Collections.Generic;
using System.Linq;
using Bars.GkhGji.Contracts.Reminder;
using Bars.B4.DataAccess;
using Bars.GkhGji.Entities;
using Castle.Windsor;

namespace Bars.GkhGji.Regions.Zabaykalye
{

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Regions.Zabaykalye.Entities;
    using Bars.GkhGji.Regions.Zabaykalye.Entities.AppealCits;

    /// <summary>
    /// Класс создания напоминания в случае если инициатором был обращение граждан 
    /// </summary>
    public class AppealCitsReminderRule: IReminderRule
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
                var servReminder = Container.Resolve<IDomainService<Reminder>>();
                var servAppCitsExecutant= Container.Resolve<IDomainService<AppealCitsExecutant>>();

                try
                {
                    var obj = entity as AppealCits;

                    // Список напоминаний которые необходимо будет либо создавать либ обновлять
                    var listRemindersToSave = new List<Reminder>();

                    var dictCurrReminders =
                            servReminder.GetAll().Where(x => x.AppealCits.Id == obj.Id).AsEnumerable().ToDictionary(x => x.Id);

                    if (!obj.State.FinalState)
                    {

                        // Получаем список напоминаний по инспекторам данного обращения
                        // Для того чтобы потом понимать какие необходимо обновить 
                        // а какие уже ненужны и их нужно удалить
                        var dictCurrByInspector = servReminder.GetAll()
                            .Where(x => x.AppealCits.Id == obj.Id)
                            .AsEnumerable()
                            .GroupBy(x => x.Inspector.Id)
                            .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                        // Список Идентификаторов Инспекторов данного обращения их может быть от 1 до 3х
                        var executants =
                            servAppCitsExecutant.GetAll()
                                                .Where(x => x.AppealCits.Id == obj.Id)
                                                .Select(
                                                x => new { x.Author, x.Executant })
                                                .ToList();

                        var listInspectors = executants.Where(x => x.Executant != null).Select(x => x.Executant.Id).Distinct().ToList();

                        listInspectors.AddRange(executants.Where(x => x.Author != null && !listInspectors.Contains(x.Author.Id)).Select(x => x.Author.Id).Distinct().ToList());
                                                
                        // Если статус не Закрыто то выполняем действия для создания напоминалок
                        foreach (var idIns in listInspectors.Distinct())
                        {
                            var rem = new Reminder();

                            if (dictCurrByInspector.ContainsKey(idIns))
                            {
                                rem = dictCurrByInspector[idIns];

                                // Если такой инсепктор уже был, то мы его
                                // удаляем из списка Текущих Инспеторов
                                dictCurrByInspector.Remove(idIns);

                                if (dictCurrReminders.ContainsKey(rem.Id))
                                {
                                    dictCurrReminders.Remove(rem.Id);
                                }
                            }

                            rem.Actuality = true;
                            rem.TypeReminder = TypeReminder.Statement;
                            rem.CategoryReminder = CategoryReminder.ExecutionStatemen;
                            rem.Num = obj.NumberGji;
                            rem.CheckDate = obj.CheckTime != DateTime.MinValue ? obj.CheckTime : null;
                            rem.AppealCits = new AppealCits { Id = obj.Id };
                            rem.Inspector = new Inspector { Id = idIns };

                            listRemindersToSave.Add(rem);
                        }
                    }

                    // Записи в этом списке либо добавляем либо обновляем
                    foreach (var item in listRemindersToSave)
                    {
                        if (item.Id > 0)
                        {
                            servReminder.Update(item);
                        }
                        else
                        {
                            servReminder.Save(item);
                        }
                    }

                    // Оставшиеся записи в словаре необходимо удалить Поскольку возможно они уже не актуалны
                    foreach (var kvp in dictCurrReminders)
                    {
                        if (kvp.Value != null)
                        {
                            servReminder.Delete(kvp.Value.Id);
                        }
                    }
                }
                catch (Exception exc)
                {
                    throw exc;
                }
                finally
                {
                    Container.Release(servReminder);
                    Container.Release(servAppCitsExecutant);
                }
            }
        }
    }
}
