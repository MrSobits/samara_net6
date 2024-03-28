namespace Bars.Gkh.Reforma.PerformerActions.SetHouseLinkToOrganization
{
    using System;

    public class SetHouseLinkToOrganizationParams
    {
        /// <summary>
        ///     Идентификатор дома
        /// </summary>
        public int ExternalId { get; set; }

        /// <summary>
        ///     ИНН УО
        /// </summary>
        public string Inn { get; set; }

        /// <summary>
        ///     Дата начала управления
        /// </summary>
        public DateTime DateStart { get; set; }

        /// <summary>
        ///     Основание взятия в управления домом
        /// </summary>
        public string management_reason { get; set; }
    }
}