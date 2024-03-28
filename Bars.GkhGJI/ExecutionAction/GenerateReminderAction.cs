namespace Bars.GkhGji.ExecutionAction
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class GenerateReminderAction : BaseExecutionAction
    {
        /// <summary>
        /// Статический код регистрации.
        /// </summary>
        /// <summary>
        /// IoC контейнер.
        /// </summary>
        /// <summary>
        /// Код для регистрации.
        /// </summary>
        /// <summary>
        /// Описание действия.
        /// </summary>
        public override string Description => "Генерация напоминаний по действиям ГЖИ";

        /// <summary>
        /// Название для отображения.
        /// </summary>
        public override string Name => "Генерация напоминаний по действиям ГЖИ";

        /// <summary>
        /// Действие.
        /// </summary>
        public override Func<IDataResult> Action => this.GenerateReminders;

        /// <summary>
        /// Метод действия.
        /// </summary>
        /// <returns>
        /// The <see cref="BaseDataResult" />.
        /// </returns>
        public BaseDataResult GenerateReminders()
        {
            var servAppealCits = this.Container.ResolveDomain<AppealCits>();
            var servDisposal = this.Container.ResolveDomain<Disposal>();
            var servReminder = this.Container.ResolveDomain<Reminder>();
            var servInspection = this.Container.ResolveDomain<InspectionGji>();
            var servBaseDispHead = this.Container.ResolveDomain<BaseDispHead>();
            var servInspectionGjiInspectors = this.Container.ResolveDomain<InspectionGjiInspector>();
            var servDocumentGji = this.Container.ResolveDomain<DocumentGji>();
            var servPrescriptionGjiViolation = this.Container.ResolveDomain<PrescriptionViol>();
            var servDocumentGjiChildren = this.Container.ResolveDomain<DocumentGjiChildren>();
            var servPrescription = this.Container.ResolveDomain<Prescription>();
            var servDocumentGjiInspectors = this.Container.ResolveDomain<DocumentGjiInspector>();
            var servActCheck = this.Container.ResolveDomain<ActCheck>();
            var servActCheckRo = this.Container.ResolveDomain<ActCheckRealityObject>();
            var servActCheckViol = this.Container.ResolveDomain<ActCheckViolation>();
            var servActRemoval = this.Container.ResolveDomain<ActRemoval>();
            var servActRemvalViol = this.Container.ResolveDomain<ActRemovalViolation>();

            try
            {
                var reminders = new List<Reminder>();

                //Обращения граждан только те, у которых статус не на Закрыто
                var appealCitsIds =
                    servAppealCits.GetAll()
                        .Where(x => !x.State.FinalState)
                        .Where(x => x.Tester != null || x.Surety != null || x.Executant != null)
                        .Select(x => new {x.Id, x.NumberGji, x.CheckTime, x.Surety, x.Executant, x.Tester})
                        .ToList();

                var listdInspectors = new List<long>();

                foreach (var obj in appealCitsIds)
                {
                    listdInspectors.Clear();

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

                    foreach (var idIns in listdInspectors)
                    {
                        var rem = new Reminder
                        {
                            Actuality = true,
                            TypeReminder = TypeReminder.Statement,
                            CategoryReminder = CategoryReminder.ExecutionStatemen,
                            Num = obj.NumberGji,
                            CheckDate = obj.CheckTime != DateTime.MinValue ? obj.CheckTime : null,
                            AppealCits = new AppealCits {Id = obj.Id},
                            Inspector = new Inspector {Id = idIns}
                        };

                        reminders.Add(rem);
                    }
                }

                //Получаем для проверок Руководителей отдельно по полю Руководитель и добавляем его в словарь по Основанию
                var dictInspectionInspectors =
                    servInspectionGjiInspectors.GetAll()
                        .Where(x => !x.Inspection.State.FinalState && x.Inspector != null)
                        .Where(x => !servDocumentGji.GetAll().Any(y => y.Inspection.Id == x.Inspection.Id))
                        .Select(
                            x =>
                                new
                                {
                                    x.Id,
                                    InspectionId = x.Inspection != null ? x.Inspection.Id : 0,
                                    InspectorId = x.Inspector != null ? x.Inspector.Id : 0
                                })
                        .AsEnumerable()
                        .GroupBy(x => x.InspectionId)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.InspectorId).Distinct().ToList());

                //Получаем инспеторов для проверок руководителей поскольку там помимо инспеторов есть также и поле Руководитель
                var inspectorsDispHead =
                    servBaseDispHead.GetAll()
                        .Where(x => !x.State.FinalState)
                        .Where(x => !servDocumentGji.GetAll().Any(y => y.Inspection.Id == x.Id))
                        .Where(x => x.Head != null)
                        .Select(x => new {x.Id, HeadId = x.Head.Id})
                        .ToList();

                foreach (var item in inspectorsDispHead)
                {
                    List<long> dict;
                    if (!dictInspectionInspectors.TryGetValue(item.Id, out dict))
                    {
                        dict = new List<long>();
                        dictInspectionInspectors.Add(item.Id, dict);
                    }

                    if (!dict.Contains(item.HeadId))
                    {
                        dict.Add(item.HeadId);
                    }
                }

                //Получаем пустые проверки (На которых нет документов)
                var baseInspectionIds =
                    servInspection.GetAll()
                        .Where(x => !x.State.FinalState)
                        .Where(x => !servDocumentGji.GetAll().Any(y => y.Inspection.Id == x.Id))
                        .Select(
                            x =>
                                new
                                {
                                    x.Id,
                                    ContragentId = x.Contragent != null ? x.Contragent.Id : 0,
                                    x.InspectionNumber,
                                    x.TypeBase
                                })
                        .ToList();

                //Проходим по всем проверкам
                foreach (var insp in baseInspectionIds)
                {
                    //Для каждой проверки, создаем списки инспекторов и создаем напоминания
                    if (dictInspectionInspectors.ContainsKey(insp.Id))
                    {
                        foreach (var insId in dictInspectionInspectors[insp.Id])
                        {
                            var rem = new Reminder
                            {
                                Actuality = true,
                                TypeReminder = TypeReminder.BaseInspection,
                                CategoryReminder = this.GetCategoryReminder(insp.TypeBase),
                                Num = insp.InspectionNumber,
                                InspectionGji = new InspectionGji {Id = insp.Id},
                                Contragent =
                                    insp.ContragentId > 0
                                        ? new Contragent {Id = insp.ContragentId}
                                        : null,
                                Inspector = new Inspector {Id = insId}
                            };

                            reminders.Add(rem);
                        }
                    }
                }

                //Формируем для всех распоряжений словари, по которым потом будем проверять
                var docInspectors =
                    servDocumentGjiInspectors.GetAll()
                        .Where(x => !x.DocumentGji.Inspection.State.FinalState)
                        .Where(
                            x =>
                                x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Disposal
                                    || x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Prescription)
                        .Where(x => x.Inspector != null)
                        .Select(x => new {x.Id, DocumentId = x.DocumentGji.Id, InspectorId = x.Inspector.Id})
                        .AsEnumerable()
                        .GroupBy(x => x.DocumentId)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.InspectorId).Distinct().ToList());

                var prescriptionViolDict =
                    servPrescriptionGjiViolation.GetAll()
                        .Where(x => !x.Document.Inspection.State.FinalState)
                        .GroupBy(x => x.Document.Id)
                        .ToDictionary(x => x.Key, y => y.AsEnumerable().Max(x => x.DatePlanRemoval));

                //Предписания
                var prescriptions =
                    servPrescription.GetAll()
                        .Where(x => x.Inspection != null && !x.Inspection.State.FinalState)
                        .Where(
                            x =>
                                !servDocumentGjiChildren.GetAll()
                                    .Any(
                                        y => y.Parent.Id == x.Id && y.Children.TypeDocumentGji == TypeDocumentGji.Disposal))
                        .Select(
                            x =>
                                new
                                {
                                    x.Id,
                                    ContragentId = x.Inspection.Contragent != null ? x.Inspection.Contragent.Id : 0,
                                    InspectionId = x.Inspection.Id,
                                    x.DocumentNumber,
                                    x.DocumentDate,
                                    x.Inspection.TypeBase
                                })
                        .ToList();

                foreach (var pres in prescriptions)
                {
                    var di = docInspectors[pres.Id];

                    foreach (var inspectorId in di)
                    {
                        var rem = new Reminder
                        {
                            Actuality = true,
                            TypeReminder = TypeReminder.Prescription,
                            CategoryReminder = this.GetCategoryReminder(pres.TypeBase),
                            Num = pres.DocumentNumber,
                            CheckDate =
                                prescriptionViolDict.ContainsKey(pres.Id)
                                    ? prescriptionViolDict[pres.Id]
                                    : null,
                            DocumentGji = new DocumentGji {Id = pres.Id},
                            Contragent =
                                pres.ContragentId > 0
                                    ? new Contragent {Id = pres.ContragentId}
                                    : null,
                            Inspector = new Inspector {Id = inspectorId},
                            InspectionGji = new InspectionGji {Id = pres.InspectionId}
                        };

                        reminders.Add(rem);
                    }
                }

                var disposalsForSave = new List<DisposalForReminder>();

                // Получаем все распоряжения этой проверки в словарь
                // поскольку в дальнейшем придется доставать из них данные
                var dictDisposals =
                    servDisposal.GetAll()
                        .Where(x => !x.Inspection.State.FinalState)
                        .Select(
                            x =>
                                new DisposalForReminder
                                {
                                    Id = x.Id,
                                    ContragentId =
                                        x.Inspection.Contragent != null
                                            ? x.Inspection.Contragent.Id
                                            : 0,
                                    InspectionId = x.Inspection.Id,
                                    DocumentNumber = x.DocumentNumber,
                                    DateEnd = x.DateEnd,
                                    TypeDisposal = x.TypeDisposal,
                                    TypeBase = x.Inspection.TypeBase,
                                    IssuedDisposalId =
                                        x.IssuedDisposal != null ? x.IssuedDisposal.Id : 0,
                                    ResponsibleExecutionId =
                                        x.ResponsibleExecution != null ? x.ResponsibleExecution.Id : 0
                                })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.First());

                var disposalIds = dictDisposals.Select(x => x.Key).ToList();

                // 3 условие Если у распоряжения нет дочерних документов, то показываем распоряжение в напоминание распоряжение
                // Для этого сначала получаем те распоряжения у которых есть дочерние записи, а затем оставляем тех которые ими не являются
                var listDisposalsWithChildrens =
                    servDocumentGjiChildren.GetAll()
                        .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                        .Where(x => !x.Parent.Inspection.State.FinalState)
                        .Select(x => x.Parent.Id)
                        .Distinct()
                        .ToList();

                foreach (var id in disposalIds)
                {
                    if (listDisposalsWithChildrens.Contains(id))
                    {
                        continue;
                    }

                    var disp = dictDisposals[id];
                    if (!disposalsForSave.Contains(disp))
                    {
                        disposalsForSave.Add(disp);
                    }
                }

                // 4 Если у акта РезультатПроверки == НеЗадано
                // То не обходимо получить родительское Распоряжение и добавить его к напоминанию
                // для этого сначала получаем идентификаторы актов удовлетворяющих условию
                var listActCheckRo =
                    servActCheckRo.GetAll()
                        .Where(x => !x.ActCheck.Inspection.State.FinalState)
                        .Select(x => new {x.Id, ActCheckId = x.ActCheck.Id, x.HaveViolation})
                        .ToList();

                // Идентификаторы актов с типом устранения нарушения = Незадано
                var actCheckNotValidIds =
                    listActCheckRo.Where(x => x.HaveViolation == YesNoNotSet.NotSet)
                        .Select(x => x.ActCheckId)
                        .Distinct()
                        .ToList();

                // 5. Если у Акта проверки признак НарушенияВыявлены = Да
                // То проверяем на все ли нарушения есть проедписание

                // Идентификаторы актов с типом устранения нарушения = Незадано
                var listActCheckYes =
                    listActCheckRo.Where(x => x.HaveViolation == YesNoNotSet.Yes)
                        .Select(x => x.ActCheckId)
                        .Distinct()
                        .ToList();

                // Если такие акты есть с типом НарушенияВыявлены = Да
                // то проверяем есть ли среди них такие у которых либо 0 нарушений, либо не на все нарушения есть предписания
                if (listActCheckYes.Any())
                {
                    // Получаем акты у которых 0 нарушений 
                    actCheckNotValidIds.AddRange(
                        servActCheck.GetAll()
                            .Where(x => !x.Inspection.State.FinalState)
                            .Where(
                                x =>
                                    servActCheckRo.GetAll()
                                        .Where(y => !y.ActCheck.Inspection.State.FinalState)
                                        .Where(y => y.HaveViolation == YesNoNotSet.Yes)
                                        .Any(y => y.ActCheck.Id == x.Id))
                            .Where(
                                x =>
                                    !servActCheckViol.GetAll()
                                        .Where(y => !y.ActObject.ActCheck.Inspection.State.FinalState)
                                        .Any(y => y.ActObject.ActCheck.Id == x.Id))
                            .Select(x => x.Id)
                            .ToList());

                    // Теперь получаем те нарушения акта на которых нет предписания
                    actCheckNotValidIds.AddRange(
                        servActCheckViol.GetAll()
                            .Where(x => !x.ActObject.ActCheck.Inspection.State.FinalState)
                            .Where(
                                x =>
                                    !servPrescriptionGjiViolation.GetAll()
                                        .Any(y => y.InspectionViolation.Id == x.InspectionViolation.Id))
                            .Select(x => x.Document.Id)
                            .Distinct()
                            .ToList());
                }

                // 6. Если у акта проверки предписания (Акт Устранения) признак НарушенияУстранены = Да
                // но при этом есть хотябы одно нарушение без даты Факт устранения то показываем Распоряженни
                var listActRemovalsNotValid = new List<long>();

                var listActRemovals =
                    servActRemoval.GetAll()
                        .Where(x => !x.Inspection.State.FinalState)
                        .Select(x => new {x.Id, x.TypeRemoval})
                        .ToList();

                if (listActRemovals.Any())
                {
                    // Если признак НарушенияУстранены = НеЗадано, то считаем что документ неправильный
                    listActRemovalsNotValid.AddRange(
                        listActRemovals.Where(x => x.TypeRemoval == YesNoNotSet.NotSet).Select(x => x.Id).ToList());

                    var listActRemoovalYes =
                        listActRemovals.Where(x => x.TypeRemoval == YesNoNotSet.Yes).Select(x => x.Id).ToList();

                    // Если есть акты с типом Нарушения Устранены = Да то все нарушения должны быть с датами Факт устранения иначе неправильно
                    listActRemovalsNotValid.AddRange(
                        servActRemvalViol.GetAll()
                            .Where(x => !(x.DateFactRemoval.HasValue || x.InspectionViolation.DateFactRemoval.HasValue))
                            .Select(x => x.Document.Id)
                            .AsEnumerable()
                            .Where(x => listActRemoovalYes.Contains(x))
                            .Distinct()
                            .ToList());

                    // Если признак НарушенияУстранеы = Нет но при этом хотябы одно нарушение не имеет дату Факт Утсранения, то считаем документ невернум
                    var listActRemovalNo =
                        listActRemovals.Where(x => x.TypeRemoval == YesNoNotSet.No).Select(x => x.Id).ToList();

                    if (listActRemovalNo.Any())
                    {
                        // Если НарушенияУстранены = Нет , но при этом все нарушения заполнены верно, то
                        // добавляем в список невалидных записей
                        listActRemovalsNotValid.AddRange(
                            servActRemoval.GetAll()
                                .Where(x => !x.Inspection.State.FinalState)
                                .Where(x => x.TypeRemoval == YesNoNotSet.No)
                                .Where(
                                    x =>
                                        !servActRemvalViol.GetAll()
                                            .Where(y => y.Document.Id == x.Id)
                                            .Any(
                                                y =>
                                                    !y.DateFactRemoval.HasValue
                                                        || !y.InspectionViolation.DateFactRemoval.HasValue))
                                .Select(x => x.Id)
                                .ToList());

                        var dictViolActRemovals =
                            servActRemvalViol.GetAll()
                                .Where(
                                    x => !x.DateFactRemoval.HasValue && !x.InspectionViolation.DateFactRemoval.HasValue)
                                .Select(x => new {DocId = x.Document.Id, ViolId = x.InspectionViolation.Id})
                                .AsEnumerable()
                                .Where(x => listActRemovalNo.Contains(x.DocId))
                                .GroupBy(x => x.DocId)
                                .ToDictionary(x => x.Key, y => y.Select(z => z.ViolId).ToList());

                        if (dictViolActRemovals.Any())
                        {
                            var listActChildrens =
                                servDocumentGjiChildren.GetAll()
                                    .Where(y => y.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                                    .Select(x => new {ParentId = x.Parent.Id, ChildrenId = x.Children.Id})
                                    .AsEnumerable()
                                    .Where(y => listActRemovalNo.Contains(y.ParentId))
                                    .ToList();

                            // Изза того что более 100 элементов нельяз передавать в Contains
                            // приходится сначала получить славарь 
                            var dictActRemovalNoChildrens =
                                servDocumentGjiChildren.GetAll()
                                    .Where(y => y.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                                    .Select(y => new {ParentId = y.Parent.Id, ChildrenId = y.Children.Id})
                                    .AsEnumerable()
                                    .Where(y => listActRemovalNo.Contains(y.ParentId))
                                    .GroupBy(y => y.ParentId)
                                    .ToDictionary(x => x.Key, y => y.Any());

                            var dictViolPrescriptions =
                                servPrescriptionGjiViolation.GetAll()
                                    .Where(x => !x.Document.Inspection.State.FinalState)
                                    .Select(x => new {DocId = x.Document.Id, ViolId = x.InspectionViolation.Id})
                                    .AsEnumerable()
                                    .Where(x => dictActRemovalNoChildrens.ContainsKey(x.DocId))
                                    .GroupBy(x => x.DocId)
                                    .ToDictionary(x => x.Key, y => y.Select(z => z.ViolId).ToList());

                            foreach (var kvp in dictViolActRemovals)
                            {
                                var docId = kvp.Key;

                                // получаем идентификаторы дочерних документов предписаний
                                var prescriptionIds =
                                    listActChildrens.Where(x => x.ParentId == docId).Select(x => x.ChildrenId).ToList();

                                // получаем нарушения которые имеются в дочернем предписаниий АктаПроверкиПредписания
                                var prescriptionViols =
                                    dictViolPrescriptions.Where(x => prescriptionIds.Contains(x.Key))
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

                    // Получив список неправильных АктовПроверкиПредписания необходимо получить родительские Акты
                    // Получаем распоряжение для Актов у которых НарушенияВыявлены = НеЗадано
                    if (listActRemovalsNotValid.Any())
                    {
                        var listNotValid =
                            servDocumentGjiChildren.GetAll()
                                .Where(x => !x.Parent.Inspection.State.FinalState)
                                .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                                .Select(x => new {ParentId = x.Parent.Id, ChildrenId = x.Children.Id})
                                .AsEnumerable()
                                .Where(x => listActRemovalsNotValid.Contains(x.ChildrenId))
                                .Select(x => x.ParentId)
                                .Distinct()
                                .ToList();

                        foreach (var item in listNotValid)
                        {
                            if (!actCheckNotValidIds.Contains(item))
                            {
                                actCheckNotValidIds.Add(item);
                            }
                        }
                    }
                }

                // Если такие акты есть с типом НарушенияВыявлены = НеЗаданно 
                // то находим родительские распоряжения
                if (actCheckNotValidIds.Any())
                {
                    // Получаем распоряжение для Актов у которых НарушенияВыявлены = НеЗадано
                    var listDisposalsForActNotSet =
                        servDocumentGjiChildren.GetAll()
                            .Where(x => !x.Parent.Inspection.State.FinalState)
                            .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                            .Select(x => new {ParentId = x.Parent.Id, ChildrenId = x.Children.Id})
                            .AsEnumerable()
                            .Where(x => actCheckNotValidIds.Contains(x.ChildrenId))
                            .Select(x => x.ParentId)
                            .Distinct()
                            .ToList();

                    foreach (var id in listDisposalsForActNotSet)
                    {
                        var disp = dictDisposals[id];
                        if (!disposalsForSave.Contains(disp))
                        {
                            disposalsForSave.Add(disp);
                        }
                    }
                }

                // После того какполучили все некорректные распоряжения создаем напоминания
                foreach (var disp in disposalsForSave)
                {
                    if (!docInspectors.ContainsKey(disp.Id))
                    {
                        docInspectors.Add(disp.Id, new List<long>());
                    }

                    var di = docInspectors[disp.Id];

                    if (disp.IssuedDisposalId > 0 && !di.Contains(disp.IssuedDisposalId))
                    {
                        di.Add(disp.IssuedDisposalId);
                    }

                    if (disp.ResponsibleExecutionId > 0 && !di.Contains(disp.ResponsibleExecutionId))
                    {
                        di.Add(disp.ResponsibleExecutionId);
                    }

                    foreach (var inspectorId in di)
                    {
                        var rem = new Reminder
                        {
                            Actuality = true,
                            TypeReminder = TypeReminder.Disposal,
                            CategoryReminder = this.GetCategoryReminder(disp.TypeBase),
                            Num = disp.DocumentNumber,
                            CheckDate = disp.DateEnd != DateTime.MinValue ? disp.DateEnd : null,
                            DocumentGji = new DocumentGji {Id = disp.Id},
                            Contragent =
                                disp.ContragentId > 0
                                    ? new Contragent {Id = disp.ContragentId}
                                    : null,
                            Inspector = new Inspector {Id = inspectorId},
                            InspectionGji = new InspectionGji {Id = disp.InspectionId}
                        };

                        reminders.Add(rem);
                    }
                }

                TransactionHelper.InsertInManyTransactions(this.Container, reminders);

                return new BaseDataResult {Success = true, Message = "Генерация прошла успешно!"};
            }

            finally
            {
                this.Container.Release(servReminder);
                this.Container.Release(servDisposal);
                this.Container.Release(servBaseDispHead);
                this.Container.Release(servInspectionGjiInspectors);
                this.Container.Release(servDocumentGji);
                this.Container.Release(servPrescriptionGjiViolation);
                this.Container.Release(servDocumentGjiChildren);
                this.Container.Release(servPrescription);
                this.Container.Release(servDocumentGjiInspectors);
                this.Container.Release(servActCheckRo);
                this.Container.Release(servActCheck);
                this.Container.Release(servActCheckViol);
                this.Container.Release(servActRemoval);
                this.Container.Release(servActRemvalViol);
                this.Container.Release(servInspection);
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

        protected class DisposalForReminder
        {
            public long Id { get; set; }

            public long ContragentId { get; set; }

            public long InspectionId { get; set; }

            public DateTime? DateEnd { get; set; }

            public string DocumentNumber { get; set; }

            public TypeDisposalGji TypeDisposal { get; set; }

            public TypeBase TypeBase { get; set; }

            public long IssuedDisposalId { get; set; }

            public long ResponsibleExecutionId { get; set; }
        }
    }
}