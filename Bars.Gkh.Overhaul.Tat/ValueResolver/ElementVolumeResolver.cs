namespace Bars.Gkh.Overhaul.Tat.ValueResolver
{
    using Bars.Gkh.Formulas;
    using Castle.Windsor;

    public class ElementVolumeResolver : FormulaParameterBase
    {
        public IWindsorContainer Container { get; set; }

        public decimal Volume { get; set; }
        
        public override string DisplayName
        {
            get { return "Объем работ элемента"; }
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
                return "ElementVolume";
            }
        }

        public override object GetValue()
        {
            return Volume;
        }
    }
}