namespace Bars.Gkh.Entities
{
    using System;

    using Bars.Gkh.Enums;

    /// <summary>
    /// Страховые организация
    /// </summary>
    public class BelayOrganization : BaseGkhEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual OrgStateRole OrgStateRole { get; set; }

        /// <summary>
        /// Наименование Контрагента (не хранимое)
        /// </summary>
        public virtual string ContragentName { get; set; }

        /// <summary>
        /// Наименование МО (не хранимое)
        /// </summary>
        public virtual string MuniсipalUnionName { get; set; }

        // Деятельность

        /// <summary>
        /// Описание для деятельности
        /// </summary>
        public virtual string ActivityDescription { get; set; }

        /// <summary>
        /// Основание прекращения деятельности
        /// </summary>
        public virtual GroundsTermination ActivityGroundsTermination { get; set; }

        /// <summary>
        /// Дата прекращения деятельности
        /// </summary>
        public virtual DateTime? DateTermination { get; set; }
    }
}
