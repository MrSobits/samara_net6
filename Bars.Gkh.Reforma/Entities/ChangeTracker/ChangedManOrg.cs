namespace Bars.Gkh.Reforma.Entities.ChangeTracker
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Изменение УО
    /// </summary>
    public class ChangedManOrg : PersistentObject
    {
        /// <summary>
        /// УО
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }

        /// <summary>
        /// Период раскрытия. Может быть null. В этом случае подразумевается
        /// изменение по всем активным периодам.
        /// </summary>
        public virtual PeriodDi PeriodDi { get; set; }
    }
}