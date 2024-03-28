namespace Bars.GkhGji.Regions.Nso.Entities.Disposal
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Документы на согласование
    /// </summary>
    public class DisposalDocConfirm: BaseEntity
    {
        /// <summary>
        /// Приказ 
        /// </summary>
        public virtual Disposal Disposal { get; set; }

        /// <summary>
        /// Наименвоание документа
        /// </summary>
        public virtual string DocumentName { get; set; }
    }
}
