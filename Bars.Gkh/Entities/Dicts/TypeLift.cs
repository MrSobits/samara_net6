namespace Bars.Gkh.Entities.Dicts
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Тип лифта
    /// </summary>
    public class TypeLift : BaseImportableEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}