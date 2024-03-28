namespace Bars.Gkh.Overhaul.Tat.ValueResolver
{
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class CurrentYearResolver : FormulaParameterBase
    {
        public IWindsorContainer Container { get; set; }

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
            var config = Container.GetGkhConfig<OverhaulTatConfig>();
            return config.ProgrammPeriodStart;
        }
    }
}