namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System;
    using System.Collections.Generic;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class DecisionSetMinAmountPriorityParam : IPriorityParams, IQualitPriorityParam
    {
        public string Id
        {
            get { return "DecisionSetMinAmount"; }
        }

        public Type EnumType {
            get
            {
                return typeof(YesNo);
            }
        }

        public string Name
        {
            get { return "Наличие решения об уплате взноса в размере, превышающем установленный"; }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Qualit; }
        }

        public HashSet<long> SetDecisions { get; set; }

        public  object GetValue(IStage3Entity obj)
        {
            return SetDecisions.Contains(obj.RealityObject.Id)
                ? YesNo.Yes
                : YesNo.No;
        }
    }
}