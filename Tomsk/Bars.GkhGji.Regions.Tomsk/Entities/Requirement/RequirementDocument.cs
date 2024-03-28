namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using GkhGji.Entities;
    using Bars.B4.DataAccess;

    /// <summary>
    /// Таблица связи Требования и Документа ГЖИ, который создан из требования
    /// </summary>
    public class RequirementDocument : BaseEntity
    {
        /// <summary>
        /// Требование
        /// </summary>
        public virtual Requirement Requirement { get; set; }

        /// <summary>
        /// Документ ГЖИ
        /// </summary>
        public virtual DocumentGji Document { get; set; }
    }
}
