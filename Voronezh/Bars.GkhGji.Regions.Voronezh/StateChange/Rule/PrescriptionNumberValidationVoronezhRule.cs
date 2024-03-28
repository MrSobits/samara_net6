namespace Bars.GkhGji.StateChange
{
    using System.Linq;

    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Voronezh.StateChange;

    /// <summary>
    /// Предписание
    /// </summary>
    public class PrescriptionNumberValidationVoronezhRule : BaseDocNumberValidationVoronezhRule
    {
        /// <inheritdoc />
        public override string Id => "gji_vor_prescription_validation_number";

        /// <inheritdoc />
        public override string Name => "Воронеж - Присвоение номера предписания";

        /// <inheritdoc />
        public override string TypeId => "gji_document_prescr";

        /// <inheritdoc />
        public override string Description => "Воронеж - Данное правило присваивает номер предписания";

        /// <inheritdoc />
        public override string GetPrefix(DocumentGji document)
        {
            var inspectors = this.DocumentGjiInspectorDomain.GetAll()
                .Where(x => x.DocumentGji.Id == document.Id)
                .Select(x => x.Inspector);

            var id = this.ZonalInspectionInspectorDomain.GetAll()
                .Where(x => inspectors.Any(i => i == x.Inspector))
                .Select(x => x.ZonalInspection.Id)
                .FirstOrDefault();

            var zonalPrefix = this.ZonalInspectionPrefixDomain.GetAll()
                .FirstOrDefault(x => x.ZonalInspection.Id == id);

            var prefix = zonalPrefix != null ? zonalPrefix.PrescriptionPrefix : string.Empty;
            if (!string.IsNullOrWhiteSpace(prefix))
            {
                prefix += "/";
            }

            return prefix;
        }
    }
}