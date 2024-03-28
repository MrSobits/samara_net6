namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using B4;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Contracts.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class GjiScriptService : IGjiScriptService
    {
        public IWindsorContainer Container { get; set; }

        /// <summary>
        ////Проставление адрессов обращениям из проверки созданной на основе этого обращения
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public BaseDataResult SetAddressAppeal(BaseParams baseParams)
        {
            try
            {
                var result = new List<AppealCitsRealityObject>();

                var service = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();

                // Получаем обращения граждан без адреса
                var appealCitsWithoutAddress =
                    this.Container.Resolve<IDomainService<AppealCits>>()
                        .GetAll()
                        .Where(x => !service.GetAll().Any(y => y.AppealCits.Id == x.Id))
                        .Select(x => x.Id)
                        .ToList();

                // Места возникновения проблемы в проверке
                var inspectionRealObj =
                    this.Container.Resolve<IDomainService<InspectionGjiRealityObject>>()
                        .GetAll()
                        .Where(x => x.Inspection.TypeBase == TypeBase.CitizenStatement)
                        .GroupBy(x => x.Inspection.Id)
                        .ToDictionary(x => x.Key, y => y.AsEnumerable());

                // Получаем связки обращение + основания проверки, созданные на 1 обращение
                var baseStatementAppealCitsDict =
                    this.Container.Resolve<IDomainService<InspectionAppealCits>>()
                        .GetAll()
                        .GroupBy(x => x.Inspection.Id)
                        .ToDictionary(x => x.Key, y => y.AsEnumerable())
                        .Where(x => x.Value.Count() == 1)
                        .ToDictionary(x => x.Key, y => y.Value.First());

                // Список уникальных пар
                var listUnique = new List<string>();

                // бежим по всем связкам
                foreach (var kvp in baseStatementAppealCitsDict)
                {
                    // 1. Если среди них есть обращения без адреса
                    // 2. У проверок по обращению есть дома
                    // 3. Эти дома все по одному МО
                    if (appealCitsWithoutAddress.Contains(kvp.Value.AppealCits.Id) && inspectionRealObj.ContainsKey(kvp.Key) && inspectionRealObj[kvp.Key].Select(x => x.RealityObject.Municipality.Id).Distinct().Count() == 1)
                    {
                        // Для каждого дома создаем Место возникновения проблемы для данного обращения
                        foreach (var insp in inspectionRealObj[kvp.Key])
                        {
                            // Проверяем уникальность
                            var key = string.Format("{0}_{1}", kvp.Value.AppealCits.Id.ToStr(), insp.RealityObject.Id);
                            if (listUnique.Contains(key))
                            {
                                continue;
                            }

                            listUnique.Add(key);

                            // Сохраняем
                            result.Add(new AppealCitsRealityObject
                            {
                                Id = 0,
                                AppealCits = new AppealCits { Id = kvp.Value.AppealCits.Id },
                                RealityObject = new RealityObject { Id = insp.RealityObject.Id }
                            });
                        }
                    }
                }

                InTransaction(result, service);


                return new BaseDataResult { Success = true, Message = string.Format("Сохранено {0} объектов", result.Count()) };
            }
            catch (Exception exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
        }

        /// <summary>
        /// Генерация фэйковых данных для панели руководителя
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public BaseDataResult ReminderGenerateFake(BaseParams baseParams)
        {
            var servAppealCits = this.Container.Resolve<IDomainService<AppealCits>>();
            var servDisposal = Container.Resolve<IDomainService<Disposal>>();
            var servReminder = Container.Resolve<IDomainService<Reminder>>();
            var servInspection = Container.Resolve<IDomainService<InspectionGji>>();
            var servBaseDispHead = Container.Resolve<IDomainService<BaseDispHead>>();
            var servInspectionGjiInspectors = Container.Resolve<IDomainService<InspectionGjiInspector>>();
            var servDocumentGji = this.Container.Resolve<IDomainService<DocumentGji>>();
            var servPrescriptionGjiViolation = this.Container.Resolve<IDomainService<PrescriptionViol>>();
            var servDocumentGjiChildren = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var servPrescription = Container.Resolve<IDomainService<Prescription>>();
            var servDocumentGjiInspectors = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var servActCheck = this.Container.Resolve<IDomainService<ActCheck>>();
            var servActCheckRO = this.Container.Resolve<IDomainService<ActCheckRealityObject>>();
            var servActCheckViol = this.Container.Resolve<IDomainService<ActCheckViolation>>();
            var servActRemoval = this.Container.Resolve<IDomainService<ActRemoval>>();
            var servActRemvalViol = this.Container.Resolve<IDomainService<ActRemovalViolation>>();

            var reminders = new List<Reminder>();

            //Обращения граждан только те, у которых статус не на Закрыто
            var appealCitsIds = servAppealCits.GetAll()
                .Where(x => !x.State.FinalState)
                .Where(x => x.Tester != null || x.Surety != null || x.Executant != null)
                .Select(x => new
                {
                    x.Id,
                    x.NumberGji,
                    x.CheckTime,
                    x.Surety,
                    x.Executant,
                    x.Tester
                })
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
                        AppealCits = new AppealCits { Id = obj.Id },
                        Inspector = new Inspector { Id = idIns }
                    };

                    reminders.Add(rem);
                }
            }

            //Получаем для проверок Руководителей отдельно по полю Руководитель и добавляем его в словарь по Основанию
            var dictInspectionInspectors = servInspectionGjiInspectors.GetAll()
                .Where(x => !x.Inspection.State.FinalState && x.Inspector != null)
                .Where(x => !servDocumentGji.GetAll().Any(y => y.Inspection.Id == x.Inspection.Id))
                .Select(x => new
                                 {
                                     x.Id,
                                     InspectionId = x.Inspection != null ? x.Inspection.Id : 0,
                                     InspectorId = x.Inspector != null ? x.Inspector.Id : 0
                                 })
                .AsEnumerable()
                .GroupBy(x => x.InspectionId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.InspectorId).Distinct().ToList());
             
            //Получаем инспеторов для проверок руководителей поскольку там помимо инспеторов есть также и поле Руководитель
            var inspectorsDispHead = servBaseDispHead.GetAll()
                .Where(x => !x.State.FinalState)
                .Where(x => !servDocumentGji.GetAll().Any(y => y.Inspection.Id == x.Id))
                .Where(x => x.Head != null)
                .Select(x => new
                                 {
                                     x.Id, 
                                     HeadId = x.Head.Id
                                 })
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
            var baseInspectionIds = servInspection.GetAll()
                .Where(x => !x.State.FinalState)
                .Where(x => !servDocumentGji.GetAll().Any(y => y.Inspection.Id == x.Id))
                .Select(x => new
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
                            CategoryReminder = GetCategoryReminder(insp.TypeBase),
                            Num = insp.InspectionNumber,
                            InspectionGji = new InspectionGji { Id = insp.Id },
                            Contragent = insp.ContragentId > 0 ? new Contragent { Id = insp.ContragentId } : null,
                            Inspector = new Inspector { Id = insId }
                        };

                        reminders.Add(rem);
                    }
                }
            }

            //Формируем для всех распоряжений словари, по которым потом будем проверять
            var docInspectors = servDocumentGjiInspectors.GetAll()
                                         .Where(x => !x.DocumentGji.Inspection.State.FinalState)
                                         .Where(x => x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Disposal
                                             || x.DocumentGji.TypeDocumentGji == TypeDocumentGji.Prescription)
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

            var prescriptionViolDict = servPrescriptionGjiViolation.GetAll()
                            .Where(x => !x.Document.Inspection.State.FinalState)
                            .GroupBy(x => x.Document.Id)
                            .ToDictionary(x => x.Key, y => y.AsEnumerable().Max(x => x.DatePlanRemoval));

            //Предписания
            var prescriptions = servPrescription.GetAll()
                            .Where(x => x.Inspection != null && !x.Inspection.State.FinalState)
                            .Where(x => !servDocumentGjiChildren.GetAll().Any(y => y.Parent.Id == x.Id && y.Children.TypeDocumentGji == TypeDocumentGji.Disposal))
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

            foreach (var pres in prescriptions)
            {
                var di = docInspectors[pres.Id];

                foreach (var inspectorId in di)
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
                        InspectionGji = new InspectionGji { Id = pres.InspectionId }
                    };

                    reminders.Add(rem);
                }
            }

            var disposalsForSave = new List<DisposalForReminder>();

            // Получаем все распоряжения этой проверки в словарь
            // поскольку в дальнейшем придется доставать из них данные
            var dictDisposals = servDisposal.GetAll()
                            .Where(x => !x.Inspection.State.FinalState)
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

            var disposalIds = dictDisposals.Select(x => x.Key).ToList();

            // 3 условие Если у распоряжения нет дочерних документов, то показываем распоряжение в напоминание распоряжение
            // Для этого сначала получаем те распоряжения у которых есть дочерние записи, а затем оставляем тех которые ими не являются
            var listDisposalsWithChildrens = servDocumentGjiChildren.GetAll()
                                       .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                                       .Where(x => !x.Parent.Inspection.State.FinalState)
                                       .Select(x => x.Parent.Id)
                                       .Distinct()
                                       .ToList();

            foreach (var id in disposalIds)
            {
                if (listDisposalsWithChildrens.Contains(id))
                    continue;

                var disp = dictDisposals[id];
                if (!disposalsForSave.Contains(disp))
                {
                    disposalsForSave.Add(disp);
                }
            }

            // 4 Если у акта РезультатПроверки == НеЗадано
            // То не обходимо получить родительское Распоряжение и добавить его к напоминанию
            // для этого сначала получаем идентификаторы актов удовлетворяющих условию
            var listActCheckRO = servActCheckRO.GetAll()
                                .Where(x => !x.ActCheck.Inspection.State.FinalState)
                                .Select(x => new { x.Id, ActCheckId = x.ActCheck.Id, x.HaveViolation })
                                .ToList();

            // Идентификаторы актов с типом устранения нарушения = Незадано
            var actCheckNotValidIds = listActCheckRO.Where(x => x.HaveViolation == YesNoNotSet.NotSet)
                                        .Select(x => x.ActCheckId)
                                        .Distinct()
                                        .ToList();

            // 5. Если у Акта проверки признак НарушенияВыявлены = Да
            // То проверяем на все ли нарушения есть проедписание

            // Идентификаторы актов с типом устранения нарушения = Незадано
            var listActCheckYes = listActCheckRO.Where(x => x.HaveViolation == YesNoNotSet.Yes)
                                        .Select(x => x.ActCheckId)
                                        .Distinct()
                                        .ToList();

            // Если такие акты есть с типом НарушенияВыявлены = Да
            // то проверяем есть ли среди них такие у которых либо 0 нарушений, либо не на все нарушения есть предписания
            if (listActCheckYes.Any())
            {

                // Получаем акты у которых 0 нарушений 
                actCheckNotValidIds.AddRange(servActCheck.GetAll()
                                .Where(x => !x.Inspection.State.FinalState)
                                .Where(x => servActCheckRO.GetAll()
                                                          .Where(y => !y.ActCheck.Inspection.State.FinalState)
                                                          .Where(y => y.HaveViolation == YesNoNotSet.Yes)
                                                          .Any(y => y.ActCheck.Id == x.Id))
                                .Where(x => !servActCheckViol.GetAll()
                                                .Where(y => !y.ActObject.ActCheck.Inspection.State.FinalState)
                                                .Any(y => y.ActObject.ActCheck.Id == x.Id))
                                .Select(x => x.Id)
                                .ToList());

                // Теперь получаем те нарушения акта на которых нет предписания
                actCheckNotValidIds.AddRange(servActCheckViol.GetAll()
                                    .Where(x => !x.ActObject.ActCheck.Inspection.State.FinalState)
                                    .Where(x => !servPrescriptionGjiViolation.GetAll()
                                                    .Any(y => y.InspectionViolation.Id == x.InspectionViolation.Id))
                                    .Select(x => x.Document.Id)
                                    .Distinct()
                                    .ToList());

            }

            // 6. Если у акта проверки предписания (Акт Устранения) признак НарушенияУстранены = Да
            // но при этом есть хотябы одно нарушение без даты Факт устранения то показываем Распоряженни
            var listActRemovalsNotValid = new List<long>();

            var listActRemovals = servActRemoval.GetAll()
                              .Where(x => !x.Inspection.State.FinalState)
                              .Select(x => new { x.Id, x.TypeRemoval })
                              .ToList();

            if (listActRemovals.Any())
            {
                // Если признак НарушенияУстранены = НеЗадано, то считаем что документ неправильный
                listActRemovalsNotValid.AddRange(listActRemovals.Where(x => x.TypeRemoval == YesNoNotSet.NotSet).Select(x => x.Id).ToList());

                var listActRemoovalYes = listActRemovals.Where(x => x.TypeRemoval == YesNoNotSet.Yes)
                                                   .Select(x => x.Id)
                                                   .ToList();

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
                var listActRemovalNo = listActRemovals.Where(x => x.TypeRemoval == YesNoNotSet.No)
                                   .Select(x => x.Id)
                                   .ToList();

                if (listActRemovalNo.Any())
                {

                    // Если НарушенияУстранены = Нет , но при этом все нарушения заполнены верно, то
                    // добавляем в список невалидных записей
                    listActRemovalsNotValid.AddRange(
                        servActRemoval.GetAll()
                                      .Where(x => !x.Inspection.State.FinalState)
                                      .Where(x => x.TypeRemoval == YesNoNotSet.No)
                                      .Where(
                                          x => !servActRemvalViol.GetAll()
                                                .Where(y => y.Document.Id == x.Id)
                                                .Any(y => !y.DateFactRemoval.HasValue || !y.InspectionViolation.DateFactRemoval.HasValue))
                                      .Select(x => x.Id)
                                      .ToList());


                    var dictViolActRemovals = servActRemvalViol.GetAll()
                            .Where(x => !x.DateFactRemoval.HasValue && !x.InspectionViolation.DateFactRemoval.HasValue)
                            .Select(x => new { DocId = x.Document.Id, ViolId = x.InspectionViolation.Id })
                            .AsEnumerable()
                            .Where(x => listActRemovalNo.Contains(x.DocId))
                            .GroupBy(x => x.DocId)
                            .ToDictionary(x => x.Key, y => y.Select(z => z.ViolId).ToList());

                    if (dictViolActRemovals.Any())
                    {
                        var listActChildrens = servDocumentGjiChildren.GetAll()
                                                   .Where(y => y.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                                                   .Select(x => new { ParentId = x.Parent.Id, ChildrenId = x.Children.Id })
                                                   .AsEnumerable()
                                                   .Where(y => listActRemovalNo.Contains(y.ParentId))
                                                   .ToList();

                        // Изза того что более 100 элементов нельяз передавать в Contains
                        // приходится сначала получить славарь 
                        var dictActRemovalNoChildrens = servDocumentGjiChildren.GetAll()
                                                   .Where(y => y.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                                                   .Select(y => new { ParentId = y.Parent.Id, ChildrenId = y.Children.Id })
                                                   .AsEnumerable()
                                                   .Where(y => listActRemovalNo.Contains(y.ParentId))
                                                   .GroupBy(y => y.ParentId)
                                                   .ToDictionary(x => x.Key, y => y.Any());
                        

                        var dictViolPrescriptions = servPrescriptionGjiViolation.GetAll()
                            .Where(x => !x.Document.Inspection.State.FinalState)
                            .Select(x => new { DocId = x.Document.Id, ViolId = x.InspectionViolation.Id })
                            .AsEnumerable()
                            .Where(x => dictActRemovalNoChildrens.ContainsKey(x.DocId))
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

                // Получив список неправильных АктовПроверкиПредписания необходимо получить родительские Акты
                // Получаем распоряжение для Актов у которых НарушенияВыявлены = НеЗадано
                if (listActRemovalsNotValid.Any())
                {
                    var listNotValid = servDocumentGjiChildren.GetAll()
                                       .Where(x => !x.Parent.Inspection.State.FinalState)
                                       .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.ActCheck)
                                       .Select(x => new { ParentId = x.Parent.Id, ChildrenId = x.Children.Id})
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
                var listDisposalsForActNotSet = servDocumentGjiChildren.GetAll()
                                        .Where(x => !x.Parent.Inspection.State.FinalState)
                                        .Where(x => x.Parent.TypeDocumentGji == TypeDocumentGji.Disposal)
                                        .Select(x => new { ParentId =x.Parent.Id, ChildrenId = x.Children.Id })
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

            disposalsForSave = FilterDisposalsForSave(disposalsForSave);

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
                        CategoryReminder = GetCategoryReminder(disp.TypeBase),
                        Num = disp.DocumentNumber,
                        CheckDate = disp.DateEnd != DateTime.MinValue ? disp.DateEnd : null,
                        DocumentGji = new DocumentGji { Id = disp.Id },
                        Contragent = disp.ContragentId > 0 ? new Contragent { Id = disp.ContragentId } : null,
                        Inspector = new Inspector { Id = inspectorId },
                        InspectionGji = new InspectionGji { Id = disp.InspectionId }
                    };

                    reminders.Add(rem);
                }
            }

            var i = 0;
            var listTransaction = new List<Reminder>();
            var cnt = reminders.Count();
            foreach (var reminder in reminders)
            {
                i++;
                listTransaction.Add(reminder);

                //через транзакцию по 1000 объектов
                if ((i % 2000 == 0) || i >= cnt)
                {
                    InTransaction(listTransaction, servReminder);
                    listTransaction = new List<Reminder>();
                }
            }

            Container.Release(servReminder);
            Container.Release(servDisposal);
            Container.Release(servBaseDispHead);
            Container.Release(servInspectionGjiInspectors);
            Container.Release(servDocumentGji);
            Container.Release(servPrescriptionGjiViolation);
            Container.Release(servDocumentGjiChildren);
            Container.Release(servPrescription);
            Container.Release(servDocumentGjiInspectors);
            Container.Release(servActCheckRO);
            Container.Release(servActCheck);
            Container.Release(servActCheckViol);
            Container.Release(servActRemoval);
            Container.Release(servActRemvalViol);
            Container.Release(servInspection);

            return new BaseDataResult { Success = true, Message = "Генерация прошла успешно!" };
        }
        protected virtual List<DisposalForReminder> FilterDisposalsForSave(List<DisposalForReminder> disposalsForSave)
        {
            return disposalsForSave;
        }
        
        /// <summary>
        /// Проставление ЗЖИ в обращения
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public BaseDataResult SetZonalInspection(BaseParams baseParams)
        {
            var serviceAppealCits = this.Container.Resolve<IDomainService<AppealCits>>();
            var serviceZonalInspectionMunicipality = this.Container.Resolve<IDomainService<ZonalInspectionMunicipality>>();
            var serviceZonalInspection = this.Container.Resolve<IDomainService<ZonalInspection>>();
            var serviceAppealCitsRealityObject = this.Container.Resolve<IDomainService<AppealCitsRealityObject>>();

            try
            {
                var listResult = new List<AppealCits>();
                var zonalInspMunicipDict =
                    serviceZonalInspectionMunicipality
                        .GetAll()
                        .GroupBy(x => x.Municipality.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.ZonalInspection.Id).AsEnumerable());

                // 1.Получаем все обращения с пустой зжи
                var appealCits =
                    serviceAppealCits.GetAll().Where(x => x.ZonalInspection == null).AsEnumerable();

                var zonalInspDict =
                    serviceZonalInspection.GetAll().GroupBy(x => x.Id).ToDictionary(x => x.Key, y => y.FirstOrDefault());

                // 2.Получаем по обращениям их места
                var appealCitPlaces =
                    serviceAppealCitsRealityObject
                        .GetAll()
                        .GroupBy(x => x.AppealCits.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.RealityObject.Municipality.Id).AsEnumerable());

                foreach (var appeal in appealCits)
                {
                    if (appealCitPlaces.ContainsKey(appeal.Id))
                    {
                        // Если количество уникальных мест 1
                        var countDistinct = appealCitPlaces[appeal.Id].Distinct().Count();
                        if (countDistinct == 1)
                        {
                            var municipalityId = appealCitPlaces[appeal.Id].FirstOrDefault();
                            if (zonalInspMunicipDict.ContainsKey(municipalityId))
                            {
                                var zonalInspId = zonalInspMunicipDict[municipalityId].FirstOrDefault();
                                if (zonalInspId > 0)
                                {
                                    appeal.ZonalInspection = zonalInspDict.ContainsKey(zonalInspId) ? zonalInspDict[zonalInspId] : null;
                                }

                                listResult.Add(appeal);
                            }
                        }
                    }
                }

                this.InTransaction(listResult, serviceAppealCits);

                return new BaseDataResult { Success = true, Message = string.Format("Генерация прошла успешно!Обновлено {0} записей", listResult.Count) };
            }
            catch (Exception exc)
            {
                return new BaseDataResult { Success = false, Message = exc.Message };
            }
            finally
            {
                this.Container.Release(serviceAppealCits);
                this.Container.Release(serviceZonalInspectionMunicipality);
                this.Container.Release(serviceZonalInspection);
                this.Container.Release(serviceAppealCitsRealityObject);
            }
        }

        #region Транзакция

        private IDataTransaction BeginTransaction()
        {
            return Container.Resolve<IDataTransaction>();
        }

        private void InTransaction(IEnumerable<IEntity> list, IDomainService repos)
        {
            using (var transaction = BeginTransaction())
            {
                try
                {
                    foreach (var entity in list)
                    {
                        if (entity.Id.ToLong() > 0)
                        {
                            repos.Update(entity);
                        }
                        else
                        {
                            repos.Save(entity);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }
        #endregion

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

        public IDataResult CorrectDocNumbers(BaseParams baseParams)
        {
            var docService = Container.Resolve<IDomainService<DocumentGji>>();
            var docChildService = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var viewDispService = Container.Resolve<IDomainService<ViewDisposal>>();
            var muService = Container.Resolve<IDomainService<Municipality>>();
            var resolProsService = Container.Resolve<IDomainService<ResolPros>>();

            var mainDocs = docService.GetAll()
                .Where(x => x.DocumentYear == 2014)
                .Where(x => x.DocumentNum.ToString().Length == 6)
                .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal || x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                .ToArray();

            var maxNum = docService.GetAll()
                .Where(x => x.DocumentYear == 2014)
                .Where(x => x.DocumentNum.ToString().Length < 5)
                .Where(x => x.TypeDocumentGji == TypeDocumentGji.Disposal || x.TypeDocumentGji == TypeDocumentGji.ResolutionProsecutor)
                .Max(x => x.DocumentNum);

            var query = docService.GetAll()
                .Where(x => x.DocumentYear == 2014)
                //.Where(x => x.Parent.DocumentYear == 2014)
                .Where(x => x.DocumentNum.ToString().Length == 6)
                .Where(x => x.DocumentNumber != null);

            var counts = new Dictionary<string, int>();

            counts["main"] = mainDocs.Length;

            var result = new Dictionary<TypeDocumentGji, List<Result>>();

            foreach (TypeDocumentGji item in Enum.GetValues(typeof(TypeDocumentGji)))
            {
                result.Add(item, new List<Result>());
            }

            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var disp in mainDocs)
                    {
                        var muId =
                            disp.TypeDocumentGji == TypeDocumentGji.Disposal
                                ? viewDispService.GetAll()
                                    .Where(x => x.Id == disp.Id)
                                    .Select(x => x.MunicipalityId)
                                    .FirstOrDefault()
                                : resolProsService.GetAll()
                                    .Where(x => x.Id == disp.Id)
                                    .Where(x => x.Contragent.Municipality != null)
                                    .Select(x => x.Contragent.Municipality.Id)
                                    .FirstOrDefault();

                        var muCode = muId.HasValue && muId > 0
                            ? muService.Get(muId.Value).Code
                            : null;

                        var number = ++maxNum;

                        var oldNumber = disp.DocumentNumber;

                        var newNumber = muCode.IsEmpty()
                            ? number.ToString()
                            : string.Format("{0}-{1}", muCode, number);

                        result[disp.TypeDocumentGji].Add(new Result
                        {
                            OldNumber = oldNumber,
                            NewNumber = newNumber
                        });

                        disp.DocumentNum = number;
                        disp.DocumentSubNum = 0;
                        disp.DocumentNumber = newNumber;

                        docService.Update(disp);
                    }

                    var actRemovals = query.Where(x => x.TypeDocumentGji == TypeDocumentGji.ActRemoval);

                    counts["actRemovals"] = actRemovals.Count();

                    result[TypeDocumentGji.ActRemoval].AddRange(ProcessDocs(actRemovals, docService));

                    var acts = query.Where(x => x.TypeDocumentGji == TypeDocumentGji.ActCheck);

                    counts["acts"] = acts.Count();

                    result[TypeDocumentGji.ActCheck].AddRange(ProcessDocs(acts, docService));

                    var prescr = query.Where(x => x.TypeDocumentGji == TypeDocumentGji.Prescription);

                    counts["prescr"] = prescr.Count();

                    result[TypeDocumentGji.Prescription].AddRange(ProcessDocs(prescr, docService));

                    var prots = query.Where(x => x.TypeDocumentGji == TypeDocumentGji.Protocol);

                    counts["prots"] = prots.Count();

                    result[TypeDocumentGji.Protocol].AddRange(ProcessDocs(prots, docService));

                    var resols = query.Where(x => x.TypeDocumentGji == TypeDocumentGji.Resolution);

                    counts["resols"] = resols.Count();

                    result[TypeDocumentGji.Resolution].AddRange(ProcessDocs(resols, docService));
                    
                    tr.Commit();
                }
                catch (Exception)
                {
                    tr.Rollback();
                    throw;
                }
            }

            return new BaseDataResult(new {counts, result});
        }

        private IEnumerable<Result> ProcessDocs(IQueryable<DocumentGji> query, IDomainService<DocumentGji> service)
        {
            var result = new List<Result>();

            var docs = query.AsEnumerable().Distinct(x => x.Id);

            foreach (var stageDoc in docs.GroupBy(x => x.Stage.Id))
            {
                int stageMaxNum = 0;

                var parentStageId = stageDoc.Select(y => y.Stage.Parent.Id).First();

                var parentStageDoc = service.GetAll().First(x => x.Stage.Id == parentStageId);

                foreach (var doc in stageDoc)
                {
                    var number = parentStageDoc.DocumentNum;
                    var subNumber = stageMaxNum++;

                    var oldNumber = doc.DocumentNumber;
                    var newNumber = parentStageDoc.DocumentNumber + (subNumber > 0 ? "/" + subNumber : "");

                    result.Add(new Result
                    {
                        NewNumber = newNumber,
                        OldNumber = oldNumber
                    });

                    doc.DocumentNumber = newNumber;
                    doc.DocumentNum = number;
                    doc.DocumentSubNum = subNumber;

                    service.Update(doc);
                }
            }

            return result;
        }

        public IDataResult ListRealityObjectOnSpecAcc(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var roDomain = Container.ResolveDomain<RealityObject>();

            try
            {
                var data = roDomain.GetAll()
                    .Where(x => x.TypeHouse != TypeHouse.Individual && x.ConditionHouse != ConditionHouse.Razed && x.ConditionHouse != ConditionHouse.Emergency)
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.Municipality.Name,
                        x.Address
                    })
                    .Filter(loadParam, Container);

                var totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);

            }
            finally
            {

            }


        }


        private class Result
        {
            public string OldNumber;

            public string NewNumber;
        }

        public IGkhUserManager UserManager { get; set; }

        public IDataResult TrySetOpenEDS(BaseParams baseParams)
        {
            var EDSDomain = Container.Resolve<IDomainService<EDSInspection>>();
            var disposalAnnexServ = this.Container.Resolve<IRepository<DisposalAnnex>>();
            var actCheckServ = this.Container.Resolve<IRepository<ActCheckAnnex>>();
            var prescroptionServ = this.Container.Resolve<IRepository<PrescriptionAnnex>>();
            var protocolService = this.Container.Resolve<IRepository<ProtocolAnnex>>();
            var resolutionService = this.Container.Resolve<IRepository<ResolutionAnnex>>();

            try
            {
                var docId = baseParams.Params.ContainsKey("docId") ? baseParams.Params["docId"].ToLong() : 0;
                Operator thisOperator = UserManager.GetActiveOperator();
                if (docId > 0 && thisOperator?.Contragent != null)
                {
                    var eds = EDSDomain.Get(docId);
                    if (eds != null)
                    {
                        eds.NotOpened = false;
                        EDSDomain.Update(eds);
                    }

                    disposalAnnexServ.GetAll()
                    .Where(x => x.Disposal.Inspection.Id == eds.InspectionGji.Id)
                    .Where(x => x.SignedFile != null)
                    .ToList().ForEach(x =>
                    {
                        x.MessageCheck = MessageCheck.Recd;
                        x.DocumentDelivered = DateTime.Now;
                        disposalAnnexServ.Update(x);

                    });
                    actCheckServ.GetAll()
                   .Where(x => x.ActCheck.Inspection.Id == eds.InspectionGji.Id)
                   .Where(x => x.SignedFile != null)
                   .ToList().ForEach(x =>
                   {
                       x.MessageCheck = MessageCheck.Recd;
                       x.DocumentDelivered = DateTime.Now;
                       actCheckServ.Update(x);

                   });
                    prescroptionServ.GetAll()
                  .Where(x => x.Prescription.Inspection.Id == eds.InspectionGji.Id)
                  .Where(x => x.SignedFile != null)
                  .ToList().ForEach(x =>
                  {
                      x.MessageCheck = MessageCheck.Recd;
                      x.DocumentDelivered = DateTime.Now;
                      prescroptionServ.Update(x);

                  });
                    protocolService.GetAll()
                  .Where(x => x.Protocol.Inspection.Id == eds.InspectionGji.Id)
                  .Where(x => x.SignedFile != null)
                  .ToList().ForEach(x =>
                  {
                      x.MessageCheck = MessageCheck.Recd;
                      x.DocumentDelivered = DateTime.Now;
                      protocolService.Update(x);

                  });
                    resolutionService.GetAll()
                  .Where(x => x.Resolution.Inspection.Id == eds.InspectionGji.Id)
                  .Where(x => x.SignedFile != null)
                  .ToList().ForEach(x =>
                  {
                      x.MessageCheck = MessageCheck.Recd;
                      x.DocumentDelivered = DateTime.Now;
                      resolutionService.Update(x);

                  });
                }

  

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }
    }
}