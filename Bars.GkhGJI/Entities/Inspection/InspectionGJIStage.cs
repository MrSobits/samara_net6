namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Этап проверки ГЖИ
    /// </summary>
    public class InspectionGjiStage : BaseGkhEntity
    {
        /// <summary>
        /// Проверка ГЖИ
        /// </summary>
        public virtual InspectionGji Inspection { get; set; }

        /// <summary>
        /// Родительский этап
        /// </summary>
        public virtual InspectionGjiStage Parent { get; set; }

        /// <summary>
        /// Тип этапа
        /// </summary>
        public virtual TypeStage TypeStage { get; set; }

        /// <summary>
        /// Позиция этапа в дереве
        /// то есть последовательность от меньшего к большему
        /// </summary>
        public virtual int Position { get; set; }
    }
}