namespace Bars.GkhGji.Regions.Tatarstan.StateChange.PreventiveAction
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    using Castle.Windsor;

    public class PreventiveActionDocNumberValidationRule: IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public string Id => "gji_document_preventive_action_doc_number_validation_rule";

        /// <inheritdoc />
        public string Name => "Проверка возможности формирования номера профилактического мероприятия";

        /// <inheritdoc />
        public string TypeId => "gji_document_preventive_action";

        /// <inheritdoc />
        public string Description => "Данное правило проверяет формирование номера профилактического мероприятия в соответствии с правилами РТ";

        /// <inheritdoc />
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is PreventiveAction preventiveAction)
            {
                var documentGjiInspectorService = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
                var preventiveActionService = this.Container.Resolve<IDomainService<PreventiveAction>>();
                
                using (this.Container.Using(documentGjiInspectorService, preventiveActionService))
                {
                    var hasInspector = documentGjiInspectorService.GetAll()
                        .Any(x => x.DocumentGji.Id == preventiveAction.Id);
                    
                    if (!hasInspector ||
                        !preventiveAction.ControlledPersonType.HasValue ||
                        preventiveAction.ControlledOrganization.IsNull())
                    {
                        return ValidateResult.No("Невозможно сформировать номер, " +
                            "поскольку имеются незаполненные обязательные поля.");
                    }

                    var preventiveActionSerialNum = preventiveActionService.GetAll()
                        .Where(x => x.Id != preventiveAction.Id &&
                            x.Municipality == preventiveAction.Municipality &&
                            x.DocumentNum.HasValue &&
                            x.DocumentNumber != string.Empty)
                        .SafeMax(x => x.DocumentNum) + 1 ?? 1;

                    preventiveAction.DocumentNum = preventiveActionSerialNum;
                    preventiveAction.DocumentNumber = $"{preventiveAction.Municipality.Code}-{preventiveActionSerialNum}";
                }
            }

            return ValidateResult.Yes();
        }
        
    }
}