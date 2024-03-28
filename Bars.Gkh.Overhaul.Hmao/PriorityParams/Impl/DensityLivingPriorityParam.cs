namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using Entities;

    public class DensityLivingPriorityParam : IPriorityParams
    {
        public string Id
        {
            get { return "DensityLiving"; }
        }

        public string Name
        {
            get { return "Плотность населения в доме"; }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Quant; }
        }

        public  object GetValue(IStage3Entity obj)
        {
            var areaLiving = obj.RealityObject.AreaLiving.GetValueOrDefault(0m);
            var numberEntrancies = obj.RealityObject.NumberLiving.GetValueOrDefault(0);

            return areaLiving > 0 ? numberEntrancies/areaLiving : 0;
        }
    }
}