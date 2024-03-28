namespace Bars.Gkh.Modules.Gkh1468.Entities.Dict
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Вид потребителя
    /// </summary>
    public class TypeCustomer : BaseImportableEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}