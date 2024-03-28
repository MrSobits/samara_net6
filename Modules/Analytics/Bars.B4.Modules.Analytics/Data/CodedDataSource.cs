namespace Bars.B4.Modules.Analytics.Data
{
    using System;
    using System.Collections.Generic;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Filters;
    using Bars.B4.Utils.Annotations;

    /// <summary>
    /// Источник данных, реализованный в коде.
    /// </summary>
    public class CodedDataSource : IDataSource
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public DataFilter DataFilter { get; set; }
        public SystemFilter SystemFilter { get; set; }

        public CodedDataSource(string name, IDataProvider dataProvider)
        {
            ArgumentChecker.NotNull(dataProvider, "dataProvider");
            ArgumentChecker.NotNullOrEmpty(name, "name");
            this.Name = name;
            this.Provider = dataProvider;
        }

        /// <summary>
        /// Поставщик данных, который используется для фактического получения данных.
        /// </summary>
        public IDataProvider Provider { get; private set; }

        public object GetData(BaseParams baseParams)
        {
            return this.Provider.ProvideData(this.SystemFilter, this.DataFilter, baseParams);
        }

        public Type GetMetaData()
        {
            return this.Provider.ProvideMetaData();
        }

        public IEnumerable<IParam> Params { get { return new List<DataProviderParam>(); } }
        public object GetSampleData()
        {
            return this.Provider.GetSampleData();
        }
    }
}
