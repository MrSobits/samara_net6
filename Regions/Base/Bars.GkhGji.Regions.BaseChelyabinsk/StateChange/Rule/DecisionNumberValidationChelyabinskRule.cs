namespace Bars.GkhGji.Regions.BaseChelyabinsk.StateChange.Rule
{
    using Bars.GkhGji.Contracts;

    /// <summary>
    /// Приказ
    /// </summary>
    public class DecisionNumberValidationChelyabinskRule : BaseDocNumberValidationChelyabinskRule
    {
        /// <inheritdoc />
        public IDisposalText DisposalText { get; set; }

        /// <inheritdoc />
        public override string Id => "gji_nso_decision_validation_number";

        /// <inheritdoc />
        public override string TypeId => "gji_document_decision";

        /// <inheritdoc />
        public override string Name => $"Челябинск - Присвоение номера решению";

        /// <inheritdoc />
        public override string Description => $"Челябинск - Данное правило присваивает номер решению";
    }
}