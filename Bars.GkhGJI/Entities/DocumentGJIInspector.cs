namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Base;

    /// <summary>
    /// Инспекторы документа ГЖИ
    /// </summary>
    public class DocumentGjiInspector : BaseGkhEntity, IEntityUsedInErp, IEntityUsedInErknm
    {
        /// <summary>
        /// Документ ГЖИ
        /// </summary>
        public virtual DocumentGji DocumentGji { get; set; }

        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Порядковый номер инспектора для текущего документа
        /// </summary>
        public virtual int Order { get; set; }

        /// <inheritdoc />
        public virtual string ErpGuid { get; set; }

        /// <inheritdoc />
        public virtual string ErknmGuid { get; set; }
    }
}