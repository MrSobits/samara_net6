namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.GkhGji.Regions.Tatarstan.Entities.Disposal;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanDecision;

    /// <summary>
    /// Mapping <see cref="DecisionKnmAction"/>
    /// </summary>
    public class DecisionKnmActionMap : BaseKnmActionMainEntityRefMap<DecisionKnmAction, TatarstanDecision>
    {
        /// <inheritdoc />
        public DecisionKnmActionMap()
            : base("Запланированные действия", "GJI_DECISION_KNM_ACTION")
        {
        }

        /// <inheritdoc />
        protected override string MainEntityName() => "Решение";

        /// <inheritdoc />
        protected override string MainEntityColumn() => "DECISION_ID";
    }
}