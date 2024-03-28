namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Эксперт рапоряжения ГЖИ
    /// </summary>
    public class DecisionExpert : BaseGkhEntity
    {
        /// <summary>
        /// Распоряжение
        /// </summary>
        public virtual Decision Decision { get; set; }

        /// <summary>
        /// Эксперт
        /// </summary>
        public virtual ExpertGji Expert { get; set; }
    }
}