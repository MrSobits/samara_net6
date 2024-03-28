namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System.Collections.Generic;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class LastOverhaulYearPriorityParam : IPriorityParams
    {
        public string Id 
        {
            get { return "LastOverhaulYear"; }
        }

        public string Name
        {
            get { return "Дата последнего КР в МКД (в т.ч. отдельных КЭ)"; }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Quant; }
        }

        public Dictionary<long, int> DictLastOverhaulYear { get; set; }

        public  object GetValue(IStage3Entity obj)
        {
            return DictLastOverhaulYear.ContainsKey(obj.RealityObject.Id)
                ? DictLastOverhaulYear[obj.RealityObject.Id]
                : 0;
        }
    }
}