namespace Bars.Gkh.MetaValueConstructor.DataFillers
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Поставщик работы с регистрациями источников данных
    /// </summary>
    public class DataFillerProvider : IDataFillerProvider
    {
        private IReadOnlyList<DataFillerInfo> infos;
        private readonly IWindsorContainer container;

        /// <summary>
        /// Домен-сервис <see cref="MetaConstructorGroup"/>
        /// </summary>
        public IDomainService<MetaConstructorGroup> MetaConstructorGroupDomain { get; set; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="container">Контейнер</param>
        public DataFillerProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        /// <summary>
        /// Инициализировать
        /// </summary>
        public void Init()
        {
            var listData = new List<DataFillerInfo>();

            this.container.UsingForResolvedAll<IConstructorDataFillerMap>((cnt, fillersMap) =>
            {
                foreach (var constructorDataFillerMap in fillersMap)
                {
                    constructorDataFillerMap.Map();
                    foreach (var dataFillerInfo in constructorDataFillerMap.GetInfo())
                    {
                        if (!dataFillerInfo.IsNamespace)
                        {
                            // если это реализация, то проверяем её валидность
                            if (dataFillerInfo.EntityType == null)
                            {
                                throw new ValidationException("Property EntityType of DataFillerInfo is null");
                            }

                            this.container.UsingForResolved<IConstructorDataFiller>(
                                dataFillerInfo.Code,
                                (cnt1, dataFiller) =>
                                {
                                    if (dataFiller.Type != dataFillerInfo.ConstructorType)
                                    {
                                        throw new ValidationException("Источник данных зарегистрирован для одного конструктора, хотя его реализация работает с другим типом");
                                    }
                                });
                        }

                        listData.Add(dataFillerInfo);
                    }
                }
            });

            this.infos = new ReadOnlyCollection<DataFillerInfo>(listData);
        }

        /// <summary>
        /// Вернуть описание всех источников
        /// </summary>
        /// <returns>Перечисление</returns>
        public IEnumerable<DataFillerInfo> GetDataFillerInfo() => this.infos;

        /// <summary>
        /// Вернуть дерево источников данных
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Дерево</returns>
        public DataTree<DataFillerInfo> GetTree(BaseParams baseParams)
        {
            var groupId = baseParams.Params.GetAsId("groupId");
            var group = this.MetaConstructorGroupDomain.Get(groupId);

            if (group != null)
            {
                // достаем имплементации по типу
                var dataFillers = this.infos.Where(x => !x.IsNamespace && x.ConstructorType == group.ConstructorType).ToHashSet();

                //теперь надо добавить всех родителей
                foreach (var dataFillerInfo in dataFillers.ToList())
                {
                    this.AddParents(dataFillers, dataFillerInfo);
                }

                // а теперь построим из этого дерево
                return new DataGenerator<DataFillerInfo>().GetTree(dataFillers);
            }

            throw new ValidationException("Не передан обязательный параметр: Тип конструктора");
        }

        private void AddParents(HashSet<DataFillerInfo> items, DataFillerInfo children)
        {
            if (children.Parent != null)
            {
                items.Add(children.Parent);
                this.AddParents(items, children.Parent);
            }
        }
    }
}