namespace Bars.Gkh.RegOperator.Regions.Tyumen.Permissions
{
    using Bars.B4;
    using Bars.Gkh.RegOperator.Regions.Tyumen.Entities;

    /// <summary>
    /// Маппинг прав доступа
    /// </summary>
    public class GkhTyumenPermissionMap : PermissionMap
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public GkhTyumenPermissionMap()
        {
            #region Справочники

            this.Namespace("Gkh.Dictionaries", "Справочники");

            this.Namespace("Gkh.Dictionaries.RequestStatePerson", "Получатели запросов на редактирование");
            this.CRUDandViewPermissions("Gkh.Dictionaries.RequestStatePerson");
            #endregion
        }
    }
}