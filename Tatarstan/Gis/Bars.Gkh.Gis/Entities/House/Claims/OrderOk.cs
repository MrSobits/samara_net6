namespace Bars.Gkh.Gis.Entities.House.Claims
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Заявки из Открытой Казани
    /// </summary>
    [DataContract]
    class OrderOk
    {
        /// <summary>
        /// Заявки
        /// </summary>
        [DataMember(Name="rows")]
        public Dictionary<string, OrderInfoOk> Rows { get; set; }
    }
}
