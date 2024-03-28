namespace Bars.Gkh.Overhaul.Tat.ValueResolver
{
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class DpkrEndYearResolver : FormulaParameterBase
    {
        public IWindsorContainer Container { get; set; }
        
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
            var config = Container.GetGkhConfig<OverhaulTatConfig>();
            return config.ProgrammPeriodEnd;
        }
    }
}