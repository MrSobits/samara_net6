namespace Bars.Gkh.Overhaul.Tat.ValueResolver
{
    using Bars.Gkh.Formulas;
    using Castle.Windsor;

    public class ElementLifeTimeResolver : FormulaParameterBase
    {
        public IWindsorContainer Container { get; set; }

        public int LifeTime { get; set; }

        public override string DisplayName
        {
            get
            {
                return "Нормативный срок КЭ";
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
                return "ElementLifeTime";
            }
        }

        public override object GetValue()
        {
            return LifeTime;
        }
    }
}