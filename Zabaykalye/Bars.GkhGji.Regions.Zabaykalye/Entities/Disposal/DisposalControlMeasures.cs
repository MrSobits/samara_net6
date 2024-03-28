namespace Bars.GkhGji.Regions.Zabaykalye.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Мероприятия по контролю распоряжения ГЖИ
    /// </summary>
    public class DisposalControlMeasures : BaseEntity
    {
        /// <summary>
        /// Распоряжение
        /// </summary>
        public virtual Disposal Disposal { get; set; }

        /// <summary>
        /// Мероприятие по контролю
        /// </summary>
        public virtual ControlActivity ControlActivity { get; set; }
    }
}