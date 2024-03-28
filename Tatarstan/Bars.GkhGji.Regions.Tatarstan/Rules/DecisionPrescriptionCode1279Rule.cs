namespace Bars.GkhGji.Regions.Tatarstan.Rules
{
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    public class DecisionPrescriptionCode1279Rule : DispPrescrCode1279Rule
    {
        public override string Code => nameof(DecisionPrescriptionCode1279Rule);

        public override string Name => "Решение на проверку предписания " +
            "(код вида проверки первого решения - 1,2,7,9)";

        protected override TypeDocumentGji AvailableTypeDocumentGji => TypeDocumentGji.Decision;
    }
}