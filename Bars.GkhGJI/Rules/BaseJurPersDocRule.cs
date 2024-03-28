namespace Bars.GkhGji.Rules
{
    using B4;
    using Entities;
    using Enums;
    using Castle.Windsor;

    public class BaseJurPersDocRule : IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public int Priority { get { return 0; } }

        public string Code { get { return @"BaseJurPersDocRule"; } }

        public string Name { get { return @"Плановая проверка ЮЛ. Форма проверки «Документарная»"; } }

        public TypeCheck DefaultCode { get { return TypeCheck.PlannedDocumentation; } }

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.Base)
                return false;

            if (entity.Inspection.TypeBase != TypeBase.PlanJuridicalPerson)
                return false;

            var jurPers = Container.Resolve<IDomainService<BaseJurPerson>>().Load(entity.Inspection.Id);

            if (jurPers.TypeForm != TypeFormInspection.Documentary)
                return false;

            return true;
        }
    }
}