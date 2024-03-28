namespace Bars.GkhGji.Regions.Chelyabinsk.StateChange
{
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.DomainService;

    using Castle.Windsor;

    public abstract class WorkRule : IRuleChangeStatus
    {
        /// <summary>
        /// Статус принятия в работу
        /// </summary>
        protected abstract bool IsAccept { get; }

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
            var appeal = statefulEntity as AppealCits;

            if (!(appeal?.AppealUid.HasValue ?? false))
            {
                return ValidateResult.Yes();
            }

            var client = this.Container.Resolve<ICitizensAppealServiceClient>();
            
            using (this.Container.Using(client))
            {
                var result = client.ExportInfoAcceptWorkResult(appeal, this.IsAccept);

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
}