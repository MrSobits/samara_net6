namespace Bars.GkhGji.Regions.Chelyabinsk.StateChange
{
    /// <summary>
    /// Прием в работу обращения - в работе
    /// </summary>
    public class AcceptWorkRule : WorkRule
    {
        /// <inheritdoc />
        protected override bool IsAccept => true;

        /// <inheritdoc />
        public override string Id => this.GetType().Name;

        /// <inheritdoc />
        public override string Name => "Прием в работу обращения - в работе";
    }
}