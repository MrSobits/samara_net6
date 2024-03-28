namespace Bars.Gkh.Overhaul.Nso.ValueResolver
{
    using Bars.Gkh.Formulas;
    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Entities;

    public class RoWearoutResolver : FormulaParameterBase
    {
        public IWindsorContainer Container { get; set; }

        public RealityObjectStructuralElement RealityObjectStructuralElement { get; set; }

        //public RealityObject RealityObject { get; set; }

        public decimal? PhysicalWear { get; set; }

        public StructuralElement StructuralElement { get; set; }

        public override string DisplayName
        {
            get
            {
                return "Физический износ";
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
                return "RoWear";
            }
        }

        public override object GetValue()
        {
            return PhysicalWear.HasValue ? PhysicalWear.Value : 0;
        }
    }
}