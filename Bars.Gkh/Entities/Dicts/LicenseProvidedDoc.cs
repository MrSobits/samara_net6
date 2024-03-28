namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Предоставляемые документы для выдачи лицензии
    /// </summary>
    public class LicenseProvidedDoc : BaseImportableEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}