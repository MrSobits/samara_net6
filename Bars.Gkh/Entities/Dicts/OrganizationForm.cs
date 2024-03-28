namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Организационно правовая форма
    /// </summary>
    public class OrganizationForm : BaseGkhEntity
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
        /// Код ОКОПФ
        /// </summary>
        public virtual string OkopfCode { get; set; }
    }
}
