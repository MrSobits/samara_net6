namespace Bars.Gkh.Overhaul.Nso.ValueResolver
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Formulas;
    using Castle.Windsor;
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Entities;

    public class ElementVolumeResolver : FormulaParameterBase
    {
        public IWindsorContainer Container { get; set; }

        public RealityObjectStructuralElement RealityObjectStructuralElement { get; set; }

        public RealityObject RealityObject { get; set; }

        public StructuralElement StructuralElement { get; set; }
        
        public override string DisplayName
        {
            get
            {
                return "Объем работ элемента";
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
                return "ElementVolume";
            }
        }

        public override object GetValue()
        {
            return RealityObjectStructuralElement.Volume;
        }
    }
}