namespace Bars.Gkh.Entities
{
    using System;

    using Bars.Gkh.Enums;

    /// <summary>
    /// Обслуживающая организация (Поставщик жилищных услуг)
    /// </summary>
    public class ServiceOrganization : BaseGkhEntity
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
        /// Наименование контрагента (Не хранимое)
        /// </summary>
        public virtual string ContragentName { get; set; }

        // Деятельность

        /// <summary>
        /// Основание прекращения деятельности
        /// </summary>
        public virtual GroundsTermination ActivityGroundsTermination { get; set; }

        /// <summary>
        /// Дата прекращения деятельности
        /// </summary>
        public virtual DateTime? DateTermination { get; set; }

        /// <summary>
        /// Примечание прекращения деятельности
        /// </summary>
        public virtual string DescriptionTermination { get; set; }
    }
}
