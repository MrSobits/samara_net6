namespace Bars.Gkh.Overhaul.Hmao.ValueResolver
{
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    public class CurrentYearResolver : FormulaParameterBase
    {
        public OverhaulHmaoConfig OverhaulHmaoConfig { get; set; }

        public override string DisplayName
        {
            get { return "Начальный год программы"; }
        }

        public override string Code
        {
            get { return Id; }
        }

        public static string Id
        {
            get { return "CurrentYear"; }
        }

        public override object GetValue()
        {
            return OverhaulHmaoConfig.ProgrammPeriodStart;
        }
    }
}