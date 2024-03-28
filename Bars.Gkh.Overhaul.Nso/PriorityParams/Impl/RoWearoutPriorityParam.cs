namespace Bars.Gkh.Overhaul.Nso.PriorityParams.Impl
{
    using System.Collections.Generic;
    using Entities;

    public class RoWearoutPriorityParam : IPriorityParams
    {

        public string Id
        {
            get { return "RoWearout"; }
        }

        public string Name
        {
            get { return "Физический износ МКД (%)"; }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Quant; }
        }

        public object GetValue(IStage3Entity obj)
        {
            return obj.RealityObject.PhysicalWear.HasValue ? obj.RealityObject.PhysicalWear.Value : 0m;
        }
    }
}