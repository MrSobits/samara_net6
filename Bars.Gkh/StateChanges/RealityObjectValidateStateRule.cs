namespace Bars.Gkh.StateChanges
{
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.States;

    using Bars.B4.IoC;
    using Bars.B4.Modules.Mapping;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Entities;

    public class RealityObjectValidateStateRule : IRuleChangeStatus
    {
        public virtual IWindsorContainer Container { get; set; }

        public string Id => "RealityObjectValidateStateRule";

        public string Name => "Проверка заполненности данных жилого дома";

        public string TypeId => "gkh_real_obj";

        public string Description => "Проверка заполненности данных жилого дома";

        /// <inheritdoc />
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var ro = statefulEntity as RealityObject;

            if (ro == null)
            {
                return ValidateResult.No("Внутренняя ошибка.");
            }

            var fieldRequirementDomain = this.Container.ResolveDomain<FieldRequirement>();
            var structElService = this.Container.Resolve<IRealityObjectStructuralElementService>();
            var protocolService = this.Container.Resolve<IRealityObjectDecisionProtocolProxyService>();

            using (this.Container.Using(fieldRequirementDomain, structElService, protocolService))
            {
                var entityDescriptor = MappingSchema.GetEntityDescriptor<RealityObject>();

                var roFields = entityDescriptor.GetPropertyMapDescriptors()
                    .Select(
                        x => new
                        {
                            PropertyName = x.Key.Name,
                            Description = x.Value.Name,
                            Value = x.Key.GetValue(statefulEntity) != null
                        })
                    .ToList();

                var roRequirements = fieldRequirementDomain.GetAll()
                    .Where(x => x.RequirementId.StartsWith("Gkh.RealityObject.Field"))
                    .Select(x => x.RequirementId.Replace("Gkh.RealityObject.Field.", string.Empty).Replace("_Rqrd", string.Empty))
                    .ToList();

                var blankFields = roFields
                    .Where(x => !x.Value)
                    .Where(x => roRequirements.Contains(x.PropertyName))
                    .Select(x => x.Description)
                    .ToList();

                if (blankFields.Any())
                {
                    var text = blankFields.AggregateWithSeparator(x => x, ", ");

                    return ValidateResult.No($"Заполнены не все обязательные поля карточки дома: {text}");
                }

                var checkStructEl = structElService.CheckBlankFields(ro);

                if (!checkStructEl.Success)
                {
                    return ValidateResult.No(checkStructEl.Message);
                }

                var checkprotocol = protocolService.CheckDecisionProtocol(ro);

                if (!checkprotocol.Success)
                {
                    return ValidateResult.No(checkprotocol.Message);
                }

                return ValidateResult.Yes();
            }
        }
    }
}
