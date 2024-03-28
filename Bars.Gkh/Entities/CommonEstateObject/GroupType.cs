namespace Bars.Gkh.Entities.CommonEstateObject
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Тип группы ООИ
    /// </summary>
    public class GroupType : BaseImportableEntity
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