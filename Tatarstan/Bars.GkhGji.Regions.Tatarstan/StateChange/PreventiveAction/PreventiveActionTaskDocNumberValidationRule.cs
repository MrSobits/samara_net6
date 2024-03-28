namespace Bars.GkhGji.Regions.Tatarstan.StateChange.PreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    using Castle.Windsor;

    public class PreventiveActionTaskDocNumberValidationRule: IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public string Id => "gji_document_preventive_action_task_doc_number_validation_rule";

        /// <inheritdoc />
        public string Name => "Проверка возможности формирования номера задания профилактического мероприятия";

        /// <inheritdoc />
        public string TypeId => "gji_document_preventive_action_task";

        /// <inheritdoc />
        public string Description => "Данное правило проверяет формирование номера задания профилактического мероприятия в соответствии с правилами РТ";

        /// <inheritdoc />
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is PreventiveActionTask preventiveActionTask)
            {
                var documentGjiChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
                var documentDomain = this.Container.ResolveDomain<DocumentGji>();
                var preventiveActionDomain = this.Container.ResolveDomain<PreventiveAction>();
                var preventiveActionTaskDomain = this.Container.ResolveDomain<PreventiveActionTask>();
                
                using (this.Container.Using(documentGjiChildrenDomain,
                    preventiveActionDomain, preventiveActionTaskDomain, documentDomain))
                {
                    if (!preventiveActionTask.DocumentDate.HasValue    ||
                        preventiveActionTask.ActionLocation.IsNull()   ||
                        !preventiveActionTask.ActionStartDate.HasValue ||
                        preventiveActionTask.TaskingInspector.IsNull() ||
                        preventiveActionTask.Executor.IsNull()         ||
                        preventiveActionTask.StructuralSubdivision.IsNull())
                    {
                        return ValidateResult.No("Невозможно сформировать номер, " +
                            "поскольку имеются незаполненные обязательные поля.");
                    }

                    var preventiveAction = preventiveActionDomain.GetAll()
                        .FirstOrDefault(x => x.Inspection.Id == preventiveActionTask.Inspection.Id);

                    var relatedPreventiveActionTasks = preventiveActionTaskDomain.GetAll()
                        .Join(documentGjiChildrenDomain.GetAll(),
                            x => x.Id,
                            y => y.Children.Id,
                            (x, y) => new
                            {
                                PreventiveActionTask = x,
                                y.Parent
                            })
                        .Join(preventiveActionDomain.GetAll(),
                            x => x.Parent.Id,
                            y => y.Id,
                            (x, y) => new
                            {
                                x.PreventiveActionTask,
                                PreventiveAction = y
                            })
                        .Where(x => x.PreventiveAction.Inspection.Id == preventiveAction.Inspection.Id)
                        .Where(x =>
                            x.PreventiveActionTask.Inspection.TypeBase == TypeBase.PreventiveAction &&
                            x.PreventiveAction.Inspection.TypeBase == TypeBase.PreventiveAction)
                        .Where(x => x.PreventiveActionTask.Id != preventiveActionTask.Id &&
                            x.PreventiveAction.Municipality == preventiveAction.Municipality &&
                            x.PreventiveActionTask.DocumentNum.HasValue &&
                            x.PreventiveActionTask.DocumentNumber != string.Empty)
                        .Select(x => x.PreventiveActionTask);

                    int? preventiveActionTaskSerialNum = null;
                    if (relatedPreventiveActionTasks.Any())
                    {
                        // Если найденный документ единственный, то его подномер = null => проставляем новому подномер "1"
                        preventiveActionTaskSerialNum = relatedPreventiveActionTasks.SafeMax(x => x.DocumentSubNum) + 1 ?? 1;
                    }

                    var numPostfix = preventiveActionTaskSerialNum.HasValue
                        ? $"{preventiveAction.DocumentNum}/{preventiveActionTaskSerialNum}"
                        : preventiveAction.DocumentNum.ToString();
                    preventiveActionTask.DocumentNum = preventiveAction.DocumentNum;
                    preventiveActionTask.DocumentSubNum = preventiveActionTaskSerialNum;
                    preventiveActionTask.DocumentNumber = $"{preventiveAction.Municipality.Code}-{numPostfix}";
                }
            }

            return ValidateResult.Yes();
        }
        
    }
}