namespace Bars.Gkh.Gis.Entities.PersonalAccount.PublicControlClaims
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Ответ от Народного Контроля метода получения заявок orders.json
    /// </summary>
    [DataContract]
    public class PublicControlResponse
    {
        /// <summary>
        /// Id
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Номер л/с
        /// </summary>
        [DataMember(Name = "account_number")]
        public string AccountNumber { get; set; }

        /// <summary>
        /// Id категории
        /// </summary>
        [DataMember(Name = "category_id")]
        public string CategoryId { get; set; }

        /// <summary>
        /// Название категории
        /// </summary>
        [DataMember(Name = "category_name")]
        public string CategoryName { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        [DataMember(Name = "address")]
        public string Address { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>
        /// Территория
        /// </summary>
        [DataMember(Name = "territory_name")]
        public string Territory { get; set; }

        /// <summary>
        /// Id организации
        /// </summary>
        [DataMember(Name = "organization_id")]
        public string OrganizationId { get; set; }

        /// <summary>
        /// Название организации
        /// </summary>
        [DataMember(Name = "organization_name")]
        public string OrganizationName { get; set; }

        /// <summary>
        /// StateId
        /// </summary>
        [DataMember(Name = "status_id")]
        public string StateId { get; set; }

        /// <summary>
        /// StateName
        /// </summary>
        [DataMember(Name = "status_name")]
        public string StateName { get; set; }

        /// <summary>
        /// Дата создания
        /// </summary>
        [DataMember(Name = "created_date")]
        public string CreatedDate { get; set; }

        /// <summary>
        /// Дата последнего изменения статуса
        /// </summary>
        [DataMember(Name = "update_date")]
        public string UpdateDate { get; set; }
    }
}