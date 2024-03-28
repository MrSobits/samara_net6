using Castle.Windsor;

namespace Bars.GkhGji.Rules
{
    using Entities;
    using Enums;

    public class BaseActTsjRule : IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public int Priority { get { return 0; } }

        public string Code { get { return @"BaseActTsjRule"; } }

        public string Name { get { return @"Проверка деятельности ТСЖ"; } }

        public TypeCheck DefaultCode { get { return TypeCheck.InspectionSurvey; } }

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.Base)
                return false;

            return entity.Inspection.TypeBase == TypeBase.ActivityTsj;
        }
    }
}