namespace Bars.Gkh.Modules.Gkh1468.Entities
{
    using System;
    
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Поставщик ресурсов
    /// </summary>
    public class PublicServiceOrg : BaseImportableEntity
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
