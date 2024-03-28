namespace Bars.Gkh.Regions.Voronezh.Permissions
{
    using B4;
    using Bars.B4.Application;
    using Bars.Gkh.TextValues;
    using Entities;

    /// <summary>
    /// PermissionMap для GkhGjiPermissionMap
    /// </summary>
    public class GKHVoronezhPermissionMap : PermissionMap
    {
        /// <summary>
        /// Интерфейс для описания текстовых значений пунктов меню
        /// </summary>
        public IMenuItemText MenuItemText { get; set; }

        /// <summary>
        /// Конструктор GkhGjiPermissionMap
        /// </summary>
        public GKHVoronezhPermissionMap()
        {
            this.Namespace("GkhVoronezh", "Модуль ЖКХ Воронеж");

        

            #region Справочники
            this.Namespace("GkhVoronezh.ImportEGRN", "Импорт выписок ЕГРН");

           

            this.Namespace("GkhVoronezh.ImportEGRN.DataAreaOwner", "Реестр выписок ЕГРН");
            this.CRUDandViewPermissions("GkhVoronezh.ImportEGRN.DataAreaOwner");
            this.Namespace("GkhVoronezh.ImportEGRN.DataAreaOwnerMerger", "Сопоставление помещений и ЛС");
            this.CRUDandViewPermissions("GkhVoronezh.ImportEGRN.DataAreaOwnerMerger");



            #endregion



        }
    }
}