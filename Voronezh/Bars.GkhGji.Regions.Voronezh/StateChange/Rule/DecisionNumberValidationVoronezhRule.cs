namespace Bars.GkhGji.StateChange
{
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Regions.Voronezh.StateChange;

    /// <summary>
    /// Приказ
    /// </summary>
    public class DecisionNumberValidationVoronezhRule : BaseDocNumberValidationVoronezhRule
    {
        /// <inheritdoc />
        public IDisposalText DisposalText { get; set; }

        /// <inheritdoc />
        public override string Id => "gji_vor_decision_validation_number";

        /// <inheritdoc />
        public override string TypeId => "gji_document_decision";

        /// <inheritdoc />
        public override string Name => $"Воронеж - Присвоение номера решению";

        /// <inheritdoc />
        public override string Description => $"Воронеж - Данное правило присваивает номер решению";
    }
}