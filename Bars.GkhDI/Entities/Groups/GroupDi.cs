namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Группа домов раскрытия информации
    /// </summary>
    public class GroupDi : BaseImportableEntity
    {
        /// <summary>
        /// Деятельность упр организации в периоде раскрытия информации
        /// </summary>
        public virtual DisclosureInfo DisclosureInfo { get; set; }

        /// <summary>
        /// Имя группы
        /// </summary>
        public virtual string Name { get; set; }
    }
}
