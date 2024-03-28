namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Gkh.Entities;

    /// <summary>
    ///  Конструктивный элемент дома в ДПКР
    /// </summary>
    public class RealityObjectStructuralElementInProgrammStage3 : BaseEntity
    {
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Плановый Год
        /// </summary>
        public virtual int Year { get; set; }

        /// <summary>
        /// Строка объектов общего имущества
        /// </summary>
        public virtual string CommonEstateObjects { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public virtual int IndexNumber { get; set; }

        /// <summary>
        /// Балл
        /// </summary>
        public virtual decimal Point { get; set; }

        /// <summary>
        /// Нехранимое поле Потребность капремонта
        /// </summary>
        public virtual int NeedOverhaul { get; set; }

        public virtual RealityObjectStructuralElementInProgrammStage3 Clone()
        {
            return MemberwiseClone() as RealityObjectStructuralElementInProgrammStage3;
        }

        /// <summary>
        /// Значения критериев сортировки
        /// </summary>
        public virtual List<StoredPriorityParam> StoredCriteria { get; set; }

        public RealityObjectStructuralElementInProgrammStage3()
        {
            StoredCriteria = new List<StoredPriorityParam>();
        }
    }

    /// <summary>
    /// Значение параметра приоритета
    /// </summary>
    public sealed class StoredPriorityParam
    {
        /// <summary>
        /// Критерий
        /// </summary>
        public string Criterion { get; set; }

        /// <summary>
        /// Значение
        /// </summary>
        public string Value { get;set; }
    }
}