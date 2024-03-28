namespace Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Замечание действия акта проверки
    /// </summary>
    public class ActCheckActionRemark : BaseEntity
    {
        /// <summary>
        /// Действие акта проверки
        /// </summary>
        public virtual ActCheckAction ActCheckAction { get; set; }

        /// <summary>
        /// Замечание
        /// </summary>
        public virtual string Remark { get; set; }

        /// <summary>
        /// ФИО участника
        /// </summary>
        public virtual string MemberFio { get; set; }
    }
}