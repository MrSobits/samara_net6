namespace Bars.Gkh.Overhaul.Nso.ValueResolver
{
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;

    using Castle.Windsor;

    public class CurrentYearResolver : FormulaParameterBase
    {
        public IWindsorContainer Container { get; set; }

        public OverhaulNsoConfig Config { get; set; }

        public override string DisplayName
        {
            get
            {
                return "Начальный год программы";
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
                return "CurrentYear";
            }
        }

        public override object GetValue()
        {
            return Config.ProgrammPeriodStart;
        }
    }
}