namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Привязка документа ДПКР к дому
    /// </summary>
    public class DpkrDocumentRealityObject : BaseEntity
    {
        /// <summary>
        /// Документ ДПКР
        /// </summary>
        public virtual DpkrDocument DpkrDocument { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Исключен?
        /// </summary>
        public virtual bool IsExcluded { get; set; }
    }
}