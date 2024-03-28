namespace Bars.Gkh.Overhaul.Tat.ValueResolver
{
    using B4.Utils;
    using Bars.Gkh.Formulas;
    using Castle.Windsor;

    public class ElementLastRepairYearResolver : FormulaParameterBase
    {
        public IWindsorContainer Container { get; set; }

        public int LastOverhaulYear { get; set; }

        public int? BuildYear { get; set; }

        public override string DisplayName
        {
            get
            {
                return "Год последнего ремонта элемента";
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
                return "ElementLastRepairYear";
            }
        }

        public override object GetValue()
        {
            return LastOverhaulYear > 0 ? LastOverhaulYear : BuildYear.ToInt();
        }
    }
}