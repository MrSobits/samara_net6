namespace Bars.GkhGji.Regions.Smolensk.Entities
{
    using Bars.B4.DataAccess;
    /// <summary>
    /// Мероприятия по контролю распоряжения ГЖИ
    /// </summary>
    public class DisposalControlMeasures : BaseEntity
    {
        /// <summary>
        /// Распоряжение
        /// </summary>
        public virtual GkhGji.Entities.Disposal Disposal { get; set; }

        /// <summary>
        /// Мероприятия по контролю
        /// </summary>
        public virtual string ControlMeasuresName { get; set; }
    }
}