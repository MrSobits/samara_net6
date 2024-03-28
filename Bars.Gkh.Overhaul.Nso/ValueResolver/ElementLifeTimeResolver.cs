namespace Bars.Gkh.Overhaul.Nso.ValueResolver
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Formulas;
    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Entities;

    public class ElementLifeTimeResolver : FormulaParameterBase
    {
        public IWindsorContainer Container { get; set; }

        public RealityObjectStructuralElement RealityObjectStructuralElement { get; set; }

        public RealityObject RealityObject { get; set; }

        public StructuralElement StructuralElement { get; set; }
        
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
            return StructuralElement.LifeTime;
        }
    }
}