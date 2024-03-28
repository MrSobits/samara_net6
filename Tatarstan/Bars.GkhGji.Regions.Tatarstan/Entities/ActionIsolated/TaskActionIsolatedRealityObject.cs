namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Дома Задания КНМ без взаимодействия с контролируемыми лицами
    /// </summary>
    public class TaskActionIsolatedRealityObject : BaseGkhEntity
    {
        /// <summary>
        /// Задание
        /// </summary>
        public virtual TaskActionIsolated Task { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }
    }
}