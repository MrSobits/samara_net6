namespace Bars.Gkh.DomainService.EfficiencyRating.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.MetaValueConstructor;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.EfficiencyRating;
    using Bars.Gkh.Extensions;
    using Bars.Gkh.MetaValueConstructor.DataFillers;
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.MetaValueConstructor.Enums;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис для работы с мета-значениями для Рейтинга Эффективности
    /// </summary>
    public class EfficiencyRatingDataValueService : AbstractDataValueService, IEfficiencyRatingService
    {
        private readonly IList<EfficiencyRatingPeriod> periods;

        /// <summary>
        /// Домен-сервис <see cref="ManagingOrganizationEfficiencyRating"/>
        /// </summary>
        public IDomainService<ManagingOrganizationEfficiencyRating> ManagingOrganizationEfficiencyRatingDomainService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ManagingOrganizationDataValue"/>
        /// </summary>
        public IDomainService<ManagingOrganizationDataValue> ManorgDataValueDomainService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ManagingOrganization"/>
        /// </summary>
        public IDomainService<ManagingOrganization> ManagingOrganizationDomainService { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="EfficiencyRatingPeriod"/>
        /// </summary>
        public IDomainService<EfficiencyRatingPeriod> EfficiencyRatingPeriodDomainService { get; set; }

        /// <summary>
        /// Интерфейс сервиса по заполнению данными объектов-значений
        /// </summary>
        public IDataFillerService DataFillerService { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="periodDomain">Домен-сервис <see cref="EfficiencyRatingPeriod"/></param>
        public EfficiencyRatingDataValueService(IDomainService<EfficiencyRatingPeriod> periodDomain)
        {
            this.periods = periodDomain.GetAll().OrderBy(x => x.DateStart).ToList();
        }

        /// <inheritdoc />
        public override DataMetaObjectType ConstructorType => DataMetaObjectType.EfficientcyRating;

        /// <inheritdoc />
        public override IDataResult GetMetaValues(BaseParams baseParams)
        {
            var efManorgId = baseParams.Params.GetAsId("efmanorgId");
            var efManorg = this.ManagingOrganizationEfficiencyRatingDomainService.Get(efManorgId);

            if (efManorg.IsNull())
            {
                return BaseDataResult.Error("Not found");
            }

            IDataResult result = null;
            this.Container.InTransactionWithMutexLock(efManorg.GetMutexName(),
                "Получение элементов рейтинга эффективности",
                () => result = new BaseDataResult(this.GetMetaValueInternal(efManorg)));


            return result;
        }

        /// <inheritdoc />
        public override IDataResult CalcNow(BaseParams baseParams)
        {
            var efManorgId = baseParams.Params.GetAsId("efmanorgId");
            var efManorg = this.ManagingOrganizationEfficiencyRatingDomainService.Get(efManorgId);

            if (efManorg.IsNull())
            {
                return BaseDataResult.Error("Not found");
            }

            IDataResult result = null;
            this.Container.InTransactionWithMutexTryLock(
                efManorg.GetMutexName(),
                "Расчёт элементов рейтинга эффетивности",
                "По данной управляющей организации в текущем периоде уже производится расчёт, попробуйте позднее",
                () =>
                {
                    var dataValues = this.GetItems(efManorg, this.GetMeta(efManorg.Period.Group));

                    result = this.CalcInternal(efManorg, dataValues);

                    if (result.Success)
                    {
                        // актуализируем динамику
                        this.ActualizeDynamics(efManorg, dataValues);
                    }
                });

            return result ?? new BaseDataResult(false, "При выполнении расчета произошла ошибка");
        }

        /// <inheritdoc />
        public override IDataResult CalcMass(BaseParams baseParams)
        {
            var selectedIds = baseParams.Params["objectIds"].As<List<object>>().Select(x => x.As<DynamicDictionary>().ReadClass<Record>()).ToList();
            var loadParams = baseParams.GetLoadParam();
            var municipalityIds = baseParams.Params.GetAs<long[]>("municipalityIds");
            var periodId = baseParams.Params.GetAs<long>("periodId");

            var period = this.EfficiencyRatingPeriodDomainService.Get(periodId);
            if (period.IsNull())
            {
                return BaseDataResult.Error("Не выбран период рейтинга эффективности");
            }

            IQueryable<ManagingOrganizationEfficiencyRating> manorgQuery;
            if (selectedIds.IsNotEmpty())
            {
                var ids = selectedIds.Where(x => x.Id.HasValue && x.Id.Value > 0).Select(x => x.Id.Value).ToArray();
                manorgQuery = this.ManagingOrganizationEfficiencyRatingDomainService.GetAll().WhereContains(x => x.Id, ids);
            }
            else
            {
                manorgQuery =
                    this.ManagingOrganizationEfficiencyRatingDomainService.GetAll()
                        .Where(x => x.Period.Id == periodId)
                        .WhereIf(municipalityIds.IsNotEmpty(), x => municipalityIds.Contains(x.ManagingOrganization.Contragent.Municipality.Id))
                        .Filter(loadParams, this.Container);
            }

            this.Container.InTransactionWithMutexTryLock("EfficiencyRatingMetaValueService.CalcMass",
                "Массовый расчёт рейтинга эффективности",
                "В данный момент уже производится массовый расчёт!",
                () => this.CalcMassInternal(manorgQuery, period));

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult GetFactors(BaseParams baseParams)
        {
            var data = this.DataMetaInfoDomainService.GetAll()
                .Where(x => x.Group.ConstructorType == this.ConstructorType && x.Level == 0)
                .Select(x => new { x.Name, x.Code })
                .DistinctBy(x => x.Code)
                .ToList();

            return new ListDataResult(data, data.Count);
        }

        /// <inheritdoc />
        protected override decimal CalcInternal<TElement>(DataValueTreeNode<TElement> root)
        {
            // если мы на уровне коэффициента и есть хотя бы 1 незаполненный атрибут, то коэффициент равен = 0
            if (!root.IsRoot && root.Current.MetaInfo.Level == 1 && root.Children.Any(x => x.Current.MetaInfo.Required && x.Value.IsNull()))
            {
                return 0;
            }

            return base.CalcInternal(root);
        }

        private static EfficiencyRatingDataValueTreeNode CreateTree(bool forUI, IDictionary<DataMetaInfo, ManagingOrganizationDataValue> itemsDict)
        {
            return new DataValueTreeGenerator<ManagingOrganizationDataValue, EfficiencyRatingDataValueTreeNode>().GetTree(
                itemsDict.Values,
                null,
                x => x.Name,
                forUI);
        }

        private void ActualizeDynamics(
            IQueryable<ManagingOrganizationEfficiencyRating> manorgQuery,
            EfficiencyRatingPeriod period,
            IDictionary<ManagingOrganizationEfficiencyRating, IDictionary<DataMetaInfo, ManagingOrganizationDataValue>> dataValues)
        {
            var prevAndCurrentPeriodIds =
                new[]
                {
                    this.periods.LastOrDefault(x => x.DateStart < period.DateStart),
                    this.periods.FirstOrDefault(x => x.DateStart > period.DateStart)
                }
                .Where(x => x != null)
                .Select(x => x.Id)
                .ToArray();

            var elementsToRecalcDynamics = dataValues.Values.SelectMany(x => x.Values).Where(x => x.MetaInfo.Level == 0).ToList();
            var codesToFind = elementsToRecalcDynamics.Select(x => x.Code).Distinct().ToList();

            // получаем все факторы с такими же кодами по этой уо за следующий и предыдущий период
            var existItems = this.ManorgDataValueDomainService.GetAll()
                .Where(x => prevAndCurrentPeriodIds.Contains(x.EfManagingOrganization.Period.Id))
                .Where(x => manorgQuery.Any(y => y.ManagingOrganization.Id == x.EfManagingOrganization.ManagingOrganization.Id))
                .Where(x => codesToFind.Contains(x.MetaInfo.Code))
                .ToList();

            // получаем показатель эффективности УО за следующий и предыдущий периоды
            var prevAndNextErManorg = this.ManagingOrganizationEfficiencyRatingDomainService.GetAll()
                .Where(x => manorgQuery.Any(y => y.ManagingOrganization.Id == x.ManagingOrganization.Id))
                .Where(x => prevAndCurrentPeriodIds.Contains(x.Period.Id))
                .ToList();

            foreach (var dataValue in dataValues)
            {
                this.ActualizeDynamics(dataValue.Key, dataValue.Value.Values.Where(x => x.MetaInfo.Level == 0), prevAndNextErManorg, existItems);
            }
        }

        private void ActualizeDynamics(ManagingOrganizationEfficiencyRating efManorg, IDictionary<DataMetaInfo, ManagingOrganizationDataValue> dataValues)
        {
            var prevAndCurrentPeriodIds =
                new[]
                {
                    this.periods.LastOrDefault(x => x.DateStart < efManorg.Period.DateStart),
                    this.periods.FirstOrDefault(x => x.DateStart > efManorg.Period.DateStart)
                }
                .Where(x => x != null)
                .Select(x => x.Id)
                .ToArray();

            var elementsToRecalcDynamics = dataValues.Values.Where(x => x.MetaInfo.Level == 0).ToList();
            var codesToFind = elementsToRecalcDynamics.Select(x => x.Code).Distinct().ToList();

            // получаем все факторы с такими же кодами по этой уо за следующий и предыдущий период
            var existItems = this.ManorgDataValueDomainService.GetAll()
                .Where(x => prevAndCurrentPeriodIds.Contains(x.EfManagingOrganization.Period.Id))
                .Where(x => x.EfManagingOrganization.ManagingOrganization.Id == efManorg.ManagingOrganization.Id)
                .Where(x => codesToFind.Contains(x.MetaInfo.Code))
                .ToList();

            // получаем показатель эффективности УО за следующий и предыдущий периоды
            var prevAndNextErManorg = this.ManagingOrganizationEfficiencyRatingDomainService.GetAll()
                .Where(x => x.ManagingOrganization.Id == efManorg.ManagingOrganization.Id)
                .Where(x => prevAndCurrentPeriodIds.Contains(x.Period.Id))
                .ToList();

            this.ActualizeDynamics(efManorg, elementsToRecalcDynamics, prevAndNextErManorg, existItems);
        }

        private void ActualizeDynamics(
            ManagingOrganizationEfficiencyRating efManorg,
            IEnumerable<ManagingOrganizationDataValue> items,
            IList<ManagingOrganizationEfficiencyRating> prevAndCurrentManorgs,
            IList<ManagingOrganizationDataValue> prevAndCurrentItems)
        {
            // актуализируем динамику для каждой записи по текущей УО за след и пред периоды
            this.CalcDynamics(prevAndCurrentManorgs, efManorg);

            foreach (var managingOrganizationDataValue in items)
            {
                // актуализируем динамику для каждого фактора за след и пред периоды
                this.CalcDynamics(prevAndCurrentItems, managingOrganizationDataValue);
            }

            // сохраняем изменения
            prevAndCurrentManorgs.ForEach(x => this.ManagingOrganizationEfficiencyRatingDomainService.Update(x));
            prevAndCurrentItems.ForEach(x => this.ManorgDataValueDomainService.Update(x));
        }

        private void CalcDynamics(IList<ManagingOrganizationDataValue> existElements, ManagingOrganizationDataValue element)
        {
            var prevPeriod = this.periods.LastOrDefault(x => x.DateStart < element.EfManagingOrganization.Period.DateStart);
            var nextPeriod = this.periods.FirstOrDefault(x => x.DateStart > element.EfManagingOrganization.Period.DateStart);

            if (prevPeriod != null)
            {
                var prevElement = existElements.FirstOrDefault(x => 
                        element.Code == x.Code 
                        && x.EfManagingOrganization.ManagingOrganization.Id == element.EfManagingOrganization.ManagingOrganization.Id 
                        && x.EfManagingOrganization.Period.Id == prevPeriod.Id);

                if (prevElement?.Value != null && prevElement.Value.ToDecimal() != 0)
                {
                    element.Dynamics = element.Value.ToDecimal() / prevElement.Value.ToDecimal() * 100;
                }
            }

            if (nextPeriod != null)
            {
                var nextElement = existElements.FirstOrDefault(x =>
                        element.Code == x.Code
                        && x.EfManagingOrganization.ManagingOrganization.Id == element.EfManagingOrganization.ManagingOrganization.Id
                        && x.EfManagingOrganization.Period.Id == nextPeriod.Id);

                if (nextElement?.Value != null && element.Value.ToDecimal() != 0)
                {
                    nextElement.Dynamics = nextElement.Value.ToDecimal() / element.Value.ToDecimal() * 100;
                }
            }
        }

        private void CalcDynamics(IList<ManagingOrganizationEfficiencyRating> existElements, ManagingOrganizationEfficiencyRating element)
        {
            var prevPeriod = this.periods.LastOrDefault(x => x.DateStart < element.Period.DateStart);
            var nextPeriod = this.periods.FirstOrDefault(x => x.DateStart > element.Period.DateStart);

            if (prevPeriod != null)
            {
                var prevElement = existElements.FirstOrDefault(x => x.ManagingOrganization.Id == element.ManagingOrganization.Id && x.Period.Id == prevPeriod.Id);

                if (prevElement?.Rating != null && prevElement.Rating != 0)
                {
                    element.Dynamics = element.Rating / prevElement.Rating * 100;
                }
            }

            if (nextPeriod != null)
            {
                var nextElement = existElements.FirstOrDefault(x => x.ManagingOrganization.Id == element.ManagingOrganization.Id && x.Period.Id == nextPeriod.Id);

                if (nextElement?.Rating != null && element.Rating != 0)
                {
                    nextElement.Dynamics = nextElement.Rating / element.Rating * 100;
                }
            }
        }

        private void CalcMassInternal(IQueryable<ManagingOrganizationEfficiencyRating> manorgQuery, EfficiencyRatingPeriod period)
        {
            // Получаем массово все данные и досоздаём отсутствующие
            var dictMo = this.GetItems(manorgQuery, this.GetMeta(period.Group));

            // готовим данные для простановки значениями из источников
            var dataValues = dictMo.Values.SelectMany(x => x.Values)
                .Where(x => x.MetaInfo.DataValueType == DataValueType.Dictionary && x.MetaInfo.DataFillerName.IsNotEmpty())
                .Cast<BaseDataValue>()
                .ToList();

            // параметры постановки данных из источников
            var baseParams = new BaseParams();
            baseParams.Params.SetValue("manorgQuery", manorgQuery.Select(x => x.ManagingOrganization));
            baseParams.Params.SetValue("period", period);

            // проставляем данные из источников, там нет зависимости от реализации, важен только объект с источником
            this.DataFillerService.SetValue(baseParams, dataValues);

            // расчёт по данным в памяти
            foreach (var manorgKvp in dictMo)
            {
                this.CalcInternal(manorgKvp.Key, manorgKvp.Value);
            }

            this.ActualizeDynamics(manorgQuery, period, dictMo);
        }

        private IDataResult CalcInternal(ManagingOrganizationEfficiencyRating manorg, IDictionary<DataMetaInfo, ManagingOrganizationDataValue> dataValues)
        {
            var dataTree = EfficiencyRatingDataValueService.CreateTree(false, dataValues);

            try
            {
                manorg.Rating = this.CalcInternal(dataTree);
            }
            catch (ValidationException exception)
            {
                return new BaseDataResult(false, exception.Message);
            }
            

            dataValues.ForEach(x => this.ManorgDataValueDomainService.SaveOrUpdate(x.Value));
            this.ManagingOrganizationEfficiencyRatingDomainService.Update(manorg);

            return new BaseDataResult();
        }

        private EfficiencyRatingDataValueTreeNode GetMetaValueInternal(ManagingOrganizationEfficiencyRating manorg)
        {
            // 1. формируем список элементов, если их нет, то они создадутся
            var itemsDict = this.GetItems(manorg, this.GetMeta(manorg.Period.Group));

            // 2. Заполняем данными из источников перед отображением
            var manorgQuery = this.ManagingOrganizationDomainService.GetAll().Where(x => x.Id == manorg.ManagingOrganization.Id);
            var baseParams = new BaseParams();
            baseParams.Params.SetValue("manorgQuery", manorgQuery);
            baseParams.Params.SetValue("period", manorg.Period);
            this.DataFillerService.SetValue(baseParams, itemsDict.Values.Cast<BaseDataValue>().ToList());

            // 3. сохраняем всё это дело, сортируем по уровню, чтобы сначала сохранились элементы верхнего уровня
            itemsDict.Values.OrderBy(x => x.MetaInfo.Level).ForEach(x => this.ManorgDataValueDomainService.SaveOrUpdate(x));

            // 4. строим дерево
            var dataTree = EfficiencyRatingDataValueService.CreateTree(true, itemsDict);

            // 5. отдаём результат
            return dataTree;
        }

        private IDictionary<DataMetaInfo, ManagingOrganizationDataValue> GetItems(
            ManagingOrganizationEfficiencyRating manorg, 
            IEnumerable<DataMetaInfo> metaInfos,
            IDictionary<DataMetaInfo, ManagingOrganizationDataValue> exists = null)
        {
            var result = exists ?? this.ManorgDataValueDomainService.GetAll()
                .Where(x => x.EfManagingOrganization.Id == manorg.Id)
                .ToDictionary(x => x.MetaInfo);

            foreach (var dataMetaInfo in metaInfos)
            {
                this.GetItem(result, manorg, dataMetaInfo);
            }

            return result;
        }

        private IDictionary<ManagingOrganizationEfficiencyRating, IDictionary<DataMetaInfo, ManagingOrganizationDataValue>> GetItems(
            IQueryable<ManagingOrganizationEfficiencyRating> manorgQuery,
            IEnumerable<DataMetaInfo> metaInfos)
        {
            var result = this.ManorgDataValueDomainService.GetAll()
                .Where(x => manorgQuery.Any(y => y.Id == x.EfManagingOrganization.Id))
                .AsEnumerable()
                .GroupBy(x => x.EfManagingOrganization)
                .ToDictionary(x => x.Key, y => y.ToDictionary(x => x.MetaInfo));

            return result.ToDictionary(manorg => manorg.Key, manorg => this.GetItems(manorg.Key, metaInfos, manorg.Value));
        }

        private ManagingOrganizationDataValue GetItem(
            IDictionary<DataMetaInfo, ManagingOrganizationDataValue> items,
            ManagingOrganizationEfficiencyRating manorg, 
            DataMetaInfo metaInfo)
        {
            ManagingOrganizationDataValue result;

            if (items.TryGetValue(metaInfo, out result))
            {
                return result;
            }

            return items[metaInfo] = new ManagingOrganizationDataValue
            {
                EfManagingOrganization = manorg,
                MetaInfo = metaInfo,
                Parent = metaInfo.Parent != null ? this.GetItem(items, manorg, metaInfo.Parent) : null
            };
        }

        /// <summary>
        /// Элемент дерева для Рейтинга эффективности
        /// </summary>
        private class EfficiencyRatingDataValueTreeNode : DataValueTree<ManagingOrganizationDataValue>
        {
            /// <summary>
            /// Динамика
            /// </summary>
            public decimal Dynamics => this.Current.IsNotNull() && this.Current.MetaInfo.Level == 0 ? this.Current.Dynamics : 0;

            public long EfManagingOrganization => this.Current?.EfManagingOrganization.Id ?? 0;

            /// <summary>
            /// .ctor
            /// </summary>
            /// <param name="value">Значение узла дерева</param>
            public EfficiencyRatingDataValueTreeNode(ManagingOrganizationDataValue value)
                : base(value)
            {
            }

            public EfficiencyRatingDataValueTreeNode() : base(null)
            {
                
            }

            /// <inheritdoc />
            protected override DataValueTreeNode<ManagingOrganizationDataValue> CreateNodeInternal(ManagingOrganizationDataValue dataValue)
            {
                return new EfficiencyRatingDataValueTreeNode(dataValue)
                {
                    CreateFullTree = this.CreateFullTree,
                    Parent = this
                };
            }
        }

        private class Record
        {
            public long? Id { get; set; }
            public long ManorgId { get; set; }
        }
    }
}