namespace Bars.Gkh.Overhaul.Nso.ValueResolver
{
    using B4.Utils;
    using Bars.Gkh.Formulas;
    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Entities;

    public class ElementLastRepairYearResolver : FormulaParameterBase
    {
        public IWindsorContainer Container { get; set; }

        public RealityObjectStructuralElement RealityObjectStructuralElement { get; set; }

        public int? BuildYear { get; set; }

        public StructuralElement StructuralElement { get; set; }

        public override string DisplayName
        {
            get
            {
                return "Год последнего ремонта элемента";
            }
        }

        public override string Code
        {
            get
            {
                return Id;
            }
        }

        public static string Id
        {
            get
            {
                return "ElementLastRepairYear";
            }
        }

        public override object GetValue()
        {
            return RealityObjectStructuralElement.LastOverhaulYear != 0 
                ? RealityObjectStructuralElement.LastOverhaulYear
                : BuildYear.ToInt();
        }
    }
}