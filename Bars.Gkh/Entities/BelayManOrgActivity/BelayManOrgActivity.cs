namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Страхование деятельности управляющей организации
    /// </summary>
    public class BelayManOrgActivity : BaseGkhEntity
    {
        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual ManagingOrganization ManagingOrganization { get; set; }
    }
}