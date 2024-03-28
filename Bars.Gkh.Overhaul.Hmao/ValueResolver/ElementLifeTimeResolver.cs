namespace Bars.Gkh.Overhaul.Hmao.ValueResolver
{
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Formulas;

    public class ElementLifeTimeResolver : FormulaParameterBase
    {
        public StructuralElement StructuralElement { get; set; }

        public override string DisplayName
        {
            get { return "Нормативный срок КЭ"; }
        }

        public override string Code
        {
            get { return Id; }
        }

        public static string Id
        {
            get { return "ElementLifeTime"; }
        }

        public override object GetValue()
        {
            return StructuralElement.LifeTime;
        }
    }
}