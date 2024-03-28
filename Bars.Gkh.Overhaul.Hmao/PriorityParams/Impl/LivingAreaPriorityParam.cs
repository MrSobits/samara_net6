namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class LivingAreaPriorityParam : IPriorityParams
    {
        public string Id 
        {
            get { return "LivingArea"; }
        }

        public string Name
        {
            get { return "Общая площадь жилых помещений"; }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Quant; }
        }

        public  object GetValue(IStage3Entity obj)
        {
            return obj.RealityObject.AreaLiving;
        }
    }
}