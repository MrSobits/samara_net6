namespace Bars.GkhGji.Rules
{
    using B4;
    using Entities;
    using Enums;
    using Castle.Windsor;

    public class BaseStatVisualRule : IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public int Priority { get { return 0; }}

        public string Code { get { return @"BaseStatVisualRule"; } }

        public string Name { get { return @"Проверка по обращению, Форма проверки «Визуальное обследование»"; } }

        public TypeCheck DefaultCode { get { return TypeCheck.VisualSurvey; } }

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.Base)
                return false;

            if (entity.Inspection.TypeBase != TypeBase.CitizenStatement)
                return false;

            var baseStat = Container.Resolve<IDomainService<BaseStatement>>().Load(entity.Inspection.Id);

            if (baseStat.TypeForm != TypeFormInspection
                .Visual)
                return false;

            return true;
        }
    }
}