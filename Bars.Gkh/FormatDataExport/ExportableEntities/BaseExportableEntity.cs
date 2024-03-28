namespace Bars.Gkh.FormatDataExport.ExportableEntities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.FormatDataExport.Enums;
    using Bars.Gkh.FormatDataExport.ExportableEntities.ExportableFile;
    using Bars.Gkh.FormatDataExport.ExportableEntities.Model;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Базовый класс не хранимой экспортируемой сущности
    /// </summary>
    /// <remarks>
    /// Информация собирается из нескольких сущностей
    /// </remarks>
    public abstract partial class BaseExportableEntity : IExportableEntity
    {
        /// <summary>
        /// Версия формата
        /// </summary>
        public static string Version = "4.0.6.2";

        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Логгер
        /// </summary>
        public ILogger LogManager { get; set; }

        /// <summary>
        /// Контрагент предоставляющий данные
        /// </summary>
        protected Contragent Contragent { get; set; }

        /// <summary>
        /// Поставщик информации
        /// </summary>
        protected FormatDataExportProviderType Provider => this.FilterService.Provider;

        /// <summary>
        /// Коллекция экспортируемых файлов
        /// </summary>
        private IFileEntityCollection FileEntityCollection { get; set; }

        private IFormatDataExportIncrementalService FormatDataExportIncrementalService { get; set; }

        /// <inheritdoc />
        public abstract string Code { get; }

        /// <inheritdoc />
        public virtual string FormatVersion => BaseExportableEntity.Version;

        /// <summary>
        /// Параметры для выборки
        /// </summary>
        protected DynamicDictionary SelectParams { get; private set; }

        /// <summary>
        /// Фабрика селектор-сервисов
        /// </summary>
        protected IProxySelectorFactory ProxySelectorFactory { get; private set; }

        /// <summary>
        /// Сервис фильтрации
        /// </summary>
        protected IFormatDataExportFilterService FilterService { get; private set; }

        /// <summary>
        /// Получить данные сущности
        /// </summary>
        protected abstract IList<ExportableRow> GetEntityData();

        /// <summary>
        /// Получить частичные сущности
        /// </summary>
        protected virtual IPartialExportableEntity[] GetPartialEntities()
        {
            return new IPartialExportableEntity[0];
        }

        /// <inheritdoc />
        public abstract IList<string> GetHeader();

        /// <inheritdoc />
        public virtual IDataResult<IList<List<string>>> GetData(DynamicDictionary baseParams)
        {
            this.SelectParams = baseParams ?? new DynamicDictionary();
            this.ProxySelectorFactory = this.SelectParams.GetAs<IProxySelectorFactory>("ProxySelectorFactory");
            this.FilterService = this.SelectParams.GetAs<IFormatDataExportFilterService>("FormatDataExportFilterService");
            this.FormatDataExportIncrementalService = this.SelectParams.GetAs<IFormatDataExportIncrementalService>("FormatDataExportIncrementalService");
            this.Contragent = this.SelectParams.GetAs<Contragent>("Contragent");
            this.FileEntityCollection = this.SelectParams.GetAs<IFileEntityCollection>("FileEntityCollection");

            var entityData = this.CheckUniquePrimaryId(this.GetEntityData());
            var originalCount = entityData.Count;
            if (this.FormatDataExportIncrementalService != null)
            {
                entityData = this.FormatDataExportIncrementalService.GetIncrementalData(this.Code, entityData);
            }

            this.EmptyMandatoryFields = this.GetEmptyMandatoryFields(entityData);

            if (this.SelectParams.GetAs("NoEmptyMandatoryFields", false) && this.EmptyMandatoryFields.IsNotEmpty())
            {
                throw new FormatException($"{this.Code}|Не все обязательные поля заполнены");
            }

            var endityValues = entityData
                .Select(x => x.Cells.Values.ToList())
                .ToList();
            return new GenericDataResult<IList<List<string>>>(endityValues) { Success = originalCount > 0 };
        }

        /// <inheritdoc />
        public IDictionary<long, IEnumerable<int>> EmptyMandatoryFields { get; private set; }

        /// <summary>
        /// Список обязательных полей
        /// </summary>
        protected virtual IList<int> MandatoryFields { get; } = null;

        /// <summary>
        /// Предикат выбора незаполненных обязательных полей
        /// </summary>
        protected virtual Func<KeyValuePair<int, string>, ExportableRow, bool> EmptyFieldPredicate { get; } = null;

        private bool CheckField(KeyValuePair<int, string> pair, ExportableRow row)
        {
            return this.MandatoryFields.Contains(pair.Key) && pair.Value.IsEmpty();
        }

        /// <summary>
        /// Получить незаполненные обязательные поля по определенному правилу
        /// </summary>
        /// <param name="rows">Строки с данными</param>
        private IDictionary<long, IEnumerable<int>> GetEmptyMandatoryFields(IList<ExportableRow> rows)
        {
            var result = new Dictionary<long, IEnumerable<int>>();

            if (this.EmptyFieldPredicate == null && this.MandatoryFields == null)
            {
                return result;
            }

            var predicate = this.EmptyFieldPredicate ?? this.CheckField;

            foreach (var row in rows)
            {
                var emptyCells = row.Cells
                    .Where(x => predicate(x, row))
                    .Select(x => x.Key + 1);

                if (emptyCells.IsNotEmpty())
                {
                    result.Add(row.Id, emptyCells);
                }
            }

            return result;
        }

        /// <summary>
        /// Получить объединенную сущность
        /// </summary>
        /// <param name="mainData">Данные из базовой сущности</param>
        /// <param name="baseParams">Параметры</param>
        protected IEnumerable<ExportableRow> MergeData(IEnumerable<ExportableRow> mainData, DynamicDictionary baseParams)
        {
            var partialExportableEntities = this.GetPartialEntities();
            if (partialExportableEntities.Length == 0)
            {
                return mainData;
            }

            var mainDataDict = mainData.ToDictionary(x => x.Id);

            foreach (var entity in partialExportableEntities)
            {
                var partialData = entity.GetPartialEntityData(baseParams);
                foreach (var partRow in partialData)
                {
                    if (mainDataDict.ContainsKey(partRow.Id))
                    {
                        mainDataDict[partRow.Id].Merge(partRow);
                    }
                }
            }

            return mainDataDict.Values;
        }

        /// <summary>
        /// Добавить файл в коллекцию экспорта и отфильтровать данные
        /// <para>Коллекция фильтруется, если передан параметр "OnlyExistsFiles"</para>
        /// </summary>
        /// <param name="entityData">Коллекция экспортируемых данных</param>
        /// <param name="fileSelector">Селектор файлов</param>
        protected IEnumerable<T> AddFilesToExport<T>(IEnumerable<T> entityData, Func<T, FileInfo> fileSelector)
        {
            var existsFiles = this.FileEntityCollection.AddFileRange(entityData
                    .Select(fileSelector)
                    .Where(x => x != null)
                    .Select(x => ExportableFileInfo.CreateFromBase(x, this.Code)))
                .Select(x => x.Id)
                .ToList();

            if (this.SelectParams.GetAs("OnlyExistsFiles", false))
            {
                return entityData.Where(x => existsFiles.Contains(fileSelector.Invoke(x)?.Id ?? 0));
            }

            return entityData;
        }

        /// <summary>
        /// Получить коллекцию идентификаторов всех полей
        /// </summary>
        protected IList<int> GetAllFieldIds()
        {
            return this.GetHeader().Select((x, i) => i).ToList();
        }

        private IList<ExportableRow> CheckUniquePrimaryId(IList<ExportableRow> data)
        {
            var groupingData = data.GroupBy(x => x.Id);

            if (groupingData.Any(x => x.Count() > 1))
            {
                var dublicateIds = groupingData.Where(x => x.Count() > 1).Select(x => x.Key.ToString()).AggregateWithSeparator(", ");
                throw new FormatException($"Нарушена уникальность первичного ключа сущности '{this.Code}'." +
                    $"Дублированные идентификаторы: {dublicateIds}");
            }

            return data;
        }

        /// <inheritdoc />
        public virtual IList<string> GetInheritedEntityCodeList()
        {
            return new List<string>();
        }

        /// <inheritdoc />
        public virtual FormatDataExportProviderFlags AllowProviderFlags => FormatDataExportProviderFlags.All;

        private string GetEntityCode(Type entiType)
        {
            var name = entiType.Name;
            if (!name.EndsWith("ExportableEntity"))
            {
                throw new Exception($"Не корректное наименование экспортируемой секции: '{name}'");
            }

            return name.Replace("ExportableEntity", "").ToUpper();
        }

        protected IList<string> GetEntityCodeList(params Type[] entityTypes)
        {
            if (entityTypes == null)
            {
                return new List<string>();
            }

            return entityTypes.Select(this.GetEntityCode).ToList();
        }

        protected string EntityCode<TEntity>()
            where TEntity : IExportableEntity
        {
            return this.GetEntityCode(typeof(TEntity));
        }
    }

    /// <summary>
    /// Базовый класс хранимой экспортируемой сущности
    /// </summary>
    /// <typeparam name="T">Экспортируемая сущность</typeparam>
    [Obsolete("Использовать в связке с SelectorService")]
    public abstract class BaseExportableEntity<T> : BaseExportableEntity
        where T : IEntity
    {
        /// <summary>
        /// Репозиторий сущности
        /// </summary>
        public IRepository<T> EntityRepository { get; set; }

        /// <summary>
        /// Отфильтрованный по контрагентам запрос получения сущностей типа <typeparamref name="T"/>
        /// </summary>
        /// <param name="contragentSelector">Селектор контрагента</param>
        protected IQueryable<T> GetFiltred(Expression<Func<T, Contragent>> contragentSelector)
        {
            return this.FilterService.FilterByContragent(this.EntityRepository.GetAll(), contragentSelector);
        }
    }

    internal class ExportableEntityComparer : IEqualityComparer<IExportableEntity>, IComparer<IExportableEntity>
    {
        /// <inheritdoc />
        public bool Equals(IExportableEntity x, IExportableEntity y)
        {
            if (x == null || y == null)
            {
                return true;
            }

            return string.Equals(x.Code, y.Code, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc />
        public int GetHashCode(IExportableEntity obj)
        {
            return obj.Code.GetHashCode();
        }

        /// <inheritdoc />
        public int Compare(IExportableEntity x, IExportableEntity y)
        {
            return string.Compare(x.Code, y.Code, StringComparison.OrdinalIgnoreCase);
        }
    }
}