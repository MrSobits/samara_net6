namespace Bars.Gkh.Gasu.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Gasu.Enums;

    /// <summary>
    /// Показатель ГАСУ
    /// </summary>
    public class GasuIndicator : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Ед.измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }

        /// <summary>
        /// Периодичность
        /// </summary>
        public virtual Periodicity Periodicity { get; set; }

        /// <summary>
        /// Модуль ЕБИР
        /// </summary>
        public virtual EbirModule EbirModule { get; set; }     
    }
}