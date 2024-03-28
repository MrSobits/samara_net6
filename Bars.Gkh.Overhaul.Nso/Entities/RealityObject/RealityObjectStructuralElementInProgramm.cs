namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.B4.DataAccess;
    using Overhaul.Entities;

    /// <summary>
    ///  Конструктивный элемент дома в ДПКР
    /// </summary>
    public class RealityObjectStructuralElementInProgramm : BaseEntity
    {
        /// <summary>
        /// КЭ
        /// </summary>
        public virtual RealityObjectStructuralElement StructuralElement { get; set; }


        public virtual RealityObjectStructuralElementInProgrammStage2 Stage2 { get; set; }

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


        /// <summary>
        /// Год последнего ремонта, который был в КЭ при расчете
        /// </summary>
        public virtual int LastOverhaulYear { get; set; }

        /// <summary>
        /// Объем, который был в КЭ при расчете 
        /// </summary>
        public virtual decimal Volume { get; set; }

        /// <summary>
        /// Износ, который был в КЭ при расчете
        /// </summary>
        public virtual decimal Wearout { get; set; }

    }
}