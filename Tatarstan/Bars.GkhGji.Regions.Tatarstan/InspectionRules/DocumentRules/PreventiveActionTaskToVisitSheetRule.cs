namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    using Castle.Windsor;

    public class PreventiveActionTaskToVisitSheetRule : IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<PreventiveActionTask> PreventiveActionTaskDomain { get; set; }

        public IDomainService<VisitSheet> VisitSheetDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }

        /// <inheritdoc />
        public string CodeRegion => "Tat";

        /// <inheritdoc />
        public string Id => nameof(PreventiveActionTaskToVisitSheetRule);

        /// <inheritdoc />
        public string Description => "Правило создания документа 'Лист визита' из документа 'Задание по профилактическому мероприятию'";

        /// <inheritdoc />
        public string ResultName => "Лист визита";

        /// <inheritdoc />
        public string ActionUrl => "B4.controller.preventiveaction.Visit";

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.PreventiveActionTask;

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentResult => TypeDocumentGji.VisitSheet;

        /// <inheritdoc />
        public void SetParams(BaseParams baseParams)
        {
        }

        /// <inheritdoc />
        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем этап проверки
            var parentStage = document.Stage;
            if (parentStage?.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;
            var currentStage = this.InspectionStageDomain.GetAll()
                .FirstOrDefault(x => x.Parent.Id == parentStage.Id
                    && x.TypeStage == TypeStage.VisitSheet);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.VisitSheet,
                    Parent = parentStage,
                    Position = 1
                };

                var stageMaxPosition = this.InspectionStageDomain.GetAll()
                    .Where(x => x.Inspection.Id == document.Inspection.Id)
                    .OrderByDescending(x => x.Position)
                    .FirstOrDefault();

                if (stageMaxPosition != null)
                    currentStage.Position = stageMaxPosition.Position + 1;

                newStage = currentStage;
            }
            #endregion

            #region Формируем документ
            var state = this.StateDomain.GetAll()
                .FirstOrDefault(x => x.StartState && x.TypeId == "gji_document_visit_sheet");
            var task = this.PreventiveActionTaskDomain.Get(document.Id);

            var visitSheet = new VisitSheet
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.VisitSheet,
                Stage = currentStage,
                State = state,
                VisitDateStart = task.ActionStartDate,
                ExecutingInspector = task.Executor,
                HasCopy = YesNoNotSet.NotSet
            };
            #endregion

            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = visitSheet
            };
            #endregion

            #region Сохранение
            using (var tr = this.Container.Resolve<IDataTransaction>())
            {
                if (newStage != null)
                {
                    this.InspectionStageDomain.Save(newStage);
                }

                this.VisitSheetDomain.Save(visitSheet);
                this.ChildrenDomain.Save(parentChildren);

                tr.Commit();
            }
            #endregion

            return new BaseDataResult(new { documentId = visitSheet.Id, inspectionId = visitSheet.Inspection.Id });
        }

        /// <inheritdoc />
        public IDataResult ValidationRule(DocumentGji document)
        {
            if (this.ChildrenDomain.GetAll()
                .Any(
                    x =>
                        x.Parent.Id == document.Id
                        && x.Children.TypeDocumentGji == TypeDocumentGji.VisitSheet))
            {
                return new BaseDataResult(false, "У задания может быть только 1 лист");
            }

            if (this.PreventiveActionTaskDomain.Get(document.Id)?.ActionType != PreventiveActionType.Visit)
            {
                return new BaseDataResult(false, "Вид мероприятия не Визит");
            }

            return new BaseDataResult();
        }
    }
}