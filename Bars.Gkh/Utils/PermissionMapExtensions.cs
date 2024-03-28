namespace Bars.Gkh.Utils
{
    using Bars.B4;

    public static class PermissionMapExtensions
    {
        /// <summary>
        /// Права доступа на просмотр и редактирование
        /// <para>_View и _Edit</para>
        /// </summary>
        public static void FieldPermission(this PermissionMap source, string permissionId, string description)
        {
            source.Permission(permissionId + "_View", description + " - Просмотр");
            source.Permission(permissionId + "_Edit", description + " - Редактирование");
        }
    }
}