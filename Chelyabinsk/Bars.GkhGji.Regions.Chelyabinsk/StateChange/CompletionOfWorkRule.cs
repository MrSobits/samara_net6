namespace Bars.GkhGji.Regions.Chelyabinsk.StateChange
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.DomainService;

    using Castle.Windsor;

    public abstract class CompletionOfWorkRule : IRuleChangeStatus
    {
        /// <summary>
        /// Статус успешного завершения
        /// </summary>
        protected abstract bool IsSuccess { get; }

        /// <inheritdoc />
        public abstract string Id { get; }

        /// <inheritdoc />
        public abstract string Name { get; }

        /// <inheritdoc />
        public string TypeId => "gji_appeal_citizens";

        /// <inheritdoc />
        public virtual string Description => this.Name;

        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public virtual ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            IDataResult result = null;
            var appeal = statefulEntity as AppealCits;

            if (!(appeal?.AppealUid.HasValue ?? false))
            {
              //  return ValidateResult.Yes();
            }

            this.Container.UsingForResolved<ICitizensAppealServiceClient>((ioc, client) =>
            {
                result = client.ExportInfoCompletionOfWorkResult(appeal, this.IsSuccess);
            });

            if (result?.Success ?? false)
            {
                return ValidateResult.Yes();
            }
            else
            {
                return ValidateResult.No(result?.Message);
            }
        }
    }
}