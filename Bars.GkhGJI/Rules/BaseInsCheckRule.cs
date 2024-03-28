namespace Bars.GkhGji.Rules
{
    using Entities;
    using Enums;
    using Castle.Windsor;

    public class BaseInsCheckRule : IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public int Priority { get { return 0; } }

        public string Code { get { return @"BaseInsCheckRule"; } }

        public string Name { get { return @"План инспекционных проверок"; } }

        public TypeCheck DefaultCode { get { return TypeCheck.InspectionSurvey; } }

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.Base)
                return false;

            if (entity.Inspection.TypeBase != TypeBase.Inspection)
                return false;

            return true;
        }
    }
}