namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Специальность
    /// </summary>
    public class Specialty : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}
