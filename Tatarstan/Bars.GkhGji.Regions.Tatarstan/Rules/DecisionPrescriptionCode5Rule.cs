namespace Bars.GkhGji.Regions.Tatarstan.Rules
{
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    public class DecisionPrescriptionCode5Rule : DispPrescrCode5Rule
    {
        public override string Code => nameof(DecisionPrescriptionCode5Rule);

        public override string Name => "Решение на проверку предписания " +
            "(код вида проверки первого решения - 5)";

        protected override TypeDocumentGji AvailableTypeDocumentGji => TypeDocumentGji.Decision;
    }
}