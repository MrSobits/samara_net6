namespace Bars.GkhCr.Entities
{
    using Gkh.Entities;

    /// <summary>
    /// Базовая смета объекта КР
    /// </summary>
    public class BaseEstimate : BaseGkhEntity
    {
        /// <summary>
        /// Номер
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Обоснование
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// Зарплата механикам
        /// </summary>
        public virtual decimal? MechanicSalary { get; set; }

        /// <summary>
        /// Основная зарплата
        /// </summary>
        public virtual decimal? BaseSalary { get; set; }

        /// <summary>
        /// Трудозатраты механизированные
        /// </summary>
        public virtual decimal? MechanicWork { get; set; }

        /// <summary>
        /// Трудозатраты основные
        /// </summary>
        public virtual decimal? BaseWork { get; set; }

        /// <summary>
        /// Количество всего
        /// </summary>
        public virtual decimal? TotalCount { get; set; }

        /// <summary>
        /// Общая стоимость
        /// </summary>
        public virtual decimal? TotalCost { get; set; }

        /// <summary>
        /// Количество на ед.
        /// </summary>
        public virtual decimal? OnUnitCount { get; set; }

        /// <summary>
        /// Стоимость на ед.
        /// </summary>
        public virtual decimal? OnUnitCost { get; set; }

        /// <summary>
        /// Стоимость материалов
        /// </summary>
        public virtual decimal? MaterialCost { get; set; }

        /// <summary>
        /// Стоимость эксплуатации машин
        /// </summary>
        public virtual decimal? MachineOperatingCost { get; set; }

        /// <summary>
        /// Ед. измерения
        /// </summary>
        public virtual string UnitMeasure { get; set; }
    }
}
