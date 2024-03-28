namespace Bars.Gkh.DomainService
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Modules.Security;
    using Bars.B4.Security;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Сервис для работы локального администратора
    /// </summary>
    public interface ILocalAdminRoleService
    {
        /// <summary>
        /// Вернуть список локальных администраторов
        /// </summary>
        IList<Role> GetAll();

        /// <summary>
        /// Вернуть список локальных администраторов
        /// </summary>
        IDataResult GetAll(BaseParams baseParams);

        /// <summary>
        /// Вернуть список ролей доступных локальному администратору
        /// </summary>
        IDataResult GetChildRoleList(BaseParams baseParams);

        /// <summary>
        /// Вернуть список ролей доступных локальному администратору
        /// </summary>
        IList<Role> GetChildRoleList(long localAdminId);

        /// <summary>
        /// Является локальным администратором
        /// </summary>
        /// <param name="roleId">Идентификатор проверяемой роли</param>
        bool IsLocalAdmin(long roleId);

        /// <summary>
        /// Текущий пользователь является локальным администратором
        /// </summary>
        bool IsThisUserLocalAdmin();

        /// <summary>
        /// Сериализовать права доступа
        /// </summary>
        /// <param name="treeNode">Дерево доступных разделов</param>
        /// <param name="allowPermissionSet">Набор идентификаторов разрешений</param>
        /// <param name="allowPermissionNodeSet">Набор идентификаторов доступных узлов разрешений</param>
        JToken ConvertTreeToJObject(PermissionTreeNode treeNode, ISet<string> allowPermissionSet, ISet<string> allowPermissionNodeSet = null);
    }
}