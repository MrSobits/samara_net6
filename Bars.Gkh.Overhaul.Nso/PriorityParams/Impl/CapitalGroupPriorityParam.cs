namespace Bars.Gkh.Overhaul.Nso.PriorityParams.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Gkh.Entities;
    using Entities;

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

        public object GetValue(IStage3Entity obj)
        {
            return null;
        }

        public Type Type
        {
            get { return typeof(CapitalGroup); }
        }

        public bool CheckContains(IStage3Entity obj, IEnumerable<StoredMultiValue> value)
        {
            if (obj.RealityObject.CapitalGroup == null)
            {
                return false;
            }

            return !value.Select(x => x.Id).Contains(obj.RealityObject.CapitalGroup.Id);
        }
    }
}
