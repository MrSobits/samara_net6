namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///  Конструктивный элемент дома в ДПКР
    /// </summary>
    public class RealityObjectStructuralElementInProgrammStage3 : BaseImportableEntity, IStage3Entity
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
        [Obsolete]
        public virtual decimal Point { get; set; }

        public virtual RealityObjectStructuralElementInProgrammStage3 Clone()
        {
            return this.MemberwiseClone() as RealityObjectStructuralElementInProgrammStage3;
        }

        /// <summary>
        /// Значения критериев сортировки
        /// </summary>
        public virtual List<StoredPriorityParam> StoredCriteria { get; set; }

        /// <summary>
        /// Значения параметров очередности по баллам
        /// </summary>
        public virtual List<StoredPointParam> StoredPointParams { get; set; }

        public RealityObjectStructuralElementInProgrammStage3()
        {
            StoredCriteria = new List<StoredPriorityParam>();
            StoredPointParams = new List<StoredPointParam>();
        }
    }
}