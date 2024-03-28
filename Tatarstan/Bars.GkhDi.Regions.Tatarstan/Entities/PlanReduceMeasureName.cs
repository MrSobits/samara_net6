namespace Bars.GkhDi.Regions.Tatarstan.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Regions.Tatarstan.Entities.Dict;

    /// <summary>
    ///  Сущность для связи названия меры из справочника с планом
    /// </summary>
    public class PlanReduceMeasureName : BaseEntity
    {
        /// <summary>
        /// Работы по плану мер по снижению расходов
        /// </summary>
        public virtual PlanReductionExpenseWorks PlanReductionExpenseWorks { get; set; }

        /// <summary>
        ///  Меры по снижению расходов
        /// </summary>
        public virtual MeasuresReduceCosts MeasuresReduceCosts { get; set; }
    }
}
