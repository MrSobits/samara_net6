namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Связанные обращения
    /// </summary>
    public class RelatedAppealCits : BaseGkhEntity
    {
        /// <summary>
        /// Родительское обращение
        /// </summary>
        public virtual AppealCits Parent { get; set; }

        /// <summary>
        /// Дочернее обращение
        /// </summary>
        public virtual AppealCits Children { get; set; }
    }
}