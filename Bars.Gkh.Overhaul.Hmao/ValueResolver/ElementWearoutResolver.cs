namespace Bars.Gkh.Overhaul.Hmao.ValueResolver
{
    using Bars.Gkh.Formulas;
    using Overhaul.Entities;

    public class ElementWearoutResolver : FormulaParameterBase
    {
        public RealityObjectStructuralElement RealityObjectStructuralElement { get; set; }

        public override string DisplayName
        {
            get { return "Физический износ КЭ"; }
        }

        public override string Code
        {
            get { return Id; }
        }

        public static string Id
        {
            get { return "ElementWear"; }
        }

        public override object GetValue()
        {
            return RealityObjectStructuralElement.Wearout;
        }
    }
}