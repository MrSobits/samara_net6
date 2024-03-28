namespace Bars.Gkh.Overhaul.Hmao.PriorityParams.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;

    public class StructElementUsageParams : IPriorityParams
    {
        private readonly IWindsorContainer _container;
        private Dictionary<long, decimal> _data;
        private bool _initialized = false;

        #region IPriorityParams members
        public string Id { get { return "StructElementUsage"; } }
        public string Name { get { return "Эксплуатация КЭ (Срок эксплуатации/Межремонтный срок)"; } }
        public TypeParam TypeParam { get { return TypeParam.Quant; } }
        #endregion

        public StructElementUsageParams(IWindsorContainer container)
        {
            _container = container;
        }

        public object GetValue(IStage3Entity obj)
        {
            PrepareData();

            return _data.Get(obj.Id).RoundDecimal(2);
        }

        private void PrepareData()
        {
            if (_initialized) return;

            _data = new Dictionary<long, decimal>();

            var stage1Domain = _container.ResolveDomain<RealityObjectStructuralElementInProgramm>();
            using (_container.Using(stage1Domain))
            {
                var stage1Items = stage1Domain.GetAll()
                    .Select(
                        x =>
                            new Stage1Node()
                            {
                                Stage3Id = x.Stage2.Stage3.Id,
                                PlanYear = x.Stage2.Stage3.Year,
                                RoId = x.Stage2.RealityObject.Id,
                                RoStructElId = x.StructuralElement.Id,
                                Lifetime = x.StructuralElement.StructuralElement.LifeTime,
                                LifetimeAfterRepair = x.StructuralElement.StructuralElement.LifeTimeAfterRepair,
                                OverhaulYear = x.StructuralElement.LastOverhaulYear,
                                BuildYear = x.Stage2.RealityObject.BuildYear
                            })
                            .ToList()
                            .GroupBy(x => x.RoStructElId)
                            .ToDictionary(x => x.Key, x => x.OrderBy(y => y.PlanYear));

                // Плановый год - год последнего ремонта
                // Для первого элемента из первого этапа данные могут браться из дома
                // Для остальных из предыдущей записи
                // Сортировка по году
                foreach (var stage1Item in stage1Items)
                {
                    Stage1Node prev = null;
                    foreach (var item in stage1Item.Value)
                    {
                        int lastFixYear = 0;
                        int lifetime = 0;

                        if (prev == null)
                        {
                            lastFixYear = item.OverhaulYear != 0 ? item.OverhaulYear : item.BuildYear.GetValueOrDefault();
                            lifetime = item.Lifetime;
                        }
                        else
                        {
                            lastFixYear = prev.PlanYear;
                            lifetime = prev.LifetimeAfterRepair;
                        }

                        prev = item;

                        var formula = lifetime > 0 ? (item.PlanYear - lastFixYear) / (decimal)lifetime : 0;
                        _data[item.Stage3Id] = (formula + _data.Get(item.Stage3Id));
                    }
                }
            }

            _initialized = true;
        }

        private class Stage1Node
        {
            public long Stage3Id { get; set; }
            public int PlanYear { get; set; }
            public long RoId { get; set; }
            public long RoStructElId { get; set; }
            public int Lifetime { get; set; }
            public int LifetimeAfterRepair { get; set; }
            public int OverhaulYear { get; set; }
            public int? BuildYear { get; set; }
        }
    }
}