namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Orgs;

    public class TatarstanZonalInspection : ZonalInspection
    {
        /// <summary>
        /// КНО.
        /// </summary>
        public virtual ControlOrganization ControlOrganization { get; set; }
    }
}
