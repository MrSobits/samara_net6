namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Связь дома с документом ДПКР
    /// </summary>
    public class DpkrDocumentRealityObject : BaseEntity
    {
        /// <summary>
        /// Дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
        
        /// <summary>
        /// Документ ДПКР
        /// </summary>
        public virtual DpkrDocument DpkrDocument { get; set; }
        
        /// <summary>
        /// Дом включен
        /// </summary>
        public virtual bool IsIncluded { get; set; }
        
        /// <summary>
        /// Дом исключен
        /// </summary>
        public virtual bool IsExcluded { get; set; }
    }
}