namespace Bars.Gkh.Overhaul.Hmao.ValueResolver
{
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    public class DpkrEndYearResolver : FormulaParameterBase
    {
        public OverhaulHmaoConfig OverhaulHmaoConfig { get; set; }

        public override string DisplayName
        {
            get { return "Год окончания программы"; }
        }

        public override string Code
        {
            get { return Id; }
        }

        public static string Id
        {
            get { return "DpkrEndYearResolver"; }
        }

        public override object GetValue()
        {
            return OverhaulHmaoConfig.ProgrammPeriodEnd;
        }
    }
}