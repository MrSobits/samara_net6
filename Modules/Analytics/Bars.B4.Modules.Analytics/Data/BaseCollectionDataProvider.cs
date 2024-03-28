namespace Bars.B4.Modules.Analytics.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Extensions;
    using Bars.B4.Modules.Analytics.Filters;
    using Castle.Windsor;

    /// <summary>
    /// Абстрактная реализация поставщика данных, возвращающего коллекцию объектов.
    /// </summary>
    public abstract class BaseCollectionDataProvider<T> : ICollectionDataProvider<T> where T : class, new()
    {
        private readonly IWindsorContainer _container;
        private readonly List<DataProviderParam> _paramsList; 

        /// <summary>
        /// Создает новый экземпляр.
        /// </summary>
        /// <param name="container">IoC-контейнер.</param>
        protected BaseCollectionDataProvider(IWindsorContainer container)
        {
            _container = container;
            _paramsList = new List<DataProviderParam>();
        }

        /// <summary>
        /// IoC-контейнер.
        /// </summary>
        protected virtual IWindsorContainer Container { get { return _container; } }

        /// <summary>
        /// Метод получения данных. Реализуется в наследниках, 
        /// которые должны возвращать <see cref="IQueryable{T}"/>.
        /// К этим данным будут применены фильтры.
        /// </summary>
        /// <returns></returns>
        protected abstract IQueryable<T> GetDataInternal(BaseParams baseParams);

        public IQueryable<T> GetCollectionData(SystemFilter systemFilter, DataFilter dataFilter, BaseParams baseParams)
        {
            var query = GetDataInternal(baseParams);

            var x = Expression.Parameter(typeof(T), "x");

            var filterExpr = systemFilter.BuildFilterExpression(typeof(T), Container);
            if (filterExpr != null)
            {
                query = query.Where(Expression.Lambda<Func<T, bool>>(filterExpr, x));
            }

            var expr = dataFilter.ReadDataFilterExpression(typeof(T), x, Container);

            if (expr != null)
            {
                query = query.Where(Expression.Lambda<Func<T, bool>>(expr, x));
            }
            return query;
        }

        #region IDataProvider members
        public abstract string Name { get; }
        public abstract string Description { get; }
        public virtual Type ProvideMetaData()
        {
            return typeof(T);
        }

        public virtual IEnumerable<DataProviderParam> Params
        {
            get
            {
                return _paramsList;
            }
        }

        public virtual bool IsHidden { get { return false; } }
        public object GetSampleData()
        {
            return new[] { new T() };
        }

        public void AddParam(DataProviderParam dpParam)
        {
            _paramsList.Add(dpParam);
        }

        public virtual string Key
        {
            get { return GetType().Name; }
        }
        public object ProvideData(SystemFilter systemFilter, DataFilter dataFilter, BaseParams baseParams)
        {
            return GetCollectionData(systemFilter, dataFilter, baseParams).ToList();
        }
        #endregion
    }
}
