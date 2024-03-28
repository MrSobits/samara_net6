namespace Bars.GkhGji.Regions.Chelyabinsk.StateChange
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Успешное завершение работы по обращению
    /// </summary>
    public class SuccessCompletionOfWorkRule : CompletionOfWorkRule
    {
        /// <inheritdoc />
        protected override bool IsSuccess => true;

        /// <inheritdoc />
        public override string Id => this.GetType().Name;

        /// <inheritdoc />
        public override string Name => "Успешное завершение работы по обращению";

        public override ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var appeal = statefulEntity as AppealCits;

            if (!(appeal?.AppealUid.HasValue ?? false))
            {
           //     return ValidateResult.Yes();
            }

            var answerDomain = this.Container.ResolveDomain<AppealCitsAnswer>();
            using (this.Container.Using(answerDomain))
            {
                var anyAnswersComplete = answerDomain.GetAll().Where(x => x.AppealCits.Id == appeal.Id)
                    .Select(x => x.State.Code)
                    .ToList()
                    .Any(x => x.ToInt() == 2); // готов ответ

                if (!anyAnswersComplete)
                {
                    return ValidateResult.No("Необходимо чтобы присутствовал ответ в статусе 'Готов ответ'");
                }
            }

            return base.Validate(statefulEntity, oldState, newState);
        }
    }
}