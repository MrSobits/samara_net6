namespace Bars.B4.Modules.Analytics.Data
{
    using System;
    using Bars.B4.Modules.Analytics.Enums;
    using Newtonsoft.Json;

    /// <summary>
    /// Параметр поставщика данных.
    /// </summary>
    public class DataProviderParam : IParam
    {
        public OwnerType OwnerType { get { return OwnerType.System; } }
        public ParamType ParamType { get; set; }
        public bool Required { get; set; }
        public bool Multiselect { get; set; }
        public string Label { get; set; }
        public string Name { get; set; }
        public string Additional { get; set; }
        public string SqlQuery { get; set; }
        public object DefaultValue { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public Type CLRType
        {
            get { return typeof(string); }
        }
    }
}