namespace Bars.GkhCr.Entities
{
    using Bars.B4.Modules.Security;

    using Gkh.Entities;

    /// <summary>
    /// Роли участника квалификационного отбора
    /// </summary>
    public class QualificationMemberRole : BaseGkhEntity
    {
        /// <summary>
        /// Период
        /// </summary>
        public virtual QualificationMember QualificationMember { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public virtual Role Role { get; set; }
    }
}
