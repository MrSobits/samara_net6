namespace Bars.GkhCr.Entities
{
    using Bars.B4.Modules.Security;

    using Gkh.Entities;

    /// <summary>
    /// Участник квалификационного отбора
    /// </summary>
    public class QualificationMember : BaseGkhEntity
    {
        /// <summary>
        /// Период
        /// </summary>
        public virtual Period Period { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Является основным
        /// </summary>
        public virtual bool IsPrimary { get; set; }

        /// <summary>
        /// Роль
        /// </summary>
        public virtual Role Role { get; set; }
    }
}
