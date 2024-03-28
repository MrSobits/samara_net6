namespace Bars.Gkh.Gis.Entities.House.Claims
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// Ответ от Открытой Казани метода получения заявок barsList
    /// </summary>
    [DataContract]
    class BarsListResponse
    {
        /// <summary>
        /// Статусы
        /// </summary>
        [DataMember(Name = "statuses")]
        public Dictionary<string, string> Statuses { get; set; }

        /// <summary>
        /// Организации
        /// </summary>
        [DataMember(Name = "organizations")]
        public Dictionary<string, OrganizationOk>[] Organizations { get; set; }

        /// <summary>
        /// Заявки
        /// </summary>
        [DataMember(Name = "orders")]
        public OrderOk Orders { get; set; }
    }
}
