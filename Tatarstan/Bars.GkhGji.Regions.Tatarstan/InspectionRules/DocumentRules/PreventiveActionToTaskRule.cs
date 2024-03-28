namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Extensions;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    using Castle.Windsor;

    public class PreventiveActionToTaskRule: IDocumentGjiRule
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<PreventiveAction> PreventiveActionDomain { get; set; }

        public IDomainService<PreventiveActionTask> TaskDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<State> StateDomain { get; set; }

        /// <inheritdoc />
        public string CodeRegion => "Tat";

        /// <inheritdoc />
        public string Id => nameof(PreventiveActionToTaskRule);

        /// <inheritdoc />
        public string Description => "Правило создания документа 'Задание профилактического мероприятия' из документа 'Профилактическое мероприятие'";

        /// <inheritdoc />
        public string ResultName => "Задание";

        /// <inheritdoc />
        public string ActionUrl => "B4.controller.preventiveaction.task.Task";

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.PreventiveAction;

        /// <inheritdoc />
        public TypeDocumentGji TypeDocumentResult => TypeDocumentGji.PreventiveActionTask;

        /// <inheritdoc />
        public void SetParams(BaseParams baseParams)
        {
        }

        /// <inheritdoc />
        public IDataResult CreateDocument(DocumentGji document)
        {
            #region Формируем этап проверки
            var parentStage = document.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;
            var currentStage = this.InspectionStageDomain.GetAll()
                                   .FirstOrDefault(x => x.Parent.Id == parentStage.Id
                                                        && x.TypeStage == TypeStage.PreventiveActionTask);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = document.Inspection,
                    TypeStage = TypeStage.PreventiveActionTask,
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
            var state = this.StateDomain.GetAll().FirstOrDefault(x => x.StartState && x.TypeId == "gji_document_preventive_action_task");
            var action = this.PreventiveActionDomain.Get(document.Id);

            var task = new PreventiveActionTask
            {
                Inspection = document.Inspection,
                TypeDocumentGji = TypeDocumentGji.PreventiveActionTask,
                Stage = currentStage,
                State = state,
                ActionType = action.ActionType,
                VisitType = action.VisitType,
                ParticipationRejection = YesNo.No
            };
            #endregion

            #region Формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = document,
                Children = task
            };
            #endregion
            
            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                if (newStage != null)
                {
                    this.InspectionStageDomain.Save(newStage);
                }

                this.TaskDomain.Save(task);

                this.ChildrenDomain.Save(parentChildren);
                
                tr.Commit();
            }
            #endregion

            return new BaseDataResult(new { documentId = task.Id, inspectionId = task.Inspection.Id });
        }

        /// <inheritdoc />
        public IDataResult ValidationRule(DocumentGji document)
        {
            if (this.ChildrenDomain.GetAll()
                .Any(
                    x =>
                        x.Parent.Id == document.Id
                        && x.Children.TypeDocumentGji == TypeDocumentGji.PreventiveActionTask))
            {
                return new BaseDataResult(false, "В рамках профилактического мероприятия может быть только 1 задание");
            }

            var action = this.PreventiveActionDomain.Get(document.Id);

            if (action.IsNull() || action.ActionType.IsDefault() || action.ActionType == PreventiveActionType.Visit && (!action.VisitType.HasValue || action.VisitType.IsDefault()))
            {
                return new BaseDataResult(false, "Не заполнены обязательные поля");
            }

            return new BaseDataResult();
        }
    }
}
