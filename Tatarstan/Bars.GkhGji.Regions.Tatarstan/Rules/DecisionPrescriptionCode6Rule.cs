namespace Bars.GkhGji.Regions.Tatarstan.Rules
{
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    public class DecisionPrescriptionCode6Rule : DispPrescrCode6Rule
    {
        public override string Code => nameof(DecisionPrescriptionCode6Rule);

        public override string Name => "Решение на проверку предписания " +
            "(код вида проверки первого решения - 6)";

        protected override TypeDocumentGji AvailableTypeDocumentGji => TypeDocumentGji.Decision;
    }
}