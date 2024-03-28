namespace Bars.GkhGji.Regions.Nso
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Nso.Entities;

    using Castle.Windsor;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Класс создания напоминания в случае если инициатором была проверка или документ ГЖИ 
    /// </summary>
    public class InspectionReminderRule : IReminderRule
    {
        public IWindsorContainer Container { get; set; }

        public string Id
        {
            get { return "InspectionReminderRule"; }
        }

        /// <summary>
        /// Данный метод создает напоминалки для НСО по типам
        /// Основание проверки, Распоряжение, Предписание, Постановление, Акт проверки, Уведомление о начале проверки
        /// </summary>
        public void Create(IEntity entity)
        {
            var servInspection = Container.Resolve<IDomainService<InspectionGji>>();
            var servReminder = Container.Resolve<IDomainService<Reminder>>();

            try
            {
                InspectionGji obj = null;

                if (entity is DocumentGji)
                {
                    obj = servInspection.Load((entity as DocumentGji).Inspection.Id);
                }
                else if (entity is InspectionGji)
                {
                    obj = entity as InspectionGji;
                }

                if (obj == null)
                {
                    return;
                }

                // Список напоминаний которые необходимо будет либо создавать либо обновлять
                var listRemindersToSave = new List<Reminder>();

                // Получаем все существующие идентификаторы по данной проверке
                var listCurrentReminder =
                    servReminder.GetAll()
                    .Where(x => x.TypeReminder == TypeReminder.ActCheck ||
                                x.TypeReminder == TypeReminder.BaseInspection ||
                                x.TypeReminder == TypeReminder.Disposal ||
                                x.TypeReminder == TypeReminder.NoticeOfInspection ||
                                x.TypeReminder == TypeReminder.Prescription ||
                                x.TypeReminder == TypeReminder.Statement)
                    .Where(x => x.InspectionGji.Id == obj.Id).ToList();

                var listRemindersToDelete =
                    servReminder.GetAll()
                                .Where(x => x.InspectionGji.Id == obj.Id)
                                .Select(x => x.Id)
                                .ToList();

                if (obj.State == null || (obj.State != null && !obj.State.FinalState))
                {

                    // 1 проверка. Получаем невалидные задачи по проверкам
                    var listInspectionNotValid = this.GetRemindersIfInspectionNotValid(obj);

                    // Если после выполнения 1го правила ест ькакието записи, то дальше смысла проверять нет
                    if (listInspectionNotValid.Any())
                    {
                        listRemindersToSave.AddRange(listInspectionNotValid);
                    }
                    else
                    {
                        // 2 проверка. Получаем невалидные предписания
                        var listPrescriptionsNotValid = this.GetRemindersIfPrescriptionNotValid(obj);
                        if (listPrescriptionsNotValid.Any())
                        {
                            listRemindersToSave.AddRange(listPrescriptionsNotValid);
                        }

                        // 3 проверка. Получаем невалидные распоряжения
                        var listDisposalsNotValid = this.GetRemindersIfDisposalNotValid(obj);
                        if (listDisposalsNotValid.Any())
                        {
                            listRemindersToSave.AddRange(listDisposalsNotValid);
                        }

                        // 4 проверка. Получаем невалидные уведомления на проверку у которых проблемы 
                        var listNoticeNotValid = GetRemindersIfNoticeOfInspection(obj);
                        if (listNoticeNotValid.Any())
                        {
                            listRemindersToSave.AddRange(listNoticeNotValid);
                        }
                    }

                    // Получаем невалидные Основания проверок у которых проблемы с Актами
                    var listActsNotValid = this.GetRemindersIfActCheckNoValid(obj);
                    if (listActsNotValid.Any())
                    {
                        listRemindersToSave.AddRange(listActsNotValid);
                    }

                    //проходим по списку существующих Напоминаний и смотрим какие изних изменились а какие нет
                    foreach (var curr in listCurrentReminder)
                    {
                        var newReminder =
                            listRemindersToSave.FirstOrDefault(
                                x =>
                                x.TypeReminder == curr.TypeReminder && x.Inspector.Id == curr.Inspector.Id
                                && x.CategoryReminder == curr.CategoryReminder && x.Num == curr.Num
                                && x.CheckDate == curr.CheckDate
                                && (curr.AppealCits != null
                                        ? (x.AppealCits != null && x.AppealCits.Id == curr.AppealCits.Id)
                                        : x.AppealCits == null)
                                && (curr.InspectionGji != null
                                        ? (x.InspectionGji != null
                                            && x.InspectionGji.Id == curr.InspectionGji.Id)
                                        : x.InspectionGji == null)
                                && (curr.DocumentGji != null
                                        ? (x.DocumentGji != null && x.DocumentGji.Id == curr.DocumentGji.Id)
                                        : x.DocumentGji == null)
                                && (curr.Contragent != null
                                        ? (x.Contragent != null && x.Contragent.Id == curr.Contragent.Id)
                                        : x.Contragent == null));

                        // Если текущий элемент и тот которых хотят добавить равны то убираем элемен и из сохранения и из добавления
                        // то есть тот, который уже есть в БД самый актуальный
                        if (newReminder != null)
                        {
                            listRemindersToSave.Remove(newReminder);
                            listRemindersToDelete.Remove(curr.Id);
                        }
                    }
                }

                // Удаляем, то что уже не актуально
                foreach (var id in listRemindersToDelete)
                {
                    servReminder.Delete(id);
                }

                // Записи в этом списке только на добавление поскольку пред этим мы уже удалили, то что было раньше
                foreach (var item in listRemindersToSave)
                {
                    servReminder.Save(item);
                }
                
            }
            finally
            {
                Container.Release(servInspection);
                Container.Release(servReminder);
            }
        }

        /// <summary>
        /// Метод получения напоминалок, если выполняется следующее правило:
        ///       Если у предписания нет дочернего Распоряжения
        ///       то необходимо создать напоминание для каждого инспектора в предписании
        /// </summary>
        private List<Reminder> GetRemindersIfPrescriptionNotValid(InspectionGji obj)
        {
            var servDocumentGjiInspectors = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var servPrescriptionGjiViolation = Container.Resolve<IDomainService<PrescriptionViol>>();
            var servPrescription = Container.Resolve<IDomainService<Prescription>>();
            var servDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            
            try
            {
                var result = new List<Reminder>();

                // Формируем словарь инспекторов для Предписаний
                // Который будем использовать в дальнейшем 
                var dictInspectors = servDocumentGjiInspectors.GetAll()
                                             .Where(x => x.DocumentGji.Inspection.Id == obj.Id)
                                             .Where(x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Prescription)
                                             .Where(x => x.Inspector != null)
                                             .Select(x => new
                                             {
                                                 x.Id,
                                                 DocumentId = x.DocumentGji.Id,
                                                 InspectorId = x.Inspector.Id
                                             })
                                             .AsEnumerable()
                                             .GroupBy(x => x.DocumentId)
                                             .ToDictionary(x => x.Key, y => y.Select(x => x.InspectorId).Distinct().ToList());

                // Если нет инспекторов значит нет сысла проверять Предписания
                if (!dictInspectors.Any())
                    return result;

                // Формируем словарь нарушений для каждого Предписания проверки чтобы получать максимальную дату по полю 'Срок устранения'
                var prescriptionViolDict = servPrescriptionGjiViolation.GetAll()
                                    .Where(x => x.Document.Inspection.Id == obj.Id)
                                    .GroupBy(x => x.Document.Id)
                                    .ToDictionary(x => x.Key, y => y.AsEnumerable().Max(x => x.DatePlanRemoval));

                // Теперь получаем предписания у которых нет дочерних элементов Распоряжения
                var prescriptions = servPrescription.GetAll()
                                .Where(x => x.Inspection.Id == obj.Id)
                                .Where(x => !servDocumentGjiChildren.GetAll()
                                                .Any(y => y.Parent.Id == x.Id
                                                            && y.Children.TypeDocumentGji == TypeDocumentGji.Disposal))
                                .Select(x => new
                                {
                                    x.Id,
                                    ContragentId = x.Inspection.Contragent != null ? x.Inspection.Contragent.Id : 0,
                                    InspectionId = x.Inspection.Id,
                                    x.DocumentNumber,
                                    x.DocumentDate,
                                    x.Inspection.TypeBase
                                })
                                .ToList();

                // Теперь для каждого предписания получаем список инспекторов и формируем для них напоминания
                foreach (var pres in prescriptions)
                {
                    if (!dictInspectors.ContainsKey(pres.Id))
                        continue;

                    var di = dictInspectors[pres.Id];

                    foreach (var inspectorId in di.Distinct())
                    {
                        var rem = new Reminder
                        {
                            Actuality = true,
                            TypeReminder = TypeReminder.Prescription,
                            CategoryReminder = GetCategoryReminder(pres.TypeBase),
                            Num = pres.DocumentNumber,
                            CheckDate = prescriptionViolDict.ContainsKey(pres.Id) ? prescriptionViolDict[pres.Id] : null,
                            DocumentGji = new DocumentGji { Id = pres.Id },
                            Contragent = pres.ContragentId > 0 ? new Contragent { Id = pres.ContragentId } : null,
                            Inspector = new Inspector { Id = inspectorId },
                            InspectionGji = obj
                        };

                        result.Add(rem);
                    }
                }

                return result;
            }
            finally
            {
                Container.Release(servDocumentGjiInspectors);
                Container.Release(servPrescriptionGjiViolation);
                Container.Release(servPrescription);
                Container.Release(servDocumentGjiChildren);
            }
        }


        /// <summary>
        /// Метод получения напоминалок, если выполняется следующее правило:
        ///       Если проверка имеет статус НеЗакрыто и если нет дочерних докуметов
        ///       то необходимо создат ьнапоминание для каждого инспектора из основания проверки
        /// </summary>
        private List<Reminder> GetRemindersIfInspectionNotValid(InspectionGji obj)
        {
            var servDocumentGji = Container.Resolve<IDomainService<DocumentGji>>();
            var servInspectionGjiInspectors = Container.Resolve<IDomainService<InspectionGjiInspector>>();
            var servBaseDispHead = Container.Resolve<IDomainService<BaseDispHead>>();

            try
            {
                var result = new List<Reminder>();

                if (!servDocumentGji.GetAll().Any(x => x.Inspection.Id == obj.Id))
                {
                    var inspectionInspectors = servInspectionGjiInspectors.GetAll()
                                                   .Where(x => x.Inspection.Id == obj.Id)
                                                   .Select(x => x.Inspector.Id)
                                                   .ToList();

                    // Если это ктому же проверка по поручению руководства,
                    // то необходимо забрать Руководителя как инспектора
                    if (obj.TypeBase == TypeBase.DisposalHead)
                    {
                        var dispHead = servBaseDispHead.Load(obj.Id);
                        if (dispHead != null && dispHead.Head != null && !inspectionInspectors.Contains(dispHead.Head.Id))
                        {
                            inspectionInspectors.Add(dispHead.Head.Id);
                        }
                    }

                    foreach (var id in inspectionInspectors.Distinct())
                    {
                        var rem = new Reminder
                        {
                            Actuality = true,
                            TypeReminder = TypeReminder.BaseInspection,
                            CategoryReminder = GetCategoryReminder(obj.TypeBase),
                            Num = obj.InspectionNumber,
                            InspectionGji = obj,
                            Contragent = obj.Contragent,
                            Inspector = new Inspector { Id = id }
                        };

                        result.Add(rem);
                    }
                }

                return result;
            }
            finally
            {
                Container.Release(servDocumentGji);
                Container.Release(servInspectionGjiInspectors);
                Container.Release(servBaseDispHead);
            }
        }

        /// <summary>
        /// проверка:
        ///   Если существует Плановая проверка, либо проверка по обращению, но нет Акта, либо есть Акт у которго в поле Нарушения выявлены = Не задано
        /// </summary>
        private List<Reminder> GetRemindersIfActCheckNoValid(InspectionGji obj)
        {
            var servJurPersonInspection = Container.Resolve<IDomainService<BaseJurPerson>>();
            var servInspection = Container.Resolve<IDomainService<InspectionGji>>();
            var servInspectionGjiInspectors = Container.Resolve<IDomainService<InspectionGjiInspector>>();
            var servInspectionAppCits = Container.Resolve<IDomainService<InspectionAppealCits>>();
            var servDocument = Container.Resolve<IDomainService<DocumentGji>>();
            var servActCheckRo = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var servAppealCitsExecutant = Container.Resolve<IDomainService<AppealCitsExecutant>>();
            
            try
            {
                var result = new List<Reminder>();

                if (obj.TypeBase == TypeBase.PlanJuridicalPerson || obj.TypeBase == TypeBase.CitizenStatement)
                {
                    // если у такой проверки нет Акта, либо есть Акт у которго в поле Нарушения выявлены = Не задано
                    if ( !servDocument.GetAll()
                                     .Any(
                                         y => y.Inspection.Id == obj.Id && y.TypeDocumentGji == TypeDocumentGji.ActCheck)
                        || (servDocument.GetAll()
                                        .Any(
                                            y =>
                                            y.Inspection.Id == obj.Id && y.TypeDocumentGji == TypeDocumentGji.ActCheck)
                            && servActCheckRo.GetAll()
                                             .Any(
                                                 y =>
                                                 y.ActCheck.Inspection.Id == obj.Id
                                                 && y.HaveViolation == YesNoNotSet.NotSet)))
                    {
                        // специально пишу так чтобы люди незапутались с восклицательными знаками
                        // Если условие выполнилось то значит мы считаем такую проверку невалидной
                        // иначе выходим из метода
                    }
                    else
                    {
                        return result;
                    }

                    var inspectionInspectors = new List<long>();
                    var deadLine = DateTime.MinValue;

                    if (obj.TypeBase == TypeBase.PlanJuridicalPerson)
                    {
                        // если это плановая проверка юр лица то получаем из Инспекторов указанных в поле Инспекторы
                        inspectionInspectors.AddRange(servInspectionGjiInspectors.GetAll()
                                                  .Where(x => x.Inspection.Id == obj.Id)
                                                  .Select(x => x.Inspector.Id)
                                                  .ToList());

                        // Берем дату начала првоерки
                        var jurPers =
                            servJurPersonInspection.GetAll()
                                                   .Where(x => x.Id == obj.Id)
                                                   .Select(x => x.DateStart)
                                                   .FirstOrDefault();

                        if (jurPers.HasValue)
                        {
                            deadLine = jurPers.Value;
                        }
                    }
                    else if (obj.TypeBase == TypeBase.CitizenStatement)
                    {
                        // если это проверка по обращению и посколкьу проверка может быт ьпо многим обращениям
                        // то тогда в качестве инспекторов берем Поля указанные в обращении

                        var data = servAppealCitsExecutant.GetAll()
                                        .Where(x => servInspectionAppCits.GetAll()
                                                 .Any(y => y.AppealCits.Id == x.AppealCits.Id && y.Inspection.Id == obj.Id))
                                        .Select(
                                            x =>
                                            new
                                                {
                                                    ExecutantId = x.Executant.Id,
                                                    x.AppealCits.DateFrom
                                                })
                                        .ToList();

                        foreach (var item in data)
                        {
                            if (!inspectionInspectors.Contains(item.ExecutantId))
                            {
                                inspectionInspectors.Add(item.ExecutantId);
                            }
                        }

                        if (data.Any(x => x.DateFrom.HasValue))
                        {
                            deadLine = data.Where(x => x.DateFrom.HasValue).Min(x => x.DateFrom.Value);    
                        }
                    }

                    foreach (var id in inspectionInspectors.Distinct())
                    {
                        var rem = new Reminder
                        {
                            Actuality = true,
                            TypeReminder = TypeReminder.ActCheck,
                            CategoryReminder = GetCategoryReminder(obj.TypeBase),
                            Num = obj.InspectionNumber,
                            InspectionGji = obj,
                            CheckDate = AddBusinessDays(deadLine, 58), // Добавляем к дате 58 рабочих дней
                            Contragent = obj.Contragent,
                            Inspector = new Inspector { Id = id }
                        };

                        result.Add(rem);
                    }

                }

                return result;
            }
            finally 
            {
                Container.Release(servInspection);
                Container.Release(servDocument);
                Container.Release(servJurPersonInspection);
                Container.Release(servInspectionAppCits);
                Container.Release(servActCheckRo);
                Container.Release(servInspectionGjiInspectors);
                Container.Release(servAppealCitsExecutant);
            }
        }

        /// <summary>
        /// Метод получения напоминалок, если выполняется следующее правило:
        ///       1) Если у распоряжения нет дочернего Распоряжения
        ///       2) Если невалидны дочерние АктыПроверок (Тут много условий поэтому читать дальше отдельно)
        ///       то необходимо создать напоминание для каждого инспектора в предписании
        /// </summary>
        private List<Reminder> GetRemindersIfDisposalNotValid(InspectionGji obj)
        {
            var servDocumentGjiInspectors = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var servDisposal = Container.Resolve<IDomainService<Disposal>>();
            try
            {
                var result = new List<Reminder>();

                // Формируем словарь инспекторов для Распоряжений
                // Который будем использовать в дальнейшем 
                var dictInspectors = servDocumentGjiInspectors.GetAll()
                    .Where(x => x.DocumentGji.Inspection.Id == obj.Id)
                    .Where(x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Disposal)
                    .Where(x => x.Inspector != null)
                    .Select(x => new
                    {
                        x.Id,
                        DocumentId = x.DocumentGji.Id,
                        InspectorId = x.Inspector.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.DocumentId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.InspectorId).Distinct().ToList());

                // Получаем все распоряжения этой проверки в словарь
                // поскольку в дальнейшем придется доставать из них данные
                var dictDisposals = servDisposal.GetAll()
                    .Where(x => x.Inspection.Id == obj.Id)
                    .Select(x => new DisposalForReminder
                    {
                        Id = x.Id,
                        ContragentId = x.Inspection.Contragent != null ? x.Inspection.Contragent.Id : 0,
                        InspectionId = x.Inspection.Id,
                        DocumentNumber = x.DocumentNumber,
                        DateEnd = x.DateEnd,
                        TypeDisposal = x.TypeDisposal,
                        TypeBase = x.Inspection.TypeBase,
                        IssuedDisposalId = x.IssuedDisposal != null ? x.IssuedDisposal.Id : 0,
                        ResponsibleExecutionId = x.ResponsibleExecution != null ? x.ResponsibleExecution.Id : 0
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.First());

                // В этом списке будут все идентификаторы Распоряжений которые по каким либо правилам считаются не валидными
                var listIds = new List<long>();

                // 1 проверка. Получаем Идентификаторы Распоряжений у которых нет дочерних документов
                var listHaveNotChildrenDocuments = this.GetDisposalHaveNotChildrenDocuments(obj);
                if (listHaveNotChildrenDocuments.Any())
                {
                    listIds.AddRange(listHaveNotChildrenDocuments);
                }

                // 2 проверка. Получаем распоряжения исходя из того что дочерние Акты заполнены неверно
                var listDisposalsWhereActsNotValid = this.GetDisposalWhereActsNotValid(obj);
                if (listDisposalsWhereActsNotValid.Any())
                {
                    listIds.AddRange(listDisposalsWhereActsNotValid);
                }

                // После того как получили все некорректные распоряжения создаем напоминания
                foreach (var id in listIds.Distinct())
                {
                    if (!dictDisposals.ContainsKey(id))
                        continue;

                    var disp = dictDisposals[id];

                    if (!dictInspectors.ContainsKey(disp.Id))
                    {
                        dictInspectors.Add(disp.Id, new List<long>());
                    }

                    var di = dictInspectors[disp.Id];

                    if (disp.IssuedDisposalId > 0 && !di.Contains(disp.IssuedDisposalId))
                    {
                        di.Add(disp.IssuedDisposalId);
                    }

                    if (disp.ResponsibleExecutionId > 0 && !di.Contains(disp.ResponsibleExecutionId))
                    {
                        di.Add(disp.ResponsibleExecutionId);
                    }

                    foreach (var inspectorId in di.Distinct())
                    {
                        var rem = new Reminder
                        {
                            Actuality = true,
                            TypeReminder = TypeReminder.Disposal,
                            CategoryReminder = GetCategoryReminder(disp.TypeBase),
                            Num = disp.DocumentNumber,
                            CheckDate = disp.DateEnd != DateTime.MinValue ? disp.DateEnd : null,
                            DocumentGji = new DocumentGji { Id = disp.Id },
                            Contragent = disp.ContragentId > 0 ? new Contragent { Id = disp.ContragentId } : null,
                            Inspector = new Inspector { Id = inspectorId },
                            InspectionGji = new InspectionGji { Id = disp.InspectionId }
                        };

                        result.Add(rem);
                    }
                }
                return result;
            }
            finally
            {
                Container.Release(servDocumentGjiInspectors);
                Container.Release(servDisposal);
            }
        }

        /// <summary>
        /// Уведомление о начале проверки которые хреново заполнены
        /// </summary>
        private List<Reminder> GetRemindersIfNoticeOfInspection(InspectionGji obj)
        {
            var servDocumentGjiInspectors = Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var servNsoDisposal = Container.Resolve<IDomainService<NsoDisposal>>();
            var servBaseJurPerson = Container.Resolve<IDomainService<BaseJurPerson>>();

            try
            {
                var result = new List<Reminder>();

                BaseJurPerson jurPerson = null; 

                if (obj.TypeBase == TypeBase.PlanJuridicalPerson)
                {
                    jurPerson = servBaseJurPerson.GetAll().FirstOrDefault(x => x.Id == obj.Id);
                }

                var query = servNsoDisposal.GetAll()
                            .Where(x => x.Inspection.Id == obj.Id)
                            .Where(x => x.NcSent == YesNo.No);

                var data = query
                    .Select(x => new DisposalForReminder
                    {
                        Id = x.Id,
                        ContragentId = x.Inspection.Contragent != null ? x.Inspection.Contragent.Id : 0,
                        InspectionId = x.Inspection.Id,
                        DocumentNumber = x.DocumentNumber,
                        DateStart = x.DateStart,
                        TypeDisposal = x.TypeDisposal,
                        TypeBase = x.Inspection.TypeBase,
                        IssuedDisposalId = x.IssuedDisposal != null ? x.IssuedDisposal.Id : 0,
                        ResponsibleExecutionId = x.ResponsibleExecution != null ? x.ResponsibleExecution.Id : 0
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.First());

                if (data.Any())
                {
                    // Формируем словарь инспекторов для Распоряжений
                    // Который будем использовать в дальнейшем 
                    var dictInspectors = servDocumentGjiInspectors.GetAll()
                        .Where(x => query.Any(y => y.Inspection.Id == x.DocumentGji.Inspection.Id))
                        .Where(x => x.Inspector != null)
                        .Select(x => new
                        {
                            x.Id,
                            DocumentId = x.DocumentGji.Id,
                            InspectorId = x.Inspector.Id
                        })
                        .AsEnumerable()
                        .GroupBy(x => x.DocumentId)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.InspectorId).Distinct().ToList());

                    foreach (var kvp in data)
                    {
                        if (!dictInspectors.ContainsKey(kvp.Key))
                        {
                            dictInspectors.Add(kvp.Key, new List<long>());
                        }

                        var di = dictInspectors[kvp.Key];

                        if (kvp.Value.IssuedDisposalId > 0 && !di.Contains(kvp.Value.IssuedDisposalId))
                        {
                            di.Add(kvp.Value.IssuedDisposalId);
                        }

                        if (kvp.Value.ResponsibleExecutionId > 0 && !di.Contains(kvp.Value.ResponsibleExecutionId))
                        {
                            di.Add(kvp.Value.ResponsibleExecutionId);
                        }

                        foreach (var inspectorId in di.Distinct())
                        {
                            // Если проверка = Плановая проверка Юр лица то отнимаем 3 рабочих дня от даты Начала проверки (которая в основании)
                            // Иначе отнимаем 1 раб. день

                            var cntWorkDays = 1;
                            var dateStart = kvp.Value.DateStart;
                            if (jurPerson != null && jurPerson.DateStart.HasValue)
                            {
                                dateStart = jurPerson.DateStart;
                                cntWorkDays = 3;
                            }
                            
                            var rem = new Reminder
                            {
                                Actuality = true,
                                TypeReminder = TypeReminder.NoticeOfInspection,
                                CategoryReminder = GetCategoryReminder(kvp.Value.TypeBase),
                                Num = kvp.Value.DocumentNumber,
                                CheckDate = this.RemoveBusinessDays(dateStart.HasValue ? dateStart.Value : DateTime.Today, cntWorkDays),
                                DocumentGji = new DocumentGji { Id = kvp.Value.Id },
                                Contragent = kvp.Value.ContragentId > 0 ? new Contragent { Id = kvp.Value.ContragentId } : null,
                                Inspector = new Inspector { Id = inspectorId },
                                InspectionGji = new InspectionGji { Id = kvp.Value.InspectionId }
                            };

                            result.Add(rem);
                        }
                    }

                }

                return result;
            }
            finally 
            {
                Container.Release(servDocumentGjiInspectors);
                Container.Release(servNsoDisposal);
                Container.Release(servBaseJurPerson);
            }
        }

        /// <summary>
        /// Метод получения Идентификаторов Распоряжений у которых нет дочерних документов 
        /// </summary>
        private List<long> GetDisposalHaveNotChildrenDocuments(InspectionGji obj)
        {
            var servDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var servDisposal = Container.Resolve<IDomainService<Disposal>>();
            try
            {
                // Если у распоряжения нет дочерних документов, то показываем распоряжение в напоминание распоряжение
                // Для этого сначала получаем те распоряжения у которых есть дочерние записи, а затем оставляем тех которые ими не являются
                // Так сделано потому что если документа 
                return servDisposal.GetAll()
                    .Where(x => x.Inspection.Id == obj.Id)
                    .Where(x => !servDocumentGjiChildren.GetAll().Any(y => y.Parent.Id == x.Id))
                    .Select(x => x.Id)
                    .ToList();
            }
            finally
            {
                Container.Release(servDocumentGjiChildren);
                Container.Release(servDisposal);
            }
        }

        /// <summary>
        /// Метод получения Идентификаторов Распоряжений у которых неправильно заполнены Акты проверок 
        /// </summary>
        private List<long> GetDisposalWhereActsNotValid(InspectionGji obj)
        {
            var servActCheckRO = Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var servActCheck = Container.Resolve<IDomainService<ActCheck>>();
            var servActCheckViol = Container.Resolve<IDomainService<ActCheckViolation>>();
            var servActRemoval = Container.Resolve<IDomainService<ActRemoval>>();
            var servActRemvalViol = Container.Resolve<IDomainService<ActRemovalViolation>>();
            var servDocumentGjiChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var servPrescriptionGjiViolation = Container.Resolve<IDomainService<PrescriptionViol>>();

            try
            {
                var result = new List<long>();
                // Тут будет список невалидных Актов
                var actCheckNotValidIds = new List<long>();

                // Получаем дома акта
                var listActCheckRO = servActCheckRO.GetAll()
                                  .Where(x => x.ActCheck.Inspection.Id == obj.Id)
                                  .Select(x => new { x.Id, ActCheckId = x.ActCheck.Id, x.HaveViolation })
                                  .ToList();

                // 1 проверка. Если у АктаПроверки признак НарушенияВыявлены == НеЗадано
                // То считаем что Акт невалидный
                actCheckNotValidIds.AddRange(listActCheckRO.Where(x => x.HaveViolation == YesNoNotSet.NotSet)
                                            .Select(x => x.ActCheckId).Distinct().ToList());

                // 2 проверка. Если у АктаПроверки признак НарушенияВыявлены = Да
                // И при этом не либо нарушений = 0, либо невсе нарушения ест ьв предписаниях
                // то считаем что АктПроверки не валидный

                // Идентификаторы АктовПроверки с типом устранения нарушения = НеЗадано
                var listActCheckYes = listActCheckRO.Where(x => x.HaveViolation == YesNoNotSet.Yes)
                                            .Select(x => x.ActCheckId).Distinct().ToList();


                // Если такие акты есть с типом НарушенияВыявлены = Да
                // то проверяем есть ли среди них такие у которых либо 0 нарушений, либо не на все нарушения есть предписания
                if (listActCheckYes.Any())
                {

                    // Получаем акты у которых 0 нарушений 
                    actCheckNotValidIds.AddRange(servActCheck.GetAll()
                                    .Where(x => listActCheckYes.Contains(x.Id))
                                    .Where(x => !servActCheckViol.GetAll().Any(y => y.ActObject.ActCheck.Id == x.Id))
                                    .Select(x => x.Id)
                                    .ToList());

                    // Теперь получаем Акты в которых хотябы одно нарушение нет в Предписании
                    actCheckNotValidIds.AddRange(servActCheckViol.GetAll()
                                        .Where(x => listActCheckYes.Contains(x.Document.Id))
                                        .Where(x => !servPrescriptionGjiViolation.GetAll()
                                                        .Any(y => y.InspectionViolation.Id == x.InspectionViolation.Id))
                                        .Select(x => x.Document.Id)
                                        .Distinct()
                                        .ToList());
                }

                // 3. Если неправильно заполнены АктыПроверкиПредписаний
                var listActRemovalsNotValid = new List<long>();

                var listActRemovals = servActRemoval.GetAll()
                                  .Where(x => x.Inspection.Id == obj.Id)
                                  .Select(x => new { x.Id, x.TypeRemoval })
                                  .ToList();

                if (listActRemovals.Any())
                {

                    // 3.1 Если признак НарушенияУстранены = НеЗадано, то считаем что документ неправильный
                    listActRemovalsNotValid.AddRange(listActRemovals.Where(x => x.TypeRemoval == YesNoNotSet.NotSet).Select(x => x.Id).ToList());

                    // 3.2 Если признак НарушенияУстранеы = Да но при этом хотябы одно нарушение 
                    // не имеет дату Факт Утсранения, то считаем документ невернум
                    var listActRemoovalYes = listActRemovals.Where(x => x.TypeRemoval == YesNoNotSet.Yes)
                                       .Select(x => x.Id)
                                       .ToList();

                    // Если есть акты с типом Нарушения Устранены = Да, то все нарушения должны быть
                    // с датамы Факт Устранения иначе неправильно
                    if (listActRemoovalYes.Any())
                    {
                        // Данной выборкой получаем Акты у которых Признак = Да но приэтом есть хотябы одно нарушение без ДатыФактУстранения
                        listActRemovalsNotValid.AddRange(servActRemvalViol.GetAll()
                                             .Where(x => listActRemoovalYes.Contains(x.Document.Id))
                                             .Where(x => !(x.DateFactRemoval.HasValue || x.InspectionViolation.DateFactRemoval.HasValue))
                                             .Select(x => x.Document.Id)
                                             .Distinct()
                                             .ToList());

                    }

                    // Если признак НарушенияУстранеы = Нет но при этом хотябы одно нарушение не имеет дату Факт Утсранения, то считаем документ невернум
                    var listActRemovalNo = listActRemovals.Where(x => x.TypeRemoval == YesNoNotSet.No)
                                       .Select(x => x.Id)
                                       .ToList();

                    if (listActRemovalNo.Any())
                    {

                        // 3.3 Если признак НарушенияУстранены = Нет, но при этом все нарушения заполнены верно,
                        // то считаем что Акт заполнен неверно
                        listActRemovalsNotValid.AddRange(servActRemoval.GetAll()
                                          .Where(x => x.Inspection.Id == obj.Id)
                                          .Where(x => x.TypeRemoval == YesNoNotSet.No)
                                          .Where(x => !servActRemvalViol.GetAll()
                                                    .Where(y => y.Document.Id == x.Id)
                                                    .Any(y => !y.DateFactRemoval.HasValue || !y.InspectionViolation.DateFactRemoval.HasValue))
                                          .Select(x => x.Id)
                                          .ToList());

                        // 3.4. Если для Акта с типом НарушенияУстранены = Нет и если нарушения без даты ФактУстранения,
                        // то необходимо чтобы для такого нарушения было предписание

                        // Сначала получаем словарь Нарушений у которых нет даты фактического устранения
                        var dictViolActRemovals = servActRemvalViol.GetAll()
                                .Where(x => listActRemovalNo.Contains(x.Document.Id))
                                .Where(x => !x.DateFactRemoval.HasValue && !x.InspectionViolation.DateFactRemoval.HasValue)
                                .Select(x => new { DocId = x.Document.Id, ViolId = x.InspectionViolation.Id })
                                .AsEnumerable()
                                .GroupBy(x => x.DocId)
                                .ToDictionary(x => x.Key, y => y.Select(z => z.ViolId).ToList());

                        if (dictViolActRemovals.Any())
                        {

                            var listActChildrens = servDocumentGjiChildren.GetAll()
                                    .Where(y => listActRemovalNo.Contains(y.Parent.Id)
                                                    && y.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                                    .Select(x => new { ParentId = x.Parent.Id, ChildrenId = x.Children.Id })
                                    .ToList();

                            var dictViolPrescriptions = servPrescriptionGjiViolation.GetAll()
                                .Where(x => x.Document.Inspection.Id == obj.Id)
                                .Where(x => servDocumentGjiChildren.GetAll().Any(y => listActRemovalNo.Contains(y.Parent.Id) && y.Children.TypeDocumentGji == TypeDocumentGji.Prescription))
                                .Select(x => new { DocId = x.Document.Id, ViolId = x.InspectionViolation.Id })
                                .AsEnumerable()
                                .GroupBy(x => x.DocId)
                                .ToDictionary(x => x.Key, y => y.Select(z => z.ViolId).ToList());


                            foreach (var kvp in dictViolActRemovals)
                            {
                                var docId = kvp.Key;

                                // получаем идентификаторы дочерних документов предписаний
                                var prescriptionIds = listActChildrens.Where(x => x.ParentId == docId).Select(x => x.ChildrenId).ToList();

                                // получаем нарушения которые имеются в дочернем предписаниий АктаПроверкиПредписания
                                var prescriptionViols = dictViolPrescriptions.Where(x => prescriptionIds.Contains(x.Key))
                                                       .Select(x => x.Value)
                                                       .ToList();

                                // получаем Все ли нарушения ест ьв нарушениях дочерних предписаний
                                var isValid = kvp.Value.All(viol => prescriptionViols.Any(x => x.Contains(viol)));

                                if (!isValid)
                                {
                                    listActRemovalsNotValid.Add(docId);
                                }
                            }
                        }
                    }

                    // Поскольку АктПроверкиПредписания является дочерним для АктаПроврки, то
                    // Получив список неправильных АктовПроверкиПредписания необходимо получить родительские АктыПроверки
                    if (listActRemovalsNotValid.Any())
                    {
                        actCheckNotValidIds.AddRange(servDocumentGjiChildren.GetAll()
                                           .Where(x => x.Parent.Inspection.Id == obj.Id)
                                           .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                                           .Where(x => listActRemovalsNotValid.Contains(x.Children.Id))
                                           .Select(x => x.Parent.Id)
                                           .Distinct()
                                           .ToList());
                    }
                }

                // После того как получили АктыПроверки, котоыре по какойто причине НеВалидные
                // Необходимо из этих актов получить родительские предписания 
                if (actCheckNotValidIds.Any())
                {
                    // Получаем распоряжения из которых создан Акт проверки
                    result.AddRange(servDocumentGjiChildren.GetAll()
                                           .Where(x => x.Parent.Inspection.Id == obj.Id)
                                           .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                                           .Where(x => actCheckNotValidIds.Contains(x.Children.Id))
                                           .Select(x => x.Parent.Id)
                                           .Distinct()
                                           .ToList());
                }

                return result;
            }
            finally
            {
                Container.Release(servActCheckRO);
                Container.Release(servActCheck);
                Container.Release(servActCheckViol);
                Container.Release(servActRemoval);
                Container.Release(servActRemvalViol);
                Container.Release(servDocumentGjiChildren);
                Container.Release(servPrescriptionGjiViolation);
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

        /// <summary>
        /// Мтод добавления к дате сколько-то рабочих дней. Отсекая субботы и воскресенья
        /// </summary>
        private DateTime AddBusinessDays(DateTime date, int days)
        {
            if (days == 0) return date;

            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(2);
                days -= 1;
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
                days -= 1;
            }

            date = date.AddDays(days / 5 * 7);
            int extraDays = days % 5;

            if ((int)date.DayOfWeek + extraDays > 5)
            {
                extraDays += 2;
            }

            return date.AddDays(extraDays);
        }

        /// <summary>
        /// Мтод убирающий количество дней без учета субботы и воскресенья
        /// </summary>
        private DateTime RemoveBusinessDays(DateTime date, int days)
        {
            if (days == 0) return date;

            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                date = date.AddDays(2);
            }
            else if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                date = date.AddDays(1);
            }

            while (days > 0)
            {
                date = date.AddDays(-1);

                if ( date.DayOfWeek != DayOfWeek.Sunday && date.DayOfWeek != DayOfWeek.Saturday )
                {
                    --days;
                }
            }

            return date;
        }

        private class DisposalForReminder
        {
            public long Id { get; set; }
            public long ContragentId { get; set; }
            public long InspectionId { get; set; }
            public DateTime? DateEnd { get; set; }
            public DateTime? DateStart { get; set; }
            public string DocumentNumber { get; set; }
            public TypeDisposalGji TypeDisposal { get; set; }
            public TypeBase TypeBase { get; set; }
            public long IssuedDisposalId { get; set; }
            public long ResponsibleExecutionId { get; set; }
        }
    }
}
