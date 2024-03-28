namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules.DocumentRules
{
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Поскольку в РТ данный акт должен называтся просто АктПроверки, то переопределяем свойство и заменяем реализацию
    /// </summary>
    public class DecisionToActCheckByRoRule : TatDisposalToActCheckByRoRule
    {
        /// <inheritdoc />
        public override string Description => "Правило создание документа 'Акт проверки' из документа 'Решение' (по выбранным домам)";

        /// <inheritdoc />
        public override TypeDocumentGji TypeDocumentInitiator => TypeDocumentGji.Decision;
    }
}