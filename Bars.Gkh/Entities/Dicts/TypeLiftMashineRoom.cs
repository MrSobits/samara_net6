namespace Bars.Gkh.Entities.Dicts
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Расположение машинного помещения
    /// </summary>
    public class TypeLiftMashineRoom : BaseImportableEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
    }
}