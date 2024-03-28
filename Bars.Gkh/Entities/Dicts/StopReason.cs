namespace Bars.Gkh.Entities.Dicts
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Справночник "Причина расторжения договора"
    /// </summary>
    public class StopReason : BaseImportableEntity
    {
        /// <summary>
        /// Наименование причины
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}
