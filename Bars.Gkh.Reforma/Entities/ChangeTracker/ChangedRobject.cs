namespace Bars.Gkh.Reforma.Entities.ChangeTracker
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Изменившийся дом
    /// </summary>
    public class ChangedRobject : PersistentObject
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Период раскрытия
        /// </summary>
        public virtual PeriodDi PeriodDi { get; set; }
    }
}