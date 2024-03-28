namespace Bars.GkhGji.Rules
{
    using Entities;
    using Enums;

    using Castle.Windsor;

    public class BaseHeatSeasonRule : IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public int Priority { get { return 0; } }

        public string Code { get { return @"BaseHeatSeasonRule"; } }

        public string Name { get { return @"Проверка по подготовке к отопительному сезону"; } }

        public TypeCheck DefaultCode { get { return TypeCheck.InspectionSurvey; }}

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.Base)
                return false;

            return entity.Inspection.TypeBase == TypeBase.HeatingSeason;
        }
    }
}