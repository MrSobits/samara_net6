namespace Bars.GkhGji.Regions.Nnovgorod.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

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
        /// Мероприятия по контролю
        /// </summary>
        public virtual string ControlMeasuresName { get; set; }
    }
}