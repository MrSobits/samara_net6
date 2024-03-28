namespace Bars.Gkh.SystemDataTransfer.Meta.ProviderMeta
{
    using System.Collections.Generic;

    using Bars.B4.Utils;

    using Newtonsoft.Json;

    /// <summary>
    /// Объект данных
    /// </summary>
    public class Item
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        [JsonIgnore]
        public long Id => (long)this.Properties["Id"];

        /// <summary>
        /// Свойства
        /// </summary>
        public IDictionary<string, object> Properties { get; set; }

        /// <summary>
        /// Селектор для хранимых данных
        /// </summary>
        /// <param name="name">Имя свойства</param>
        /// <returns>Значение</returns>
        public object this[string name]
        {
            get { return this.Properties.Get(name); }
            set { this.Properties[name] = value; }
        }
    }
}