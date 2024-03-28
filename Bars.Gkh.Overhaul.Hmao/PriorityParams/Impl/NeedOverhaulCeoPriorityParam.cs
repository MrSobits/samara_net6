namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System.Collections.Generic;

    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class NeedOverhaulCeoPriorityParam : IPriorityParams
    {
        public string Id 
        {
            get { return "NeedOverhaulCeo"; }
        }

        public string Name 
        {
            get { return "Количество ООИ, подлежащих ремонту"; }
        }

        public TypeParam TypeParam 
        {
            get { return TypeParam.Quant; }
        }

        public Dictionary<long, int> DictNeedOverhaulCeo { get; set; }

        public  object GetValue(IStage3Entity obj)
        {
            return DictNeedOverhaulCeo.ContainsKey(obj.Id) ? DictNeedOverhaulCeo[obj.Id] : 0;
        }
    }
}