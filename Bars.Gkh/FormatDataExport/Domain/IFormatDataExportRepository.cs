namespace Bars.Gkh.FormatDataExport.Domain
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.FormatDataExport.ExportableEntities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Репозиторий для экспорта <see cref="BaseEntity"/> в <see cref="IExportableEntity"/> использующий фильтрацию <see cref="IFormatDataExportFilterService"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IFormatDataExportRepository<out T>
        where T : BaseEntity
    {
        /// <summary>
        /// Получить дерево выражений запрашиваемой сущности <see cref="T"/> : <see cref="BaseEntity"/>
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetQuery(IFormatDataExportFilterService filterService);

        /// <summary>
        /// Получить данные для фильтрации
        /// </summary>
        /// <param name="baseParams">Параметры фильтрации</param>
        IDataResult List(BaseParams baseParams);
    }

    /// <summary>
    /// Базовая реализация
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseFormatDataExportRepository<T> : IFormatDataExportRepository<T>
        where T : BaseEntity
    {
        /// <summary>
        /// IoC
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Репозиторий сущности
        /// </summary>
        public IRepository<T> Repository { get; set; }

        /// <inheritdoc />
        public abstract IQueryable<T> GetQuery(IFormatDataExportFilterService filterService);

        /// <summary>
        /// Фильтрация по дате изменений в зависимости от настроек
        /// </summary>
        protected IQueryable<T> FilterByEditDate(IQueryable<T> query, IFormatDataExportFilterService filterService)
        {
            return query
                .WhereIf(filterService.UseIncremental, x => x.ObjectEditDate > filterService.StartEditDate)
                .WhereIf(filterService.UseIncremental, x => x.ObjectEditDate < filterService.EndEditDate);
        }

        /// <inheritdoc />
        public virtual IDataResult List(BaseParams baseParams)
        {
            return this.Repository.GetAll()
                .ToListDataResult(baseParams.GetLoadParam());
        }
        /// <summary>
        /// Фильтрация по контрагенту
        /// </summary>
        protected IQueryable<T> FilterByContragent(IFormatDataExportFilterService filterService, Expression<Func<T, Contragent>> contragentSelector)
        {
            var repository = this.Container.ResolveRepository<T>();
            using (this.Container.Using(repository))
            {
                return filterService
                    .FilterByContragent(repository.GetAll(), contragentSelector);
            }
        }

        /// <summary>
        /// Фильтрация по контрагенту
        /// </summary>
        protected IQueryable<T> FilterByContragent<TRepos>(IFormatDataExportFilterService filterService, Expression<Func<TRepos, Contragent>> contragentSelector, Expression<Func<TRepos, T>> selector)
            where TRepos : BaseEntity
        {
            var repository = this.Container.ResolveRepository<TRepos>();
            using (this.Container.Using(repository))
            {
                return filterService
                    .FilterByContragent(repository.GetAll(), contragentSelector)
                    .Select(selector);
            }
        }
    }
}