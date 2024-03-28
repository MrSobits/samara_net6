namespace Bars.GkhGji.Regions.Tatarstan.Models
{
    using System;

    /// <summary>
    /// Информация о направленных обращениях в СОПР для электронного письма
    /// </summary>
    public class RapidResponseAppealMailInfo
    {
        /// <summary>
        /// Наименование организации
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// Номер обращения
        /// </summary>
        public string AppealNumber { get; set; }

        /// <summary>
        /// Крайний срок ответа
        /// </summary>
        public DateTime ControlPeriod { get; set; }

        /// <summary>
        /// Адрес
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Идентификатор контрагента, которому направлено письмо
        /// </summary>
        public long ContragentId { get; set; }
    }
}