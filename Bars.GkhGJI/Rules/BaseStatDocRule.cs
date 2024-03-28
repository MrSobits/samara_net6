using Bars.B4;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Castle.Windsor;

namespace Bars.GkhGji.Rules
{
    public class BaseStatDocRule : IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public int Priority { get { return 0; } }

        public string Code { get { return @"BaseStatDocRule"; } }

        public string Name { get { return @"Проверка по обращению, Форма проверки «Документарная»"; } }

        public TypeCheck DefaultCode { get { return TypeCheck.NotPlannedDocumentation; } }

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.Base)
                return false;

            if (entity.Inspection.TypeBase != TypeBase.CitizenStatement)
                return false;

            var baseStat = Container.Resolve<IDomainService<BaseStatement>>().Load(entity.Inspection.Id);

            if (baseStat.TypeForm != TypeFormInspection.Documentary)
                return false;

            return true;
        }
    }
}