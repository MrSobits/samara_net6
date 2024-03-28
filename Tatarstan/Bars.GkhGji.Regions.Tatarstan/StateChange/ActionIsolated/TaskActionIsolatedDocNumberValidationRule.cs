namespace Bars.GkhGji.Regions.Tatarstan.StateChange.ActionIsolated
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    using Castle.Windsor;

    public class TaskActionIsolatedDocNumberValidationRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public string Id => "gji_document_task_actionisolated_doc_number_validation_rule";

        /// <inheritdoc />
        public string Name => "Проверка возможности формирования номера задания " +
            "по КНМ без взаимодействия с контролируемыми лицами";

        /// <inheritdoc />
        public string TypeId => "gji_document_task_actionisolated";

        /// <inheritdoc />
        public string Description => "Данное правило проверяет формирование номера задания " +
            "по КНМ без взаимодействия с контролируемыми лицами в соответствии с правилами РТ";

        /// <inheritdoc />
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            if (statefulEntity is TaskActionIsolated taskActionIsolated)
            {
                if (taskActionIsolated.ControlType.IsNull() ||
                    !taskActionIsolated.DateStart.HasValue ||
                    !taskActionIsolated.TimeStart.HasValue)
                {
                    return ValidateResult.No("Невозможно сформировать номер, " +
                        "поскольку имеются незаполненные обязательные поля.");
                }

                var taskActionIsolatedService = this.Container.Resolve<IDomainService<TaskActionIsolated>>();

                using (this.Container.Using(taskActionIsolatedService))
                {
                    var taskActionIsolatedSerialNum = taskActionIsolatedService.GetAll()
                        .Where(x => x.Id != taskActionIsolated.Id &&
                            x.Municipality == taskActionIsolated.Municipality &&
                            x.DocumentNum.HasValue &&
                            x.DocumentNumber != string.Empty)
                        .SafeMax(x => x.DocumentNum) + 1 ?? 1;

                    taskActionIsolated.DocumentNum = taskActionIsolatedSerialNum;
                    taskActionIsolated.DocumentNumber = $"{taskActionIsolated.Municipality.Code}-{taskActionIsolatedSerialNum}";
                }
            }

            return ValidateResult.Yes();
        }
    }
}