namespace Bars.GkhGji.Permissions
{
    using Bars.B4;

    /// <summary>
    /// Интерфейс IGjiPermission для получения приоритета пермишна и PermissionMap 
    /// </summary>
    public interface IGjiPermission
    {
        /// <summary>
        /// Поле для приоритета получения пермишна
        /// </summary>
        int Priority { get; }

        /// <summary>
        /// Получить пермишн
        /// </summary>
        /// <returns>PermissionMap</returns>
        PermissionMap GetPermissionMap();
    }
}
