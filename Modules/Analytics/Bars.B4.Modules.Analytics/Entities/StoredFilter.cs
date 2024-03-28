namespace Bars.B4.Modules.Analytics.Entities
{
    using System.Text;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Newtonsoft.Json;

    /// <summary>
    /// 
    /// </summary>
    public class StoredFilter : BaseEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual string ProviderKey { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DataFilter DataFilter { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual byte[] Filter
        {
            get { return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(DataFilter)); }
            set { DataFilter = JsonConvert.DeserializeObject<DataFilter>(Encoding.UTF8.GetString(value)); }
        }
    }
}
