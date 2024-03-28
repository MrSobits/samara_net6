namespace Bars.Gkh.Overhaul.Nso.PriorityParams.Impl
{
    using System;
    using System.Collections.Generic;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;

    public class StructElementComplexity : IPriorityParams
    {
        private readonly IWindsorContainer _container;
        private bool _initialized = false;

        public Dictionary<long, int> CountWorksByRo { get; set; }
        public Dictionary<Tuple<int, long>, int> CountWorksInYear { get; set; }

        public string Id { get { return "StructElementComplexity"; } }
        public string Name { get { return "Комплексность КР ООИ (ООИ за год/Все ООИ дома)"; } }
        public TypeParam TypeParam { get{ return TypeParam.Quant; } }

        public StructElementComplexity(IWindsorContainer container)
        {
            _container = container;
        }

        public object GetValue(IStage3Entity obj)
        {
            var inYear = CountWorksInYear.Get(Tuple.Create(obj.Year, obj.RealityObject.Id));
            var total = CountWorksByRo.Get(obj.RealityObject.Id);

            return total > 0 ? inYear / (decimal) total : 0;
        }
    }
}