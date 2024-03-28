namespace Bars.Gkh.Overhaul.Nso.Import.Program
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Import;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    using NHibernate.Linq;

    public sealed class RealityObjectWorkHelper
    {
        // ключ - ООИ id
        // значение - id группы КЭ
        private Dictionary<long, long[]> _commonEstateToStructElGroup;

        // ключ - id группы КЭ
        // значение - коллекция id КЭ
        private Dictionary<long, long[]> _structElGroupToStructEls;

        // ключ - id дома
        // значение - коллекция id КЭ
        private Dictionary<long, long[]> _roToStructEls;

        // ключ - КЭ id
        // значение - id работы
        private Dictionary<long, long> _structElToWork;

        // ключ - id работы
        // значение - id источника финансирования
        private Dictionary<long, long> _workToFinSource;

        private readonly IWindsorContainer _container;

        public RealityObjectWorkHelper(IWindsorContainer container)
        {
            _container = container;
        }

        public void InitDictionaries()
        {
            var structElGroupDomain = _container.ResolveDomain<StructuralElementGroup>();

            var structElDomain = _container.ResolveDomain<StructuralElement>();

            var roStructElDomain = _container.ResolveDomain<RealityObjectStructuralElement>();

            var structElWorkDomain = _container.ResolveDomain<StructuralElementWork>();

            var finSourceWorkDomain = _container.ResolveDomain<FinanceSourceWork>();

            using(_container.Using(structElGroupDomain, structElDomain, roStructElDomain, structElWorkDomain, finSourceWorkDomain))
            {
                _commonEstateToStructElGroup = structElGroupDomain.GetAll()
                    .Fetch(x => x.CommonEstateObject)
                    .Select(x => new
                    {
                        x.Id,
                        CommonEstateObjectId = x.CommonEstateObject.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.CommonEstateObjectId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToArray());

                _structElGroupToStructEls = structElDomain.GetAll()
                    .Fetch(x => x.Group)
                    .Select(x => new
                    {
                        x.Id,
                        GroupId = x.Group.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.GroupId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToArray());

                _roToStructEls = roStructElDomain.GetAll()
                    .Fetch(x => x.RealityObject)
                    .Fetch(x => x.StructuralElement)
                    .Select(x => new
                    {
                        RealityObjectId = x.RealityObject.Id,
                        StructuralElementId = x.StructuralElement.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RealityObjectId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.StructuralElementId).ToArray());

                _structElToWork = structElWorkDomain.GetAll()
                    .Fetch(x => x.StructuralElement)
                    .Fetch(x => x.Job)
                    .Select(x => new
                    {
                        StructElId = x.StructuralElement.Id,
                        WorkId = x.Job.Work.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.StructElId)
                    .ToDictionary(x => x.Key, x => x.First().WorkId);

                _workToFinSource = finSourceWorkDomain.GetAll()
                    .Fetch(x => x.FinanceSource)
                    .Fetch(x => x.Work)
                    .Select(x => new
                    {
                        FinSourceId = x.FinanceSource.Id,
                        WorkId = x.Work.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.WorkId)
                    .ToDictionary(x => x.Key, x => x.First().FinSourceId);
            }
        }

        public CommonEstateWorkInfo GetWorkInfo(long commonEstateId, long roId, out string error)
        {
            error = string.Empty;
            var group = _commonEstateToStructElGroup.ContainsKey(commonEstateId) ? _commonEstateToStructElGroup[commonEstateId] : null;
            if (group == null)
            {
                error = "Для ООИ {0} не найдены группы конструктивного элемента".FormatUsing(commonEstateId);
                return null;
            }

            var structElsInGroup = group.SelectMany(x => _structElGroupToStructEls.ContainsKey(x) ? _structElGroupToStructEls[x] : new long[0]).ToArray();
            if (structElsInGroup.Length == 0)
            {
                error = "Для групп КЭ {0} ООИ {1} не найден список КЭ".FormatUsing(string.Join(",", group), commonEstateId);
                return null;
            }

            var structElsInRo = _roToStructEls.ContainsKey(roId) ? _roToStructEls[roId] : null;
            if (structElsInRo == null)
            {
                error = "Для жилого дома {0} не найден список КЭ".FormatUsing(roId);
                return null;
            }

            var structEls = structElsInGroup.Where(x => structElsInRo.Any(y => x == y)).ToArray();
            if (structEls.Length == 0)
            {
                error = "Жилой дом {0} не имеет конструктивных элементов из группы {1} для ООИ {2}".FormatUsing(roId, string.Join(",", group), commonEstateId);
                return null;
            }

            var work = _structElToWork.ContainsKey(structEls[0]) ? _structElToWork[structEls[0]] : (long?)null;
            if (!work.HasValue)
            {
                error = "Для КЭ {0} дома {1} не найден работа".FormatUsing(structEls[0], roId);
                return null;
            }

            var finSource = _workToFinSource.ContainsKey(work.Value) ? _workToFinSource[work.Value] : (long?)null;
            if (!finSource.HasValue)
            {
                error = "Для работы {0} КЭ {1} дома {2} не найден фин. источник".FormatUsing(work.Value, structEls[0], roId);
                return null;
            }

            return new CommonEstateWorkInfo
            {
                WorkId = work.Value,
                FinSourceId = finSource.Value
            };
        }
    }

    public sealed class CommonEstateWorkInfo
    {
        public long WorkId { get; set; }

        public long FinSourceId { get; set; }
    }
}