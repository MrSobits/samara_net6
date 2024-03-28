namespace Bars.GkhGji.Regions.Tomsk.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Акта нарушения выявлены' из документа 'Приказ'
    /// </summary>
    public class TomskDisposalToActCheckHaveViolRule : IDocumentGjiRule
    {
        #region injection

        /// <summary>
        /// Контейнер 
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен сервис для "Рапоряжение ГЖИ"
        /// </summary>
        public IDomainService<Disposal> DisposalDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Используется в томске но поскольку есть большая вероятност ьчто дальеш в других регионах тоже могут нарушения создават ьв приказе"
        /// </summary>
        public IDomainService<DisposalViolation> DisposalViolDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Нарушение проверки
        /// Эта таблица хранит в себе нарушение проверки, без привязки к конкретному документу"
        /// </summary>
        public IDomainService<InspectionGjiViol> InsViolDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Акт проверки"
        /// </summary>
        public IDomainService<ActCheck> ActCheckDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Этап проверки ГЖИ"
        /// </summary>
        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Инспектора в проверке"
        /// </summary>
        public IDomainService<InspectionGjiInspector> InspectionInspectorDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Инспекторы документа ГЖИ"
        /// </summary>
        public IDomainService<DocumentGjiInspector> DocumentInspectorDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Дом предписания"
        /// </summary>
        public IDomainService<PrescriptionRealityObject> PrescriptionRoDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Этап указания к устранению нарушения в предписании"
        /// </summary>
        public IDomainService<PrescriptionViol> PrescriptionViolDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Таблица связи документов (Какой документ из какого был сформирован)"
        /// </summary>
        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Жилой дом"
        /// </summary>
        public IDomainService<RealityObject> RoDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Этап нарушения"
        /// </summary>
        public IDomainService<InspectionGjiViolStage> ViolStageDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Акт проверки"
        /// </summary>
        public IDomainService<ActCheckRealityObject> ActCheckRoDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Результат проверки для акта"
        /// </summary>
        public IDomainService<ActCheckVerificationResult> ActCheckVerifiDomain { get; set; }

        /// <summary>
        /// Домен сервис для "Этап выявления нарушения в акте проверки
        /// Данная таблица служит связтю между нарушением и Актом проверки
        /// Чтобы понимать какие Нарушения были выявлены входе проверки
        /// Для выявленных нарушений ставится плановая дата устранения"
        /// </summary>
        public IDomainService<ActCheckViolation> ActCheckViolDomain { get; set; }

        #endregion

        /// <summary>
        /// Код региона
        /// </summary>
        public virtual string CodeRegion
        {
            get { return "Tomsk"; }
        }

        /// <summary>
        /// Идентификатр реализации
        /// </summary>
        public virtual string Id
        {
            get { return "TomskDisposalToActCheckHaveViolRule"; }
        }

        /// <summary>
        /// Краткое описание
        /// </summary>
        public virtual string Description
        {
            get { return "Правило создания документа 'Акт (нарушения выявлены)' из документа 'Приказ'"; }
        }

        /// <summary>
        /// Наименование документ резултата
        /// </summary>
        public virtual string ActionUrl
        {
            get { return "B4.controller.ActCheck"; }
        }

        /// <summary>
        /// Карточка, которую нужно открыть после создания дкоумента
        /// </summary>
        public virtual string ResultName
        {
            get { return "Акт (нарушения выявлены)"; }
        }

        /// <summary>
        /// Тип документа инициатора, того кто инициирует действие
        /// </summary>
        public virtual TypeDocumentGji TypeDocumentInitiator
        {
            get { return TypeDocumentGji.Disposal; }
        }

        /// <summary>
        /// Тип документа результата, тоесть того который должен получится в резултате формирвоания
        /// </summary>
        public virtual TypeDocumentGji TypeDocumentResult
        {
            get { return TypeDocumentGji.ActCheck; }
        }

        /// <summary>
        /// Получение параметров котоыре переданы с клиента
        /// </summary>
        // тут надо принять параметры если таковые имеютя
        public virtual void SetParams(BaseParams baseParams)
        {
            // Не ожидаем никаких параметров
        }

        /// <summary>
        /// Метод формирования документа сразу по объекту документа (Если ест ьнеобходимость)
        /// </summary>
        /// <param name="document">Базовый документ ГЖИ</param>
        /// <returns></returns>
        public virtual IDataResult CreateDocument(DocumentGji document)
        {
            var docConcurents = ChildrenDomain.GetAll().Where(x => x.Parent.Id == document.Id).Select(x => x.Children).ToList();

            var listViols = new List<PrescriptionViol>();

            if (docConcurents.Count() > 0)
            {
                if (docConcurents.Any(x => x.TypeDocumentGji == TypeDocumentGji.ActCheck))
                {
                    return new BaseDataResult(
                        false,
                        "Невозможно сформировать Акт (нарушения выявлены), так как уже сформирован акт проверки");
                }

                var prescriptions = docConcurents.Where(x => x.TypeDocumentGji == TypeDocumentGji.Prescription).ToList();

                foreach (var pr in prescriptions)
                {
                    var viols = PrescriptionViolDomain.GetAll().Where(x => x.Document.Id == pr.Id).ToList();

                    if (!viols.Any())
                    {
                        return new BaseDataResult( false, string.Format("Невозможно сформировать Акт (нарушения выявлены), в предписании №{0} от {1} не указаны нарушения ", 
                            pr.DocumentNumber, 
                            pr.DocumentDate.ToDateTime().ToShortDateString()));
                    }

                    // сохраняем в словарь нарушения потом их будем переносить в акт
                    listViols.AddRange(viols);
                }
            }

            // Поулчаем само распоряжение
            var disposal = DisposalDomain.FirstOrDefault(x => x.Id == document.Id);

            #region Формируем этап проверки
            var parentStage = document.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;
            var currentStage = this.InspectionStageDomain.GetAll()
                                   .FirstOrDefault(x => x.Parent.Id == parentStage.Id
                                                        && x.TypeStage == TypeStage.ActCheck);

            if (currentStage == null)
            {
                // Если этап не найден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.ActCheck,
                    Parent = parentStage,
                    Position = 1
                };

                var stageMaxPosition =
                    InspectionStageDomain.GetAll()
                        .Where(x => x.Inspection.Id == document.Inspection.Id)
                        .OrderByDescending(x => x.Position)
                        .FirstOrDefault();

                if (stageMaxPosition != null)
                    currentStage.Position = stageMaxPosition.Position + 1;

                newStage = currentStage;
            }

            #endregion

            #region Формируем Акт Проверки
            var actCheck = new ActCheck
            {
                Inspection = document.Inspection,
                TypeActCheck = TypeActCheckGji.ActCheckIndividual,
                TypeDocumentGji = TypeDocumentGji.ActCheck,
                Stage = currentStage,
                DocumentDate = disposal.DateEnd
            };
            #endregion

            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = actCheck
            };
            #endregion

            #region Формируем Инспекторов
            var listInspectors = new List<DocumentGjiInspector>();
            var inspectorIds = DocumentInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == document.Id)
                .Select(x => x.Inspector.Id)
                .Distinct()
                .ToList();

            foreach (var id in inspectorIds)
            {
                listInspectors.Add(new DocumentGjiInspector
                {
                    DocumentGji = actCheck,
                    Inspector = new Inspector { Id = id }
                });
            }
            #endregion

            #region Сохраняем нарушения из приказа
            var listActCheckRo = new List<ActCheckRealityObject>();
            var listActCheckViols = new List<ActCheckViolation>();
            var actCheckRo = new Dictionary<long, ActCheckRealityObject>();
            var insViols = InsViolDomain.GetAll().Where(y => DisposalViolDomain.GetAll().Any(x => x.Document.Id == disposal.Id && x.InspectionViolation.Id == y.Id))
                                            .ToList();
            foreach (var viol in insViols)
            {
                ActCheckRealityObject actRo = null;

                var roId = viol.RealityObject != null ? viol.RealityObject.Id : 0;

                if (actCheckRo.ContainsKey(roId))
                {
                    actRo = actCheckRo[roId];
                }
                else
                {
                    actRo = new ActCheckRealityObject
                    {
                        ActCheck = actCheck,
                        RealityObject = viol.RealityObject,
                        HaveViolation = YesNoNotSet.Yes
                    };
                    listActCheckRo.Add(actRo);

                    // просто фиксируем чтобы вслед раз уже получит ьто что ест ьтакой дом
                    actCheckRo.Add((viol.RealityObject != null ? viol.RealityObject.Id : 0), actRo);
                }

                var actViol = new ActCheckViolation
                {
                    Document = actCheck,
                    ActObject = actRo,
                    InspectionViolation = viol,
                    TypeViolationStage = TypeViolationStage.Detection
                };

                listActCheckViols.Add(actViol);
            }

            #endregion

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    if (newStage != null)
                    {
                        this.InspectionStageDomain.Save(newStage);
                    }

                    this.ActCheckDomain.Save(actCheck);

                    this.ChildrenDomain.Save(parentChildren);

                    listInspectors.ForEach(x => this.DocumentInspectorDomain.Save(x));

                    listActCheckRo.ForEach(x => this.ActCheckRoDomain.Save(x));

                    listActCheckViols.ForEach(x => this.ActCheckViolDomain.Save(x));

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            }
            #endregion

            return new BaseDataResult(new { documentId = actCheck.Id, inspectionId = actCheck.Inspection.Id });
        }

        /// <summary>
        /// проверка валидности правила
        /// Например перед выполнением действия требуется проверить
        /// Можно ли формирвоать какойто дкоумент, например Если уже есть по документу уже созданные 
        /// то можно недават ьсоздавать новые (если требуется по процессу)
        /// </summary>
        /// <param name="document">Базовый документ ГЖИ</param>
        /// <returns></returns>
        public virtual IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult();
        }
    }
}
