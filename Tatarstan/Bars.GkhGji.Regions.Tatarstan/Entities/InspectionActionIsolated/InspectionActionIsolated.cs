namespace Bars.GkhGji.Regions.Tatarstan.Entities.InspectionActionIsolated
{
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Проверки по мероприятиям без взаимодействия с контролируемыми лицами
    /// </summary>
    public class InspectionActionIsolated: InspectionGji
    {
        /// <summary>
        /// Форма проверки
        /// </summary>
        public virtual TypeFormInspection TypeForm { get; set; }
        
        /// <summary>
        /// Связанная проверка
        /// </summary>
        public virtual InspectionGji ActionIsolated { get; set; }
    }
}