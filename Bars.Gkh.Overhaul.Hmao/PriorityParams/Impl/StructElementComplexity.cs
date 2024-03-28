namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;
    using Overhaul.Entities;

    public class StructElementComplexity : IPriorityParams
    {
        private readonly IWindsorContainer _container;
        private bool _initialized = false;
        private Dictionary<long, int> _byRo;
        private Dictionary<Tuple<int, long>, int> _elByRoAndYear;

        public string Id { get { return "StructElementComplexity"; } }
        public string Name { get { return "Комплексность КР ООИ (ООИ за год/Все ООИ дома)"; } }
        public TypeParam TypeParam { get { return TypeParam.Quant; } }

        public StructElementComplexity(IWindsorContainer container)
        {
            _container = container;
        }

        public object GetValue(IStage3Entity obj)
        {
            PrepareData();

            var inYear = _elByRoAndYear.Get(Tuple.Create(obj.Year, obj.RealityObject.Id));
            var total = _byRo.Get(obj.RealityObject.Id);

            return total > 0 ? inYear / (decimal)total : 0;
        }

        private void PrepareData()
        {
            if (_initialized) return;

            var domain = _container.ResolveDomain<RealityObjectStructuralElement>();
            var stage3Domain = _container.ResolveDomain<RealityObjectStructuralElementInProgrammStage3>();

            using (_container.Using(domain, stage3Domain))
            {
                // Количество КЭ в доме
                _byRo = domain.GetAll()
				    .Where(x => x.State.StartState)
                    .Select(x => x.RealityObject.Id)
                    .GroupBy(x => x)
                    .Select(x => new { RoId = x.Key, ElCount = x.Count() })
                    .ToList()
                    .ToDictionary(x => x.RoId, x => x.ElCount);

                // Кол-во работ по дому в конкретном году
                _elByRoAndYear = stage3Domain.GetAll()
                    .Select(x => new Tuple<int, long>(x.Year, x.RealityObject.Id))
                    .GroupBy(x => x)
                    .Select(
                        x => new
                        {
                            x.Key,
                            Count = x.Count()
                        })
                    .ToList()
                    .ToDictionary(x => x.Key, x => x.Count);
            }

            _initialized = true;
        }
    }
}