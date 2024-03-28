namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
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
        private readonly IWindsorContainer _container;
        private Dictionary<long, decimal> _data;
        private bool _initialized = false;

        public string Id { get { return "PhysicalWearout"; } }
        public string Name { get { return "Физический износ КЭ (износ/70)"; } }
        public TypeParam TypeParam { get { return TypeParam.Quant; } }

        public object GetValue(IStage3Entity obj)
        {
            PrepareData();

            return _data.Get(obj.Id);
        }

        public PhysicalWearout(IWindsorContainer container)
        {
            _container = container;
        }

        private void PrepareData()
        {
            if (_initialized) return;

            _data = new Dictionary<long, decimal>();

            var stage1Domain = _container.ResolveDomain<RealityObjectStructuralElementInProgramm>();
            using (_container.Using(stage1Domain))
            {
                // Сумма износов всех КЭ по дому по конкретному КЭ
                _data = stage1Domain.GetAll()
                    .Select(
                        x => new
                        {
                            x.StructuralElement.Wearout,
                            Stage3Id = x.Stage2.Stage3.Id
                        })
                    .ToList()
                    .GroupBy(x => x.Stage3Id)
                    .Select(x => new { x.Key, Wearout = x.Sum(y => y.Wearout / 70) })
                    .ToDictionary(x => x.Key, x => x.Wearout);
            }

            _initialized = true;
        }
    }
}