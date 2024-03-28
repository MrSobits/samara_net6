namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System.Collections.Generic;

    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class PhysicalWearCeoPriorityParam : IPriorityParams
    {
        public string Id 
        {
            get { return "PhysicalWearCeo"; }
        }

        public string Name 
        {
            get { return "Физический износ ООИ"; }
        }

        public TypeParam TypeParam 
        {
            get { return TypeParam.Quant; }
        }

        public Dictionary<long, decimal> DictPhysicalWearCeo { get; set; }

        public  object GetValue(IStage3Entity obj)
        {
            return DictPhysicalWearCeo.ContainsKey(obj.Id) ? DictPhysicalWearCeo[obj.Id] : 0;
        }
    }
}