namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    using Gkh.Entities;
    using Gkh.Entities.CommonEstateObject;

    /// <summary>
    ///  Конструктивный элемент дома в ДПКР
    /// </summary>
    public class RealityObjectStructuralElementInProgrammStage2 : BaseImportableEntity
    {
        public virtual RealityObject RealityObject { get; set; }

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