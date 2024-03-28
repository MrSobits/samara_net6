namespace Bars.Gkh.Overhaul.Nso.PriorityParams.Impl
{
    using System.Collections.Generic;
    using Entities;

    public class WearoutPriorityParam : IPriorityParams
    {
        public Dictionary<long, decimal> DictRoWearout { get; set; }

        public string Id
        {
            get { return "Wearout"; }
        }

        public string Name
        {
            get { return "Тех. состояние (износ) МКД (%)"; }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Quant; }
        }

        public object GetValue(IStage3Entity obj)
        {
            return DictRoWearout.ContainsKey(obj.RealityObject.Id) ? DictRoWearout[obj.RealityObject.Id] : 0;
        }
    }
}