namespace Bars.Gkh.Overhaul.Tat.PriorityParams.Impl
{
    using System;
    using Entities;
    using Gkh.Entities;

    public class CapitalGroupPriorityParam : IPriorityParams, IMultiPriorityParam
    {
        public string Id 
        {
            get { return "CapitalGroup"; }
        }

        public string Name 
        {
            get { return "Группа капитальности"; }
        }

        public TypeParam TypeParam 
        {
            get { return TypeParam.Multi; }
        }

        public object GetValue(RealityObjectStructuralElementInProgrammStage3 obj)
        {
            return obj.RealityObject.CapitalGroup != null ? obj.RealityObject.CapitalGroup.Code : null;
        }

        public Type Type 
        {
            get { return typeof (CapitalGroup); }
        }
    }
}
