namespace Bars.GkhDi.Regions.Tatarstan.Entities.Dict
{
    using Bars.B4.DataAccess;
    
    /// <summary>
    ///  Меры по снижению расходов
    /// </summary>
    public class MeasuresReduceCosts : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string MeasureName { get; set; }
    }
}
