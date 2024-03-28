namespace Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// НПА задания профилактического мероприятия
    /// </summary>
    public class PreventiveActionTaskRegulation : BaseEntity
    {
        /// <summary>
        /// Нормативно-правовой документ
        /// </summary>
        public virtual NormativeDoc NormativeDoc { get; set; }
        
        /// <summary>
        /// Задание профилактического мероприятия
        /// </summary>
        public virtual PreventiveActionTask Task { get; set; }
    }
}