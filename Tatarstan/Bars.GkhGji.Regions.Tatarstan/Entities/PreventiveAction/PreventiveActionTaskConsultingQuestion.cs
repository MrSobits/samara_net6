namespace Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Задание профилактического мероприятия. Вопрос для консультирования
    /// </summary>
    public class PreventiveActionTaskConsultingQuestion : BaseEntity
    {
        /// <summary>
        /// Задание профилактического мероприятия
        /// </summary>
        public virtual PreventiveActionTask Task { get; set; }

        /// <summary>
        /// Вопрос
        /// </summary>
        public virtual string Question { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// Подконтрольное лицо
        /// </summary>
        public virtual string ControlledPerson { get; set; }
    }
}