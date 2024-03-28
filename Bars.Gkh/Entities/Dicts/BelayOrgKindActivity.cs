namespace Bars.Gkh.Entities.Dicts
{
    using Bars.Gkh.ImportExport;

    /// <summary>
    /// Вид деятельности страховой организации
    /// </summary>
    public class BelayOrgKindActivity : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}
