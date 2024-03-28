using System;
using System.Collections.Generic;
using System.Linq;
using Bars.GkhGji.Contracts.Reminder;
using Bars.B4.DataAccess;
using Bars.GkhGji.Entities;
using Castle.Windsor;

namespace Bars.GkhGji.Regions.Nnovgorod
{

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Contracts.Enums;

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
                var inspectionAppealCitsDomain = Container.Resolve<IDomainService<InspectionAppealCits>>();

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

                        // Контрагент проверки по обращению
                        var inspectionContragent =
                            inspectionAppealCitsDomain.GetAll()
                                                      .Where(x => x.AppealCits.Id == obj.Id)
                                                      .OrderByDescending(x => x.ObjectCreateDate)
                                                      .Select(x => x.Inspection.Contragent)
                                                      .FirstOrDefault();

                        // Список Идентификаторов Инспекторов данного обращения их может быть от 1 до 3х
                        var listdInspectors = new List<long>();

                        if (obj.Surety != null && !listdInspectors.Contains(obj.Surety.Id))
                        {
                            listdInspectors.Add(obj.Surety.Id);
                        }

                        if (obj.Executant != null && !listdInspectors.Contains(obj.Executant.Id))
                        {
                            listdInspectors.Add(obj.Executant.Id);
                        }

                        if (obj.Tester != null && !listdInspectors.Contains(obj.Tester.Id))
                        {
                            listdInspectors.Add(obj.Tester.Id);
                        }

                        // Если статус не Закрыто то выполняем действия для создания напоминалок
                        foreach (var idIns in listdInspectors.Distinct())
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
                            rem.Num = obj.DocumentNumber;
                            rem.CheckDate = obj.CheckTime != DateTime.MinValue ? obj.CheckTime : null;
                            rem.AppealCits = new AppealCits { Id = obj.Id };
                            rem.Inspector = new Inspector { Id = idIns };
                            rem.Contragent = inspectionContragent;

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
                        servReminder.Delete(kvp.Key);
                    }
                }
                catch (Exception exc)
                {
                    throw exc;
                }
                finally
                {
                    Container.Release(servReminder);
                    Container.Release(inspectionAppealCitsDomain);
                }
            }
        }
    }
}
