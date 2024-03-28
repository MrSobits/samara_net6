namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Enum;

    public class NeedOverhaulStructElemPriorityParam : IPriorityParams, IQualitPriorityParam
    {
        public string Id 
        {
            get { return "NeedOverhaulStructElem"; }
        }

        public Type EnumType
        {
            get
            {
                return typeof(NeedOverhaulSeCount);
            }
        }

        public string Name 
        {
            get { return "Количество конструктивных элементов, требующих ремонта (%)"; }
        }

        public TypeParam TypeParam 
        {
            get { return TypeParam.Qualit; }
        }

        public Dictionary<long, int> DictNeedOverhaulPercent { get; set; }

        public  object GetValue(IStage3Entity obj)
        {
            var percent = DictNeedOverhaulPercent.ContainsKey(obj.Id) ? DictNeedOverhaulPercent[obj.Id] : 0;

            return percent > 99 ? NeedOverhaulSeCount.All : 
                           percent > 50 ? NeedOverhaulSeCount.OverFiftyPercent :  NeedOverhaulSeCount.LessFiftyPercent;

        }
    }
}