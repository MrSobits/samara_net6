namespace Bars.Gkh.Overhaul.Nso.ValueResolver
{
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Overhaul.Nso.ConfigSections;

    using Castle.Windsor;

    public class DpkrEndYearResolver : FormulaParameterBase
    {
        public IWindsorContainer Container { get; set; }

        public OverhaulNsoConfig Config { get; set; }
        
        public override string DisplayName
        {
            get
            {
                return "Год окончания программы";
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
                return "DpkrEndYearResolver";
            }
        }

        public override object GetValue()
        {
            return Config.ProgrammPeriodEnd;
        }
    }
}