using Bars.B4;
using Bars.B4.IoC;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.Regions.Tatarstan.Entities.InspectionActionIsolated;
using Bars.GkhGji.Rules;
using Castle.Windsor;

namespace Bars.GkhGji.Regions.Tatarstan.Rules
{
    public class ActionIsolatedDocumentaryRule : IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public int Priority => 1;

        public string Code => "ActionIsolatedDocumentaryRule";

        public string Name => "Проверка по КНМ без взаимодействия. Форма проверки \"Документарная\"";

        public TypeCheck DefaultCode => TypeCheck.NotPlannedDocumentation;

        public bool Validate(Disposal entity)
        {
            if (entity.Inspection.TypeBase != TypeBase.InspectionActionIsolated)
            {
                return false;
            }
            
            var actionIsoladedDomain = this.Container.Resolve<IDomainService<InspectionActionIsolated>>();
            using (this.Container.Using(actionIsoladedDomain))
            {
                var actionIsoladed = actionIsoladedDomain.Get(entity.Inspection.Id);

                return actionIsoladed != null && actionIsoladed.TypeForm == TypeFormInspection.Documentary;
            }
        }
    }
}
