namespace Bars.GkhGji.Regions.Chelyabinsk.StateChange
{
    /// <summary>
    /// Успешная отмена обращения граждан
    /// </summary>
    public class SuccessCitizensAppealCancelRule : CitizensAppealCancelRule
    {
        /// <inheritdoc />
        protected override bool IsSuccess => true;

        /// <inheritdoc />
        public override string Id => this.GetType().Name;

        /// <inheritdoc />
        public override string Name => "Успешная отмена обращения граждан";
    }
}