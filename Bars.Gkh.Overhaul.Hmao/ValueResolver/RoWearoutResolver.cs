namespace Bars.Gkh.Overhaul.Hmao.ValueResolver
{
    using Bars.Gkh.Formulas;

    public class RoWearoutResolver : FormulaParameterBase
    {
        public decimal? PhysicalWear { get; set; }

        public override string DisplayName
        {
            get { return "Физический износ"; }
        }

        public override string Code
        {
            get { return Id; }
        }

        public static string Id
        {
            get { return "RoWear"; }
        }

        public override object GetValue()
        {
            return PhysicalWear.HasValue ? PhysicalWear.Value : 0;
        }
    }
}