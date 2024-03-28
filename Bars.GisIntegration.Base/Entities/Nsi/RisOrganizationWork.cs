namespace Bars.GisIntegration.Base.Entities.Nsi
{
    using System.Collections.Generic;

    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Работы и услуги организации
    /// </summary>
    public class RisOrganizationWork : BaseRisEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Ссылка на НСИ "Вид работ" - Code в ГИС ЖКХ
        /// </summary>
        public virtual string ServiceTypeCode { get; set; }

        /// <summary>
        /// Ссылка на НСИ "Вид работ" - Guid в ГИС ЖКХ
        /// </summary>
        public virtual string ServiceTypeGuid { get; set; }

        /// <summary>
        /// Ссылки на НСИ "Обязательные работы, обеспечивающие надлежащее содержание МКД"
        /// </summary>
        public virtual List<RequiredService> RequiredServices { get; set; }

        /// <summary>
        /// Код ОКЕИ
        /// </summary>
        public virtual string Okei { get; set; }

        /// <summary>
        /// Другая единица измерения
        /// </summary>
        public virtual string StringDimensionUnit { get; set; }
    }

    /// <summary>
    /// Ссылка на НСИ "Обязательные работы, обеспечивающие надлежащее содержание МКД"
    /// </summary>
    public class RequiredService
    {
        /// <summary>
        /// Code в ГИС ЖКХ
        /// </summary>
        public virtual string RequiredServiceCode { get; set; }

        /// <summary>
        /// Guid в ГИС ЖКХ
        /// </summary>
        public virtual string RequiredServiceGuid { get; set; }
    }
}