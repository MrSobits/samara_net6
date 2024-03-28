namespace Bars.Gkh.Overhaul.Nso.ValueResolver
{
    using Bars.Gkh.Formulas;
    using Castle.Windsor;

    public class RoBuildYearResolver : FormulaParameterBase
    {
        public IWindsorContainer Container { get; set; }

        public int? BuildYear { get; set; }

        public override string DisplayName
        {
            get
            {
                return "Год постройки объекта недвижимости";
            }
        }

        public override string Code
        {
            get
            {
                return RoBuildYearResolver.Id;
            }
        }

        public static string Id
        {
            get
            {
                return "RoBuildYear";
            }
        }

        public override object GetValue()
        {
            return BuildYear.HasValue ? BuildYear.Value : 0;
        }
    }
}