namespace Bars.B4.Modules.Analytics.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Domain;
    using Bars.B4.Modules.Analytics.Enums;
    using Bars.B4.Modules.Analytics.Exceptions;
    using Bars.B4.Modules.Analytics.Filters;
    using Newtonsoft.Json;

    /// <summary>
    /// Источник данных.
    /// </summary>
    public class DataSource : BaseEntity, IDataSource
    {
        private DataSource _parent;

        /// <summary>
        /// Пустой конструктор для того чтобы мапинг работал
        /// </summary>
        public DataSource()
        {
        }

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="provider">Провайдер данных</param>
        public DataSource(IDataProvider provider)
        {
            this.ProviderKey = provider.Key;
            this.Name = provider.Name;
            this.Description = provider.Description;
            this.OwnerType = OwnerType.System;
        }

        /// <summary>
        /// Название.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Тип источника данных.
        /// </summary>
        public virtual OwnerType OwnerType { get; set; }

        /// <summary>
        /// Родитель
        /// </summary>
        public virtual DataSource Parent
        {
            get { return _parent; }
            set
            {
                if (value != null && value.Id == this.Id)
                {
                    throw new ArgumentException("Объект не может быть родителем самого себя.");
                }
                _parent = value;
            }
        }

        /// <summary>
        /// Описание.
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Ключ родительского поставщика данных <see cref="IDataProvider.Key"/>
        /// </summary>
        public virtual string ProviderKey { get; set; }

        /// <summary>
        /// Фильтр (нехранимое поле).
        /// </summary>
        public virtual DataFilter DataFilter { get; set; }

        /// <summary>
        /// Сериализованный <see cref="DataFilter"/> для хранения в БД.
        /// </summary>
        public virtual byte[] DataFiletrBytes
        {
            get { return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(DataFilter)); }
            set { DataFilter = JsonConvert.DeserializeObject<DataFilter>(Encoding.UTF8.GetString(value)); }
        }

        /// <summary>
        /// Системный фильтр (Нехранимое поле).
        /// </summary>
        public virtual SystemFilter SystemFilter { get; set; }

        /// <summary>
        /// Сериализованный <see cref="SystemFilter"/> для хранения в БД.
        /// </summary>
        public virtual byte[] SystemFilterBytes
        {
            get { return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(SystemFilter)); }
            set { SystemFilter = JsonConvert.DeserializeObject<SystemFilter>(Encoding.UTF8.GetString(value)); }
        }


        private object GetDataRecursive(BaseParams baseParams, SystemFilter systemFilter, DataFilter dataFilter)
        {
            if (Parent != null)
            {
                var combinedSysFilter = systemFilter;
                if (Parent.SystemFilter != null)
                {
                    combinedSysFilter = new SystemFilter
                    {
                        Group = SystemFilterGroup.and,
                        Filters =
                            systemFilter != null
                                ? new[] { Parent.SystemFilter, systemFilter }
                                : new[] { Parent.SystemFilter }
                    };
                }

                var combindeDataFilter = dataFilter;
                if (Parent.DataFilter != null)
                {
                    combindeDataFilter = new DataFilter
                    {
                        Group = DataFilterGroup.and,
                        Filters = dataFilter != null
                            ? new[] { Parent.DataFilter, dataFilter }
                            : new[] { Parent.DataFilter }
                    };
                }
                return Parent.GetDataRecursive(baseParams, combinedSysFilter, combindeDataFilter);
            }

            var container = ApplicationContext.Current.Container;
            var provider = container.Resolve<IDataProviderService>().Get(ProviderKey);
            var data = provider != null ? provider.ProvideData(systemFilter, dataFilter, baseParams) : null;
            container.Release(provider);
            return data;
        }

        public virtual object GetData(BaseParams baseParams)
        {
            return GetDataRecursive(baseParams, SystemFilter, DataFilter);
        }

        public virtual Type GetMetaData()
        {
            var provider = GetParentDataProvider();
            var data = provider != null ? provider.ProvideMetaData() : null;
            return data;
        }

        public virtual IEnumerable<IParam> Params
        {
            get
            {
                var provider = this.GetParentDataProvider();
                var providerParams = provider != null ? provider.Params : new List<DataProviderParam>();
                return providerParams;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual object GetSampleData()
        {
            return GetParentDataProvider().GetSampleData();
        }

        /// <summary>
        /// Получение "родительского" поставщика, используемого
        /// для непосредственного получения данных.
        /// </summary>
        /// <returns></returns>
        public virtual IDataProvider GetParentDataProvider()
        {
            var source = this;
            while (source != null && string.IsNullOrEmpty(source.ProviderKey))
            {
                source = source.Parent;
            }
            if (source == null)
            {
                throw new DataProviderNotSpecifiedException();
            }
            var container = ApplicationContext.Current.Container;
            var provider = container.Resolve<IDataProviderService>().Get(source.ProviderKey);
            return provider;
        }
    }
}
