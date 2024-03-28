using System;
using System.Collections.Generic;
using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.Quartz.Impl
{
    using B4;
    using B4.DataAccess;

    using Bars.GkhGji.Enums;

    using Gkh.Entities;
    using Contracts.Enums;
    using GkhGji.Entities;
    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    public class ReminderResolution: IReminderResolution
    {

        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Данный метод выполняет либо создание новых напоминалок с Типом Постановление
        /// либо удалет ненужные которые уже не является напоминаниями 
        /// </summary>
        public bool CreateReminders(B4.Utils.DynamicDictionary dict, out string message)
        {
            message = string.Empty;

            var reminderService = Container.Resolve<IDomainService<Reminder>>();
            var resolutionService = Container.Resolve<IDomainService<Resolution>>();
            var childrenService = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var protocolArticleLawService = Container.Resolve<IDomainService<ProtocolArticleLaw>>();
            var resolutionPayService = Container.Resolve<IDomainService<ResolutionPayFine>>();

            try
            {
                var listToSave = new List<Reminder>();
                
                var currentList = reminderService.GetAll()
                                   .Where(
                                       x =>
                                       x.TypeReminder == TypeReminder.Resolution && x.DocumentGji != null
                                       && x.Inspector != null)
                                   .Select(
                                       x =>
                                       new
                                           {
                                               x.Id,
                                               docId = x.DocumentGji.Id,
                                               inspectorId = x.Inspector.Id,
                                               x.Num,
                                               x.CheckDate,
                                               ContragentId = (x.Contragent != null ? x.Contragent.Id : 0)
                                           })
                                   .ToList();

                var dictForReminder = currentList.ToDictionary(x => x.Id);

                // получаем текущие напоминалки, которые моугт удалится если
                var dictForDocument = currentList
                                   .GroupBy(x => x.docId)
                                   .ToDictionary(x => x.Key, y => y.First());

                /*
                    Исключаем постановления попадающие под условия :
                    1) Постановления у которых родительский документ имеет один из типов:
                       * Протокол МВД
                       * Протокол ГЖИ
                       * Протокол МЖК
                    2) У которых вид санкции не административный штраф (Код 1)
                    3) Без Суммы штрафа
                    4) Без должностного лица
                    5) Сумма штрафа < суммы всех оплат
                    6) Из которых создан протокол, у которого во вкладке «Статьи закона» указана статья с кодом 10
                 */

                var notValidParentTypes = new[]
                {
                    TypeDocumentGji.ProtocolMvd,
                    TypeDocumentGji.Protocol,
                    TypeDocumentGji.ProtocolMhc
                };

                // Постановления с "не валидным" родительским документом
                var resolutionsWithNotValidParents = childrenService.GetAll()
                    .Where(x => notValidParentTypes.Contains(x.Parent.TypeDocumentGji))
                    .Select(x => x.Children.Id);

                var data =
                    resolutionService.GetAll()
                                     .Where(x => !resolutionsWithNotValidParents.Contains(x.Id))
                                     .Where(x => x.Sanction.Code == "1")
                                     .Where(x => x.Official != null)
                                     .Where(x => x.PenaltyAmount.HasValue && x.PenaltyAmount.Value > 0)
                                     .Where(
                                         x =>
                                         x.PenaltyAmount.Value > 
                                             /*> (resolutionPayService.GetAll()
                                                    .Any(y => y.Resolution.Id == x.Id && y.Amount.HasValue)
                                             ? */
                                                    resolutionPayService.GetAll()
                                                    .Where(y => y.Resolution.Id == x.Id)
                                                    .Where(y => y.Amount.HasValue)
                                                    .Sum(y => y.Amount).GetValueOrDefault()
                                             /*: 0)*/)
                                     .Where(
                                         x =>
                                         !childrenService.GetAll()
                                                         .Any(
                                                             y =>
                                                             (y.Parent.Id == x.Id
                                                              && y.Children.TypeDocumentGji == TypeDocumentGji.Protocol)
                                                             && protocolArticleLawService.GetAll()
                                                                                         .Any(
                                                                                             z =>
                                                                                             z.ArticleLaw.Code == "10"
                                                                                             && z.Protocol.Id
                                                                                             == y.Children.Id)))
                                     .Select(
                                         x =>
                                         new
                                         {
                                             resolId = x.Id,
                                             x.DocumentNumber,
                                             x.DeliveryDate,
                                             x.DocumentDate,
                                             ContragentId =
                                         x.Contragent != null
                                             ? x.Contragent.Id
                                             : x.Inspection.Contragent != null ? x.Inspection.Contragent.Id : 0,
                                             InspectionId = x.Inspection.Id,
                                             x.Inspection.TypeBase,
                                             OfficialId = x.Official.Id
                                         })
                                     .ToList();

                foreach (var record in data)
                {
                    DateTime? checkDate = null;
                    var curretDate = DateTime.Today;

                    // если есть ДатаВручения , то прибавляем к ней 70 дней и если эта дата > 
                    if (record.DeliveryDate.HasValue)
                    {
                        checkDate = record.DeliveryDate.Value.AddDays(70);
                        if (curretDate.Date <= checkDate.Value.Date)
                        {
                            // если 70 дней со дня вручения не прошло, тогда нам такое напоминание ненужно 
                            continue;
                        }
                    }
                    else if (record.DocumentDate.HasValue)
                    {
                        checkDate = record.DocumentDate.Value.AddDays(70);
                    }

                    var remId = 0L;
                    if (dictForDocument.ContainsKey(record.resolId))
                    {
                        var current = dictForDocument[record.resolId];

                        // если среди текущих элементов уже есть напоминание по данному постановлению, тогда заменяем Id чтобы произошел Update
                        remId = current.Id;

                        // и удаляем из словаря чтобы потом из БД не удалить
                        dictForDocument.Remove(record.resolId);
                        dictForReminder.Remove(remId);

                        // проверяем если никакие значения не изменились, то просто выходим из цикла
                        if (record.DocumentNumber == current.Num && checkDate == current.CheckDate && current.ContragentId == record.ContragentId && current.inspectorId == record.OfficialId)
                        {
                            continue;
                        }
                    }

                    var rem = new Reminder
                    {
                        Id = remId,
                        Actuality = true,
                        TypeReminder = TypeReminder.Resolution,
                        CategoryReminder = GetCategoryReminder(record.TypeBase),
                        Num = record.DocumentNumber,
                        CheckDate = checkDate,
                        DocumentGji = new DocumentGji { Id = record.resolId },
                        Contragent =
                            record.ContragentId > 0
                                ? new Contragent { Id = record.ContragentId }
                                : null,
                        Inspector = new Inspector { Id = record.OfficialId },
                        InspectionGji = new InspectionGji { Id = record.InspectionId }
                    };

                    listToSave.Add(rem);

                }

                using (var tr = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        

                        // удаляем те, которые уже не являются напоминалками
                        foreach (var kvp in dictForReminder)
                        {
                            reminderService.Delete(kvp.Key);
                        }

                        // создаем новые напоминалки
                        listToSave.ForEach(
                            x =>
                                {
                                    if (x.Id > 0)
                                    {
                                        reminderService.Update(x);
                                    }
                                    else
                                    {
                                        reminderService.Save(x);
                                    }
                                });

                        tr.Commit();
                        return true;
                    }
                    catch (Exception exp)
                    {
                        tr.Rollback();
                        Container.Resolve<ILogger>().LogError(exp, "Создание напоминалок с типом Постановление");
                        return false;
                    }
                }

            }
            catch (Exception exc)
            {
                throw exc;
            }
            finally
            {
                Container.Release(reminderService);
                Container.Release(resolutionService);
                Container.Release(childrenService);
                Container.Release(protocolArticleLawService);
                Container.Release(resolutionPayService);
            }
        }

        private CategoryReminder GetCategoryReminder(TypeBase typeBase)
        {
            switch (typeBase)
            {
                case TypeBase.Inspection:
                    return CategoryReminder.Inspection;
                case TypeBase.CitizenStatement:
                    return CategoryReminder.CitizenStatement;
                case TypeBase.PlanJuridicalPerson:
                    return CategoryReminder.PlanJuridicalPerson;
                case TypeBase.DisposalHead:
                    return CategoryReminder.DisposalHead;
                case TypeBase.ProsecutorsClaim:
                    return CategoryReminder.ProsecutorsClaim;
                case TypeBase.ProsecutorsResolution:
                    return CategoryReminder.ProsecutorsResolution;
                case TypeBase.ActivityTsj:
                    return CategoryReminder.ActivityTsj;
                case TypeBase.HeatingSeason:
                    return CategoryReminder.HeatingSeason;
                case TypeBase.Default:
                    return CategoryReminder.Default;
            }

            return CategoryReminder.Default;
        }
    }
}
