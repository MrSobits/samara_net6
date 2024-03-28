namespace Bars.Gkh.Overhaul.Tat.PriorityParams.Impl
{
    using System;
    using Entities;
    using Gkh.Entities.Dicts;

    public class ComplexGroupPriorityParam : IPriorityParams, IMultiPriorityParam
    {
        public string Id
        {
            get { return "ComplexGroup"; }
        }

        public Type Type
        {
            get { return typeof(Work); }
        }

        public string Name
        {
            get { return "Комплексность ремонта по видам работ"; }
        }

        public TypeParam TypeParam
        {
            get { return TypeParam.Multi; }
        }

        public object GetValue(RealityObjectStructuralElementInProgrammStage3 obj)
        {
            return 0;
        }
    }
}