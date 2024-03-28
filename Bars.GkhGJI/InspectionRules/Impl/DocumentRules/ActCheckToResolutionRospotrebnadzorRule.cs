namespace Bars.GkhGji.InspectionRules
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Правило создание документа 'Постановление Роспотребнадзора' из документа 'Акт проверки' (по выбранным нарушениям)
    /// </summary>
    public class ActCheckToResolutionRospotrebnadzorRule: IDocumentGjiRule
    {
        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Постановление Роспотребнадзора
        /// </summary>
        public IDomainService<ResolutionRospotrebnadzor> ResolutionRospotrebnadzorDomain { get; set; }

        /// <summary>
        /// Этап проверки ГЖИ
        /// </summary>
        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        /// <summary>
        /// Таблица связи документов
        /// </summary>
        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        /// <summary>
        /// Нарушения
        /// </summary>
        public IDomainService<InspectionGjiViol> InspectionViolDomain { get; set; }

        /// <summary>
        /// Нарушения в акте проверки
        /// </summary>
        public IDomainService<ActCheckViolation> ActCheckViolationDomain { get; set; }

        /// <summary>
        /// Этап наказания за нарушение в постановлении Роспотребнадзора
        /// </summary>
        public IDomainService<ResolutionRospotrebnadzorViolation> ResolutionRospotrebnadzorViolationDomain { get; set; }

        /// <summary>
        /// Код региона
        /// </summary>
        public string CodeRegion => "Tat";

        /// <summary>
        /// Идентификатр реализации
        /// </summary>
        public string Id => nameof(ActCheckToResolutionRospotrebnadzorRule);

        /// <summary>
        /// Краткое описание
        /// </summary>
        public string Description => "Правило создание документа 'Постановление Роспотребнадзора' из документа 'Акт проверки' (по выбранным нарушениям)";

        /// <summary>
        /// Наименование документ резултата
        /// </summary>
        public string ResultName => "Постановление Роспотребнадзора";

        /// <summary>
        /// Карточка, которую нужно открыть после создания дкоумента
        /// </summary>
        public string ActionUrl => "B4.controller.ResolutionRospotrebnadzor";

        /// <summary>
        /// Тип документа инициатора, того кто инициирует действие
        /// </summary>
        public TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.ActCheck;

        /// <summary>
        /// Тип документа результата, то есть того который должен получится в резултате формирвоания
        /// </summary>
        public TypeDocumentGji TypeDocumentResult => TypeDocumentGji.ResolutionRospotrebnadzor;

        /// <summary>
        /// Идентификаторы нарушений
        /// </summary>
        protected long[] ViolationIds { get; set; }

        /// <summary>
        /// Получение параметров котоыре переданы с клиента
        /// </summary>
        public void SetParams(BaseParams baseParams)
        {
            // В данном методе принимаем параметр violationIds выбранных дома
            var violationIds = baseParams.Params.GetAs("violationIds", "");

            this.ViolationIds = !string.IsNullOrEmpty(violationIds)
                                  ? violationIds.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            if (this.ViolationIds.Length == 0)
            {
                throw new Exception("Необходимо выбрать нарушения");
            }
        }

        /// <summary>
        /// Метод формирования документа сразу по объекту документа (Если есть необходимость)
        /// </summary>
        public IDataResult CreateDocument(DocumentGji document)
        {
            var resolution = new ResolutionRospotrebnadzor()
            {
                Inspection = document.Inspection,
                TypeDocumentGji = this.TypeDocumentResult,
                Contragent = document.Inspection.Contragent,
                TypeInitiativeOrg = TypeInitiativeOrgGji.Rospotrebnadzor,
                Paided = YesNoNotSet.NotSet,
                DocumentDate = DateTime.Today,
                DocumentYear = DateTime.Today.Year
            };

            // Если у родительского документа есть этап у которого есть родитель
            // тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
            var parentStage = document.Stage;
            if (parentStage?.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            var currentStage = InspectionStageDomain.GetAll()
                .FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.ResolutionRospotrebnadzor);

            var isNewStage = false;
            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.ResolutionRospotrebnadzor,
                    Parent = parentStage,
                    Position = 1
                };
                var stageMaxPosition = InspectionStageDomain.GetAll()
                    .Where(x => x.Inspection.Id == document.Inspection.Id)
                    .OrderByDescending(x => x.Position).FirstOrDefault();

                if (stageMaxPosition != null)
                {
                    currentStage.Position = stageMaxPosition.Position + 1;
                }
                isNewStage = true;
            }

            resolution.Stage = currentStage;

            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = resolution
            };

            var listResolutionRospotrebnadzorViol = new List<ResolutionRospotrebnadzorViolation>();

            var violationList = ActCheckViolationDomain.GetAll().Where(x => ViolationIds.Contains(x.InspectionViolation.Id));

            foreach (var viol in violationList)
            {
                var newRecord = new ResolutionRospotrebnadzorViolation
                {
                    Violation = viol,
                    Resolution = resolution
                };

                listResolutionRospotrebnadzorViol.Add(newRecord);
            }

            this.Container.InTransaction(() =>
            {
                if (isNewStage)
                {
                    this.InspectionStageDomain.Save(currentStage);
                }
                this.ResolutionRospotrebnadzorDomain.Save(resolution);
                this.ChildrenDomain.Save(parentChildren);
                listResolutionRospotrebnadzorViol.ForEach(x => this.ResolutionRospotrebnadzorViolationDomain.Save(x));
            });

            return new BaseDataResult(new { documentId = resolution.Id, inspectionId = document.Inspection.Id });
        }

        /// <summary>
        /// Проверка валидности правила
        /// </summary>
        /// <remarks>
        /// Например, перед выполнением действия требуется проверить,
        /// можно ли формирвоать какой-либо документ, если он не был ранее создан
        /// Также валидация используется при формировании списка кнопки "Сформировать"
        /// </remarks>
        public IDataResult ValidationRule(DocumentGji document)
        {
            if (document.Inspection.TypeBase == TypeBase.GjiWarning)
            {
                return new BaseDataResult(false, "");
            }

            return new BaseDataResult();
        }
    }
}