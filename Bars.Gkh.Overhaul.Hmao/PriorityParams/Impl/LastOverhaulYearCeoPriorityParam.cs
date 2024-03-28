namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System.Collections.Generic;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class LastOverhaulYearCeoPriorityParam : IPriorityParams
    {
        public string Id 
        {
            get { return "LastOverhaulYearCeo"; }
        }

        public string Name
        {
            get { return "Последний год проведения КР ООИ"; }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Quant; }
        }

        public Dictionary<long, int> DictLastOverhaulYearCeo { get; set; }

        public  object GetValue(IStage3Entity obj)
        {
            return DictLastOverhaulYearCeo.ContainsKey(obj.Id)
                ? DictLastOverhaulYearCeo[obj.Id]
                : 0;
        }
    }
}