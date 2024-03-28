namespace Bars.GkhGji.StateChange
{
    using Bars.GkhGji.Regions.Voronezh.StateChange;

    /// <summary>
    /// Представление
    /// </summary>
    public class PresentationNumberValidationVoronezhRule : BaseDocNumberValidationVoronezhRule
    {
        /// <inheritdoc />
        public override string Id => "gji_vor_presentation_validation_number";

        /// <inheritdoc />
        public override string Name => "Воронеж - Присвоение номера представления";

        /// <inheritdoc />
        public override string TypeId => "gji_document_presen";

        /// <inheritdoc />
        public override string Description => "Воронеж - Данное правило присваивает номер предписания";
    }
}