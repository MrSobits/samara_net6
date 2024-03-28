using System;

namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Contragent
{
    /// <summary>
    /// Полная модель контрагента
    /// </summary>
    public class FullContragentDto : ShortContragentDto
    {
        /// <summary>
        /// Организационно-правовая форма
        /// </summary>
        public string OrganizationalForm { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        public string Kpp { get; set; }

        /// <summary>
        /// Дата регистрации
        /// </summary>
        public DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// Юридический адрес
        /// </summary>
        public string LegalAddress { get; set; }

        /// <summary>
        /// Фактический адрес
        /// </summary>
        public string ActualAddress { get; set; }

        /// <summary>
        /// Почтовый адрес
        /// </summary>
        public string PostalAddress { get; set; }

        /// <summary>
        /// Руководитель
        /// </summary>
        public string Supervisor { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// E-mail
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Дополнительный телефон
        /// </summary>
        public string AdditionalTelephone { get; set; }

        /// <summary>
        /// Дополнительный E-mail
        /// </summary>
        public string AdditionalMail { get; set; }

        /// <summary>
        /// Факс
        /// </summary>
        public string Fax { get; set; }

        /// <summary>
        /// Домов в управлении
        /// </summary>
        public int CountActiveHouses { get; set; }

        /// <summary>
        /// Управление домами
        /// </summary>
        public AddressDto[] Addresses { get; set; }
    }
}
