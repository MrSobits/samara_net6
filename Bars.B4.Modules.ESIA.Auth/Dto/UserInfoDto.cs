namespace Bars.B4.Modules.ESIA.Auth.Dto
{
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Информация о пользователе из ЕСИА
    /// </summary>
    public class UserInfoDto
    {
        /// <summary>
        /// Идентификатор учетной записи в ЕСИА
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Полное имя пользователя (ФИО)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Пол
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public string BirthDate { get; set; }

        /// <summary>
        /// Идентификатор выбранной организации
        /// </summary>
        public string SelectedOrganizationId { get; set; }

        /// <summary>
        /// Идентификатор выбранного контрагента
        /// </summary>
        public long? SelectedContragentId { get; set; }

        /// <summary>
        /// Список организаций пользователя в ЕСИА
        /// </summary>
        public IList<OrganizationInfoDto> Organizations { get; set; }

        /// <summary>
        /// Список сопоставленных контрагентов
        /// </summary>
        public IList<ContragentInfoDto> MatchedContragents { get; set; }

        /// <summary>
        /// Выбранная организация
        /// </summary>
        public OrganizationInfoDto SelectedOrganization => this.Organizations
            .FirstOrDefault(x => x.Id == this.SelectedOrganizationId);
    }
}