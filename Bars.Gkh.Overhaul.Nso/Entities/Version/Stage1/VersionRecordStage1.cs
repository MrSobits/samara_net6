namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.B4.DataAccess;
    using Gkh.Entities;
    using Overhaul.Entities;

    public class VersionRecordStage1 : BaseEntity
    {
        public virtual VersionRecordStage2 Stage2Version { get; set; }

        public virtual RealityObject RealityObject { get; set; }

        public virtual RealityObjectStructuralElement StructuralElement { get; set; }

        /// <summary>
        /// плановый год, расчитанный
        /// </summary>
        public virtual int Year { get; set; }

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

        /// <summary>
        /// Сумма без учета услуг
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Сумма услуг
        /// </summary>
        public virtual decimal SumService { get; set; }
    }
}