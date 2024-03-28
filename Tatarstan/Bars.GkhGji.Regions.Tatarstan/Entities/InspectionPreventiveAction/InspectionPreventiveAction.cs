namespace Bars.GkhGji.Regions.Tatarstan.Entities.InspectionPreventiveAction
{
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Проверка по профилактическим мероприятиям
    /// </summary>
    public class InspectionPreventiveAction : InspectionGji
    {
        /// <summary>
        /// Форма проверки
        /// </summary>
        public virtual TypeFormInspection TypeForm { get; set; }
        
        /// <summary>
        /// Связанная проверка
        /// </summary>
        public virtual InspectionGji PreventiveAction { get; set; }
    }
}