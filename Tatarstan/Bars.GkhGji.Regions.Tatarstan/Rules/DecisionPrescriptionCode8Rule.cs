namespace Bars.GkhGji.Regions.Tatarstan.Rules
{
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    public class DecisionPrescriptionCode8Rule : DispPrescrCode8Rule
    {
        public override string Code => nameof(DecisionPrescriptionCode8Rule);

        public override string Name => "Решение на проверку предписания " +
            "(код вида проверки первого решения - 8)";

        protected override TypeDocumentGji AvailableTypeDocumentGji => TypeDocumentGji.Decision;
    }
}