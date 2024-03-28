namespace Bars.GkhGji.Regions.Chelyabinsk.StateChange
{
    /// <summary>
    /// Прием в работу обращения - не принято
    /// </summary>
    public class NotAcceptWorkRule : WorkRule
    {
        /// <inheritdoc />
        protected override bool IsAccept => false;

        /// <inheritdoc />
        public override string Id => this.GetType().Name;

        /// <inheritdoc />
        public override string Name => "Прием в работу обращения - не принято";
    }
}