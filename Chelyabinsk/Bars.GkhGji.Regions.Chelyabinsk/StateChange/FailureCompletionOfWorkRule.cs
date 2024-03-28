namespace Bars.GkhGji.Regions.Chelyabinsk.StateChange
{
    /// <summary>
    /// Неуспешное завершение работы по обращению
    /// </summary>
    public class FailureCompletionOfWorkRule : CompletionOfWorkRule
    {
        /// <inheritdoc />
        protected override bool IsSuccess => false;

        /// <inheritdoc />
        public override string Id => this.GetType().Name;

        /// <inheritdoc />
        public override string Name => "Неуспешное завершение работы по обращению";
    }
}