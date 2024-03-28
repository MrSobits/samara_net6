namespace Bars.Gkh.Gis.Entities.House.Claims
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Организация из Открытой Казани
    /// </summary>
    [DataContract]
    class OrganizationOk
    {
        /// <summary>
        /// ИНН
        /// </summary>
        [DataMember(Name = "inn")]
        public string Inn { get; set; }

        /// <summary>
        /// Форма организации
        /// </summary>
        [DataMember(Name = "orgform")]
        public string OrgForm { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }
    }
}
