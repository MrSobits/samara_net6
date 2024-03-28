namespace Bars.GkhGji.Rules
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class BaseJurPersVisitRule : IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public int Priority => 0;

        public string Code => nameof(BaseJurPersVisitRule);

        public string Name => "Плановая проверка ЮЛ, Форма проверки Инспекционный визит";

        public TypeCheck DefaultCode => TypeCheck.PlannedInspectionVisit;

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.Base || entity.Inspection.TypeBase != TypeBase.PlanJuridicalPerson)
            {
                return false;
            }

            var domain = this.Container.Resolve<IDomainService<BaseJurPerson>>();

            using (this.Container.Using(domain))
            {
                var inspection = domain.Load(entity.Inspection.Id);

                if (inspection.TypeForm != TypeFormInspection.InspectionVisit)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
