namespace Bars.Gkh.Entities.CommonEstateObject
{
    using System.Collections.Generic;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Группа конструктивных элементов
    /// </summary>
    public class StructuralElementGroup : BaseImportableEntity
    {
        private List<FormulaParams> formulaParams;
        
        /// <summary>
        /// Объект общего имущества
        /// </summary>
        public virtual CommonEstateObject CommonEstateObject { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Текст формула
        /// </summary>
        public virtual string Formula { get; set; }

        /// <summary>
        /// Название формулы
        /// </summary>
        public virtual string FormulaName { get; set; }

        /// <summary>
        /// Описание формулы
        /// </summary>
        public virtual string FormulaDescription { get; set; }

        /// <summary>
        /// Используется в расчете
        /// </summary>
        public virtual bool UseInCalc { get; set; }

        /// <summary>
        /// Параметры формулы
        /// </summary>
        public virtual List<FormulaParams> FormulaParams
        {
            get
            {
                return formulaParams ?? (formulaParams = new List<FormulaParams>());
            }

            set
            {
                formulaParams = value;
            }
        }

        /// <summary>
        /// Обязательность группы конструктивных элементов
        /// </summary>
        public virtual bool Required { get; set; }
    }
}