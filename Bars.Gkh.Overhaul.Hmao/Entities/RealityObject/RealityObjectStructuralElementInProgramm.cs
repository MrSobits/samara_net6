namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    using Overhaul.Entities;

    /// <summary>
    ///  Конструктивный элемент дома в ДПКР
    /// </summary>
    public class RealityObjectStructuralElementInProgramm : BaseImportableEntity
    {
        /// <summary>
        /// КЭ
        /// </summary>
        public virtual RealityObjectStructuralElement StructuralElement { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Стоимость услуг
        /// </summary>
        public virtual decimal ServiceCost { get; set; }

        public virtual RealityObjectStructuralElementInProgrammStage2 Stage2 { get; set; }
    }
}