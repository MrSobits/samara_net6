namespace Bars.GkhEdoInteg.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.GkhEdoInteg.Enums;

    public class LogRequestsAppCitsEdo : BaseEntity
    {
        public virtual LogRequests LogRequests { get; set; }

        public virtual AppealCitsCompareEdo AppealCitsCompareEdo { get; set; }

        public virtual ActionIntegrationRow ActionIntegrationRow { get; set; }

        /// <summary>
        /// Дата актуальности на момент получения обращения из ЭДО
        /// </summary>
        public virtual DateTime DateActual { get; set; }
    }
}
