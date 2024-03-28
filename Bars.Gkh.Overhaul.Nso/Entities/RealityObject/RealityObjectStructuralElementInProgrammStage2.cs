namespace Bars.Gkh.Overhaul.Nso.Entities
{
    using Bars.B4.DataAccess;
    using Gkh.Entities.CommonEstateObject;
    using Overhaul.Entities;

    /// <summary>
    ///  Конструктивный элемент дома в ДПКР
    /// </summary>
    public class RealityObjectStructuralElementInProgrammStage2 : BaseEntity
    {
        public virtual Gkh.Entities.RealityObject RealityObject { get; set; }

        public virtual CommonEstateObject CommonEstateObject { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Строка конструктивных элементов
        /// </summary>
        public virtual string StructuralElements { get; set; }

        public virtual RealityObjectStructuralElementInProgrammStage3 Stage3 { get; set; }
        
        public virtual RealityObjectStructuralElementInProgrammStage2 Clone()
        {
            return this.MemberwiseClone() as RealityObjectStructuralElementInProgrammStage2;
        }
    }
}