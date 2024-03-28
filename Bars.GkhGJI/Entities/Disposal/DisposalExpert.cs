namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Base;

    /// <summary>
    /// Эксперт рапоряжения ГЖИ
    /// </summary>
    public class DisposalExpert : BaseGkhEntity, IEntityUsedInErknm
    {
        /// <summary>
        /// Распоряжение
        /// </summary>
        public virtual Disposal Disposal { get; set; }

        /// <summary>
        /// Эксперт
        /// </summary>
        public virtual ExpertGji Expert { get; set; }

        /// <inheritdoc />
        public virtual string ErknmGuid { get; set; }
    }
}