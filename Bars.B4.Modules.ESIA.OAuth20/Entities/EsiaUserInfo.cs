using System.Collections.Generic;

namespace Bars.B4.Modules.ESIA.OAuth20.Entities
{
    /// <summary>
    /// Информация о пользователе
    /// </summary>
    public class EsiaUserInfo
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Организации
        /// </summary>
        public List<OrganizationInfo> OrganizationsList { get; set; }

        /// <summary>
        /// Данные выбранной организации
        /// </summary>
        public string SelectedOrganizationKey { get; set; }

    }
}
