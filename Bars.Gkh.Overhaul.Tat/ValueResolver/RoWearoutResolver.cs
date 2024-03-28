namespace Bars.Gkh.Overhaul.Tat.ValueResolver
{
    using Bars.B4.Utils;
    using Bars.Gkh.Formulas;
    using Castle.Windsor;

    public class RoWearoutResolver : FormulaParameterBase
    {
        public IWindsorContainer Container { get; set; }

        public decimal? PhysicalWear { get; set; }

        public override string DisplayName
        {
            get
            {
                return "Физический износ";
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
                return "RoWear";
            }
        }

        public override object GetValue()
        {
            return PhysicalWear.ToInt();
        }
    }
}