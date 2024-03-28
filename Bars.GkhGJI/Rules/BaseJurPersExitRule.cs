namespace Bars.GkhGji.Rules
{
    using B4;
    using Entities;
    using Enums;
    using Castle.Windsor;

    public class BaseJurPersExitRule : IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public int Priority { get { return 0; } }

        public string Code { get { return @"BaseJurPersExitRule"; } }

        public string Name { get { return @"Плановая проверка ЮЛ. Форма проверки «Выездная»"; } }

        public TypeCheck DefaultCode { get { return TypeCheck.PlannedExit; } }

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.Base)
                return false;

            if (entity.Inspection.TypeBase != TypeBase.PlanJuridicalPerson)
                return false;

            var jurPerson = Container.Resolve<IDomainService<BaseJurPerson>>().Load(entity.Inspection.Id);

            if (jurPerson.TypeForm != TypeFormInspection.Exit)
                return false;

            return true;
        }
    }
}