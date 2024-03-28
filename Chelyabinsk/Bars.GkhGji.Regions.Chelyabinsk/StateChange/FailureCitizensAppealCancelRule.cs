namespace Bars.GkhGji.Regions.Chelyabinsk.StateChange
{
    /// <summary>
    /// Неуспешная отмена обращения граждан
    /// </summary>
    public class FailureCitizensAppealCancelRule : CitizensAppealCancelRule
    {
        /// <inheritdoc />
        protected override bool IsSuccess => false;

        /// <inheritdoc />
        public override string Id => this.GetType().Name;

        /// <inheritdoc />
        public override string Name => "Неуспешная отмена обращения граждан";
    }
}