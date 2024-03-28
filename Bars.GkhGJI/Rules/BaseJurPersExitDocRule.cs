namespace Bars.GkhGji.Rules
{
    using B4;
    using Enums;
    using Entities;
    using Castle.Windsor;

    public class BaseJurPersExitDocRule : IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public int Priority { get { return 0; } }

        public string Code { get { return @"BaseJurPersExitDocRule"; } }

        public string Name { get { return @"Плановая проверка ЮЛ. Форма проверки «Выездная и Документарная»"; } }

        public TypeCheck DefaultCode { get { return TypeCheck.PlannedExit; } }

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.Base)
                return false;

            if (entity.Inspection.TypeBase != TypeBase.PlanJuridicalPerson)
                return false;

            var jurPers = Container.Resolve<IDomainService<BaseJurPerson>>().Load(entity.Inspection.Id);

            if (jurPers.TypeForm != TypeFormInspection.ExitAndDocumentary)
                return false;

            return true;
        }
    }
}