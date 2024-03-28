namespace Bars.GkhGji.Regions.Tatarstan.Entities
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Orgs;

    /// <summary>
    /// Связь КНО и вида контроля.
    /// </summary>
    public class ControlOrganizationControlTypeRelation : BaseEntity
    {
        /// <summary>
        /// КНО.
        /// </summary>
        public virtual ControlOrganization ControlOrganization { get; set; }

        /// <summary>
        /// Вид контроля.
        /// </summary>
        public virtual ControlType ControlType { get; set; }
    }
}
