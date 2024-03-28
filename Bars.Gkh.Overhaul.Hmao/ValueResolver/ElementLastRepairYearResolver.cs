namespace Bars.Gkh.Overhaul.Hmao.ValueResolver
{
    using Bars.Gkh.Formulas;
    using Overhaul.Entities;

    public class ElementLastRepairYearResolver : FormulaParameterBase
    {
        public RealityObjectStructuralElement RealityObjectStructuralElement { get; set; }

        public int? BuildYear { get; set; }

        public override string DisplayName
        {
            get { return "Год последнего ремонта элемента"; }
        }

        public override string Code
        {
            get { return Id; }
        }

        public static string Id
        {
            get { return "ElementLastRepairYear"; }
        }

        public override object GetValue()
        {
            return RealityObjectStructuralElement.LastOverhaulYear > 0
                ? RealityObjectStructuralElement.LastOverhaulYear
                : BuildYear ?? 0;
        }
    }
}