namespace Bars.B4.Modules.Analytics.Data
{
    using System;
    using System.Collections.Generic;
    using Bars.B4.Modules.Analytics.Filters;

    /// <summary>
    /// Абстрактная реализация поставщика, возвращающего данные в виде единичного объекта.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class BaseSingleDataProvider<T> : ISingleDataProvider<T> where T : class, new()
    {
        private readonly List<DataProviderParam> _paramsList;

        protected BaseSingleDataProvider()
        {
            _paramsList = new List<DataProviderParam>();
        }

        public abstract T GetSingleData(SystemFilter systemFilter, DataFilter dataFilter, BaseParams baseParams);

        #region base implementations
        public abstract string Name { get; }

        public virtual string Key { get { return GetType().Name; } }

        public virtual string Description { get { return Name; } }

        public virtual object ProvideData(SystemFilter systemFilter, DataFilter dataFilter, BaseParams baseParams)
        {
            return GetSingleData(systemFilter, dataFilter, baseParams);
        }

        public Type ProvideMetaData()
        {
            return typeof(T);
        }

        public IEnumerable<DataProviderParam> Params { get { return _paramsList; } }
        public virtual bool IsHidden { get { return false; } }
        public object GetSampleData()
        {
            return new T();
        }

        public void AddParam(DataProviderParam dpParam)
        {
            _paramsList.Add(dpParam);
        }

        #endregion
    }
}
