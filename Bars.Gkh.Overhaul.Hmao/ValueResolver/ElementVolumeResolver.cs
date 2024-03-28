namespace Bars.Gkh.Overhaul.Hmao.ValueResolver
{
    using Bars.Gkh.Formulas;
    using Overhaul.Entities;

    public class ElementVolumeResolver : FormulaParameterBase
    {
        public RealityObjectStructuralElement RealityObjectStructuralElement { get; set; }

        public override string DisplayName
        {
            get { return "Объем работ элемента"; }
        }

        public override string Code
        {
            get { return Id; }
        }

        public static string Id
        {
            get { return "ElementVolume"; }
        }

        public override object GetValue()
        {
            return RealityObjectStructuralElement.Volume;
        }
    }
}