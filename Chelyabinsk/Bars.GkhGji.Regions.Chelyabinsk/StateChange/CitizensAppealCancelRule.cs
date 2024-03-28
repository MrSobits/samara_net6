namespace Bars.GkhGji.Regions.Chelyabinsk.StateChange
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.DomainService;

    using Castle.Windsor;

    public abstract class CitizensAppealCancelRule : IRuleChangeStatus
    {
        /// <summary>
        /// Статус успешной отмены
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
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            IDataResult result = null;
            var appeal = statefulEntity as AppealCits;

            if (!(appeal?.AppealUid.HasValue ?? false))
            {
                return ValidateResult.Yes();
            }
            
            this.Container.UsingForResolved<ICitizensAppealServiceClient>((ioc, client) =>
            {
                result = client.ExportInfoCitizensAppealCancelResult(appeal, this.IsSuccess);

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