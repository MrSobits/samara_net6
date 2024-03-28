namespace Bars.GkhGji.StateChange
{
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Regions.Voronezh.StateChange;

    /// <summary>
    /// Приказ
    /// </summary>
    public class DisposalNumberValidationVoronezhRule : BaseDocNumberValidationVoronezhRule
    {
        /// <inheritdoc />
        public IDisposalText DisposalText { get; set; }

        /// <inheritdoc />
        public override string Id => "gji_vor_disposal_validation_number";

        /// <inheritdoc />
        public override string TypeId => "gji_document_disp";

        /// <inheritdoc />
        public override string Name => $"Воронеж - Присвоение номера {this.DisposalText.GenetiveCase.ToLower()}";

        /// <inheritdoc />
        public override string Description => $"Воронеж - Данное правило присваивает номера {this.DisposalText.GenetiveCase.ToLower()}";
    }
}