namespace Bars.GkhGji.Regions.Tatarstan.Rules
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionPreventiveAction;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;

    public class InspectionPrevActionDocumentaryRule : IKindCheckRule
    {
        /// <inheritdoc />
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public int Priority => 1;

        /// <inheritdoc />
        public string Code => nameof(InspectionPrevActionDocumentaryRule);

        /// <inheritdoc />
        public string Name => "Проверка по профилактическому мероприятию. Документарная";

        /// <inheritdoc />
        public TypeCheck DefaultCode => TypeCheck.NotPlannedDocumentation;

        /// <inheritdoc />
        public bool Validate(Disposal entity)
        {
            if (entity.Inspection.TypeBase != TypeBase.InspectionPreventiveAction)
            {
                return false;
            }

            var inspectionDomain = this.Container.ResolveDomain<InspectionPreventiveAction>();
            using (this.Container.Using(inspectionDomain))
            {
                return inspectionDomain.GetAll()
                    .Any(x => x.Id == entity.Inspection.Id && x.TypeForm == TypeFormInspection.Documentary);
            }
        }
    }
}