namespace Bars.Gkh.Overhaul.Hmao.ValueResolver
{
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Formulas;

    public class CalculateByVolumeResolver : FormulaParameterBase
    {
        public StructuralElement StructuralElement { get; set; }

        public override string DisplayName
        {
            get { return "Расчет стоимости по объему"; }
        }

        public override string Code
        {
            get { return Id; }
        }

        public static string Id
        {
            get { return "CalculateByVolume"; }
        }

        public override object GetValue()
        {
            return StructuralElement.CalculateBy == PriceCalculateBy.Volume ? 1 : 0;
        }
    }
}