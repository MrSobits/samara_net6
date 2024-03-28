namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Настраиваемые поля шаблонной услуги
    /// </summary>
    public class TemplateServiceOptionFields : BaseImportableEntity
    {
        /// <summary>
        /// Шаблонная услуга
        /// </summary>
        public virtual TemplateService TemplateService { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string FieldName { get; set; }

        /// <summary>
        /// Скрытое поле
        /// </summary>
        public virtual bool IsHidden { get; set; }
    }
}
