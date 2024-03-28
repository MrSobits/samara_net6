using Bars.B4.DataAccess;
using Bars.GkhCalendar.Enums;

namespace Bars.GkhCalendar.Entities
{
    /// <summary>
    /// Словарь очередей записи на приём
    /// </summary>
    public class AppointmentQueue : BaseEntity
    {
        /// <summary>
        /// Тип организации
        /// </summary>
        public virtual TypeOrganisation TypeOrganisation { get; set; }

        /// <summary>
        /// Подразделение / специалист
        /// </summary>
        public virtual string RecordTo { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Величина временного слота
        /// </summary>
        public virtual TimeSlot TimeSlot { get; set; }
    }
}