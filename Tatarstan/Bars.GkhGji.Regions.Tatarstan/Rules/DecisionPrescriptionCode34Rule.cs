namespace Bars.GkhGji.Regions.Tatarstan.Rules
{
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    public class DecisionPrescriptionCode34Rule : DispPrescrCode34Rule
    {
        public override string Code => nameof(DecisionPrescriptionCode34Rule);

        public override string Name => "Решение на проверку предписания " +
            "(код вида проверки первого решения - 3,4)";

        protected override TypeDocumentGji AvailableTypeDocumentGji => TypeDocumentGji.Decision;
    }
}