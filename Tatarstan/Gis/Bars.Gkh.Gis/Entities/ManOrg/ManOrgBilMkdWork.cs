namespace Bars.Gkh.Gis.Entities.ManOrg
{
    using Gkh.Entities.Dicts;
    using Gkh.Entities;

    /// <summary>
    /// Работы и услуги управляющей организации по МКД
    /// </summary>
    public class ManOrgBilMkdWork : BaseGkhEntity
    {
        /// <summary>
        /// Работы и услуги управляющей организации
        /// </summary>
        public virtual ManOrgBilWorkService WorkService { get; set; }

        /// <summary>
        /// Работа по МКД
        /// </summary>
        public virtual ContentRepairMkdWork MkdWork { get; set; }
    }
}
