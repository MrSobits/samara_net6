namespace Bars.Gkh.Overhaul.Hmao.ValueResolver
{
    using Bars.Gkh.Formulas;

    public class RoBuildYearResolver : FormulaParameterBase
    {
        public int? BuildYear { get; set; }

        public override string DisplayName
        {
            get { return "Год постройки объекта недвижимости"; }
        }

        public override string Code
        {
            get { return Id; }
        }

        public static string Id
        {
            get { return "RoBuildYear"; }
        }

        public override object GetValue()
        {
            return BuildYear.HasValue ? BuildYear.Value : 0;
        }
    }
}