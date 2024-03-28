namespace Bars.GkhGji.Regions.Tatarstan.StateChange
{
    using Bars.B4.Modules.States;

    public class TatarstanResolutionGjiValidationRule : IRuleChangeStatus
    {
        /// <inheritdoc />
        public string Id => "gji_document_resolution_gji_rt_validation_rule";

        /// <inheritdoc />
        public string Name => "Проверка заполненности карточки поставновления";

        /// <inheritdoc />
        public string TypeId => "gji_document_resolution_gji_rt";

        /// <inheritdoc />
        public string Description => this.Name;

        /// <inheritdoc />
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            return ValidateResult.Yes();
        }
    }
}
