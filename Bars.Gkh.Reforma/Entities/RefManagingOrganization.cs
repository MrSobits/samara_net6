namespace Bars.Gkh.Reforma.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Reforma.Enums;

    /// <summary>
    /// Синхронизируемая управляющая организация
    /// </summary>
    public class RefManagingOrganization : BaseEntity
    {
        /// <summary>
        /// ИНН УО
        /// </summary>
        public virtual string Inn { get; set; }

        /// <summary>
        /// Дата запроса
        /// </summary>
        public virtual DateTime RequestDate { get; set; }

        /// <summary>
        /// Дата обработки заявки
        /// </summary>
        public virtual DateTime? ProcessDate { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestStatus RequestStatus { get; set; }
    }
}