namespace Bars.B4.Modules.Analytics.Reports.Entities
{
    using System;
    using System.Text;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Enums;
    using Bars.B4.Modules.Analytics.Extensions;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public class ReportParamGkh : BaseEntity, IParam
    {
        public ReportParamGkh()
        {
            OwnerType = OwnerType.User;
        }

        // TODO: сделать нормальный конструктор

        /// <summary>
        /// 
        /// </summary>
        public virtual StoredReport StoredReport { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual OwnerType OwnerType { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual ParamType ParamType { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Required { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual bool Multiselect { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Label { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Name { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string SqlQuery { get; protected set; }

        /// <summary>
        /// Поле, содержащее доп. информацию для формирование киентского field
        /// При ReportParamType = Catalog, хранит идентификатор зарегистрированного справочника. 
        /// </summary>
        public virtual string Additional { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual object DefaultValue { get; protected set; }

        [JsonIgnore]
        public virtual Type CLRType
        {
            get { return ParamType.ToCLRType(); }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual byte[] DefaultValueBytes
        {
            get { return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(DefaultValue)); }
            set { DefaultValue = JsonConvert.DeserializeObject(Encoding.UTF8.GetString(value)); }
        }

        public virtual DataProviderParam ToDataProviderParam()
        {
            return new DataProviderParam
            {
                Additional = Additional,
                DefaultValue = DefaultValue,
                Label = Label,
                Name = Name,
                ParamType = ParamType,
                Required = Required
            };
        }

        public virtual void AddTo(StoredReport report)
        {
            StoredReport = report;
        }
    }
}
