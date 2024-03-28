namespace Bars.Gkh.SystemDataTransfer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.SystemDataTransfer.Meta;
    using Bars.Gkh.SystemDataTransfer.Meta.ProviderMeta;

    using Castle.Windsor;

    using Newtonsoft.Json;

    /// <summary>
    /// Провайдер переноса данных
    /// </summary>
    public partial class DataTransferProvider : IDataTransferProvider
    {
        private TransferEntityContainer exportContainer;

        public IWindsorContainer Container { get; set; }
        public ISessionProvider SessionProvider { get; set; }

        /// <inheritdoc />
        public IList<Type[]> GetLayers(IEnumerable<string> typeNames = null, bool exportDependencies = true)
        {
            var types = this.GetExportableTypes();
            var entities = this.GetExportableTypes()
                .Where(x => !x.Value.IsBase)
                .WhereIf(typeNames.IsNotEmpty(), x => typeNames.Contains(x.Key.FullName))
                .Select(x => x.Key)
                .ToList();

            var list = new List<Entity>();

            this.BuildEntityHierarchy(list, entities, types, null, false, exportDependencies);

            var layerResult = this.SortTypes(exportDependencies, list);

            var maxLevel = layerResult.Values.Max();
            layerResult = layerResult.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => maxLevel - x.Value);

            var typeDict = list.ToDictionary(x => x.Type.FullName);
            IList<Type[]> returnResult = new List<Type[]>();
            IList<Type> currentLevel = new List<Type>();
            var count = 0;

            // склеиваем, если у нас получилось мало сущностей в каждом слое
            foreach (var kvp in layerResult)
            {
                var type = typeDict[kvp.Key].Type;

                // грузим за раз не больше 1 000 000 записей
                if (count >= 1000000)
                {
                    returnResult.Add(currentLevel.ToArray());
                    currentLevel.Clear();
                    count = 0;
                }

                currentLevel.Add(type);
                count += this.GetEnititiesCount(type);
            }

            returnResult.Add(currentLevel.ToArray());

            return returnResult;
        }

        private IDictionary<string, int> SortTypes(bool exportDependencies, List<Entity> list)
        {
            var edges = list.ToDictionary(x => x.Type.FullName,
                x => x.Dependencies.Where(y => !y.IsParent).Select(y => y.Type.FullName));

            IEnumerable<string> dependencies;
            IList<string> addedTypes = new List<string>();

            if (!exportDependencies)
            {
                // когда грузим без зависимостей, то надо дополнить зависимыми
                dependencies = edges.SelectMany(x => x.Value);
            }
            else
            {
                // если грузим всё, то нужно добавить базовые типы
                dependencies = edges.SelectMany(x => x.Value).Where(x => this.GetExportableTypes().Values.Any(y => y.Type.FullName == x && y.IsBase));
            }

            foreach (var type in dependencies.Where(x => !edges.Keys.Contains(x)).ToArray())
            {
                addedTypes.Add(type);
                edges[type] = new List<string>();
            }

            var layerResult = DataTransferProvider.TopologicalSort(edges);

            addedTypes.ForEach(x => layerResult.Remove(x));
            return layerResult;
        }

        /// <summary>
        /// Получение типов экспортируемых сущностей
        /// </summary>
        /// <returns></returns>
        private IDictionary<Type, ITransferEntityMeta> GetExportableTypes()
        {
            if (this.exportContainer.IsNull())
            {
                this.exportContainer = new TransferEntityContainer();
                this.Container.ResolveAll<ITransferEntityProvider>().ForEach(x => x.FillContainer(this.exportContainer));
            }

            return this.exportContainer.Container;
        }

        private void ValidateEntities(IEnumerable<Entity> entities, IDictionary<Type, ITransferEntityMeta> exportableTypes)
        {
            var types = entities
                .SelectMany(x => x.Dependencies.Select(z => z.Type).Union(new[] { x.Type }))
                .Distinct()
                .Where(x => !exportableTypes.ContainsKey(x))
                .ToList();

            if (types.Any())
            {
                throw new ValidationException($"Для типов: {string.Join(", ", types)} отсутствует регистрация мета-описания экспорта/импорта");
            }
        }

        private PropertyInfo[] GetProperties(Type type, ITransferEntityMeta meta)
        {
            return meta.GetExportableProperties(type);
        }
    }
}