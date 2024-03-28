using Bars.B4;

namespace Bars.GkhGji.Rules
{
    using Enums;
    using Entities;
    using Castle.Windsor;

    public class BaseProsClaimDocRule : IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public int Priority { get { return 0; } }

        public string Code { get { return @"BaseProsClaimRule"; } }

        public string Name { get { return @"Проверка по требованию прокуратуры, форма проверки документарная"; } }

        public TypeCheck DefaultCode { get { return TypeCheck.NotPlannedDocumentation; } }

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.Base)
                return false;

            if (entity.Inspection.TypeBase != TypeBase.ProsecutorsClaim)
                return false;

            var prosClaim = Container.Resolve<IDomainService<BaseProsClaim>>().Load(entity.Inspection.Id);

            if (prosClaim.TypeForm != TypeFormInspection.Documentary)
                return false;

            return true;
        }
    }
}