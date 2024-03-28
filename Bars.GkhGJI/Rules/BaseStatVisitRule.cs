namespace Bars.GkhGji.Rules
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    public class BaseStatVisitRule: IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public int Priority => 0;

        public string Code => nameof(BaseStatVisitRule);

        public string Name => "Проверка по обращению, Форма проверки Инспекционный визит";

        public TypeCheck DefaultCode => TypeCheck.NotPlannedInspectionVisit;

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.Base || entity.Inspection.TypeBase != TypeBase.CitizenStatement)
            {
                return false;
            }

            var domain = this.Container.Resolve<IDomainService<BaseStatement>>();

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
