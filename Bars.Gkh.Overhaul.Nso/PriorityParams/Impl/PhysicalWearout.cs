namespace Bars.Gkh.Overhaul.Nso.PriorityParams.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;

    public class PhysicalWearout : IPriorityParams
    {
        public Dictionary<long, decimal> Wearout { get; set; }

        private readonly IWindsorContainer _container;
        private bool _initialized = false;

        public string Id { get { return "PhysicalWearout"; } }
        public string Name { get { return "Физический износ КЭ (износ/70)"; } }
        public TypeParam TypeParam { get{ return TypeParam.Quant; } }

        public PhysicalWearout(IWindsorContainer container)
        {
            _container = container;
        }

        public object GetValue(IStage3Entity obj)
        {
            return Wearout.Get(obj.Id);
        }
    }
}