namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Подтематика тематики 
    /// </summary>
    public class StatSubjectSubsubjectGji : BaseGkhEntity
    {
        /// <summary>
        /// Тематика
        /// </summary>
        public virtual StatSubjectGji Subject { get; set; }

        /// <summary>
        /// Подтематика
        /// </summary>
        public virtual StatSubsubjectGji Subsubject { get; set; }
    }
}