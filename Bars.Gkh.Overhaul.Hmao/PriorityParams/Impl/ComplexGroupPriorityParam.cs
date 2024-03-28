namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System;
    using System.Collections.Generic;
    using Entities;
    using Gkh.Entities.Dicts;

    public class ComplexGroupPriorityParam : IPriorityParams, IMultiPriorityParam
    {
        public string Id
        {
            get { return "ComplexGroup"; }
        }

        public string Name
        {
            get { return "Комплексность ремонта по видам работ"; }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Multi; }
        }

        public Type Type
        {
            get { return typeof(Work); }
        }

        public  object GetValue(IStage3Entity obj)
        {
            return null;
        }

        public bool CheckContains(IStage3Entity obj, IEnumerable<StoredMultiValue> value)
        {
            return false;
        }
    }
}