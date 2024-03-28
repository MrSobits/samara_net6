namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Base;

    /// <summary>
    /// Предоставляемые документы рапоряжения ГЖИ
    /// </summary>
    public class DisposalProvidedDoc : BaseEntity, IEntityUsedInErknm
    {
        /// <summary>
        /// Предоставляемый документа
        /// </summary>
        public virtual ProvidedDocGji ProvidedDoc { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Распоряжение
        /// </summary>
        public virtual Disposal Disposal { get; set; }

        /// <inheritdoc />
        public virtual string ErknmGuid { get; set; }
    }
}