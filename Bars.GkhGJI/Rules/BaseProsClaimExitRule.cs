namespace Bars.GkhGji.Rules
{
    using B4;
    using Entities;
    using Enums;
    using Castle.Windsor;

    public class BaseProsClaimExitRule : IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public int Priority { get { return 0; } }

        public string Code { get { return @"BaseProsClaimExitRule"; }}

        public string Name { get { return @"Проверка по требованию прокуратуры, форма проверки не документарная "; } }

        public TypeCheck DefaultCode { get { return TypeCheck.NotPlannedExit;} }

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.Base)
                return false;

            if (entity.Inspection.TypeBase != TypeBase.ProsecutorsClaim)
                return false;

            var prosClaim = Container.Resolve<IDomainService<BaseProsClaim>>().Load(entity.Inspection.Id);

            if (prosClaim.TypeForm == TypeFormInspection.Documentary)
                return false;

            return true;
        }
    }
}