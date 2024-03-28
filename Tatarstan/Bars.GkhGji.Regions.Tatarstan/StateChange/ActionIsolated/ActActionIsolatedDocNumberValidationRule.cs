namespace Bars.GkhGji.Regions.Tatarstan.StateChange.ActionIsolated
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    using Castle.Windsor;

    public class ActActionIsolatedDocNumberValidationRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public string Id => "gji_document_act_actionisolated_doc_number_validation_rule";

        /// <inheritdoc />
        public string Name => "Проверка возможности формирования номера акта " +
            "по КНМ без взаимодействия с контролируемыми лицами";

        /// <inheritdoc />
        public string TypeId => "gji_document_act_actionisolated";

        /// <inheritdoc />
        public string Description => "Данное правило проверяет формирование номера акта " +
            "по КНМ без взаимодействия с контролируемыми лицами в соответствии с правилами РТ";

        /// <inheritdoc />
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is ActActionIsolated actActionIsolated)
            {
                var documentGjiInspectorService = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
                var taskActionIsolatedService = this.Container.Resolve<IDomainService<TaskActionIsolated>>();
                var actActionIsolatedService = this.Container.Resolve<IDomainService<ActActionIsolated>>();
                var documentGjiChildrenDomain = this.Container.Resolve<IDomainService<DocumentGjiChildren>>();
                
                using (Container.Using(documentGjiInspectorService, taskActionIsolatedService, actActionIsolatedService, documentGjiChildrenDomain))
                {
                    var hasInspector = documentGjiInspectorService.GetAll()
                        .Any(x => x.DocumentGji.Id == actActionIsolated.Id);
                    
                    if (!actActionIsolated.DocumentDate.HasValue ||
                        !actActionIsolated.DocumentTime.HasValue ||
                        actActionIsolated.DocumentPlaceFias.IsNull() ||
                        actActionIsolated.Flat.IsNull() ||
                        !hasInspector)
                    {
                        return ValidateResult.No("Невозможно сформировать номер, " +
                            "поскольку имеются незаполненные обязательные поля.");
                    }

                    var taskActionIsolated = taskActionIsolatedService.GetAll()
                        .FirstOrDefault(x=> x.Inspection.Id == actActionIsolated.Inspection.Id);
                    
                    var relatedActs = actActionIsolatedService.GetAll()
                        .Join(documentGjiChildrenDomain.GetAll(),
                            x => x.Id,
                            y => y.Children.Id,
                            (x, y) => new
                            {
                                ActActionIsolated = x,
                                y.Parent
                            })
                        .Join(taskActionIsolatedService.GetAll(),
                            x => x.Parent.Id,
                            y => y.Id,
                            (x, y) => new
                            {
                                x.ActActionIsolated,
                                TaskActionIsolated = y
                            })
                        .Where(x => x.ActActionIsolated.Inspection.Id == actActionIsolated.Inspection.Id)
                        .Where(x => 
                            x.TaskActionIsolated.Inspection.TypeBase == TypeBase.ActionIsolated &&
                            x.ActActionIsolated.Inspection.TypeBase == TypeBase.ActionIsolated)
                        .Where(x => x.ActActionIsolated.Id != actActionIsolated.Id &&
                            x.TaskActionIsolated.Municipality == taskActionIsolated.Municipality &&
                            x.ActActionIsolated.DocumentNum.HasValue &&
                            x.ActActionIsolated.DocumentNumber != string.Empty)
                        .Select(x => x.ActActionIsolated);
                    
                    int? actActionIsolatedSerialNum = null;
                    if (relatedActs.Any())
                    {
                        // Если найденный документ единственный, то его подномер = null => проставляем новому подномер "1"
                        actActionIsolatedSerialNum = relatedActs.SafeMax(x => x.DocumentSubNum) + 1 ?? 1;
                    }

                    var numPostfix = actActionIsolatedSerialNum.HasValue
                        ? $"{taskActionIsolated.DocumentNum}/{actActionIsolatedSerialNum}"
                        : taskActionIsolated.DocumentNum.ToString();
                    actActionIsolated.DocumentNum = taskActionIsolated.DocumentNum;
                    actActionIsolated.DocumentSubNum = actActionIsolatedSerialNum;
                    actActionIsolated.DocumentNumber = $"{taskActionIsolated.Municipality.Code}-{numPostfix}";
                }
            }

            return ValidateResult.Yes();
        }
    }
}