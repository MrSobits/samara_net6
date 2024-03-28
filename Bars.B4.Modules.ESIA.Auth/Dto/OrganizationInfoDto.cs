namespace Bars.B4.Modules.ESIA.Auth.Dto
{
    /// <summary>
    /// Информация об организации
    /// </summary>
    public class OrganizationInfoDto
    {
        /// <summary>
        /// Идентификатор организации в ЕСИА
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public string ShortName { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public string Ogrn { get; set; }

        /// <summary>
        /// Активность сотрудника (признак НЕ блокировки)
        /// </summary>
        public bool IsActive { get; set; }
    }
}