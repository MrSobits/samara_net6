namespace Sobits.RosReg.Permissions
{
    using Bars.B4.Application;
    using Bars.Gkh.TextValues;

    /// <summary>
    /// PermissionMap для GkhGjiPermissionMap
    /// </summary>
    public class RosRegPermissionMap : PermissionMap
    {
        /// <summary>
        /// Интерфейс для описания текстовых значений пунктов меню
        /// </summary>
        public IMenuItemText MenuItemText { get; set; }

        /// <summary>
        /// Конструктор GkhGjiPermissionMap
        /// </summary>
        public RosRegPermissionMap()
        {
            this.Namespace("Rosreg", "Росрестр");

            #region Риск-ориентированный подход
            this.Namespace("Rosreg.AllRosreg", "Выписки ЕГРН");

            this.Namespace("Rosreg.AllRosreg.RoomEGRN", "Помещения физ лиц");
            this.CRUDandViewPermissions("Rosreg.AllRosreg.RoomEGRN");

            this.Namespace("Rosreg.AllRosreg.Import", "Импорт");
            this.CRUDandViewPermissions("Rosreg.AllRosreg.Import");

            #endregion      


        }
    }
}