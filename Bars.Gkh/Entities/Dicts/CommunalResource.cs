namespace Bars.Gkh.Entities.Dicts
{
    /// <summary>
    /// Справочник "Коммунальный ресурс"
    /// </summary>
    public class CommunalResource : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}
