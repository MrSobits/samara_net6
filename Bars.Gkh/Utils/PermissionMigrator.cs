namespace Bars.Gkh.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;

    /// <summary>
    /// Класс-помощник для накатывания миграция по правам поступа (добавляет права по ролям и статусам, если они отсутствуют)
    /// </summary>
    public static class PermissionMigrator
    {
        /// <summary>
        /// Применить права доступа для указанного списка ролей или для всех
        /// </summary>
        /// <param name="provider">Провайдер мигратора</param>
        /// <param name="namespaceOrPermission">Пространство имен или право доступа для применения</param>
        /// <param name="rolesNames">Имена ролей</param>
        public static void ApplyRolePermission(ITransformationProvider provider, string namespaceOrPermission, string[] rolesNames)
        {
            var rolesSql = "SELECT id FROM b4_role" + (rolesNames.IsNotEmpty()
                ? $"WHERE LOWER(name) IN ({string.Join(",", rolesNames.Select(x => $"'{x.ToLower()}'"))})"
                : string.Empty);

            var roleIds = new List<long>();
            using (var reader = provider.ExecuteQuery(rolesSql))
            {
                while (reader.Read())
                {
                    roleIds.Add(reader.GetInt64(0));
                }
            }

            PermissionMigrator.ApplyRolePermission(provider, namespaceOrPermission, roleIds.ToArray());
        }

        /// <summary>
        /// Применить права доступа для ролей, у которых есть права на указанную в параметре <paramref name="permissionToApplyRole"/> роль
        /// </summary>
        /// <param name="provider">Провайдер мигратора</param>
        /// <param name="namespaceOrPermission">Пространство имен или право доступа для применения</param>
        /// <param name="permissionToApplyRole">Роль для проверки</param>
        public static void ApplyRolePermission(ITransformationProvider provider, string namespaceOrPermission, string permissionToApplyRole)
        {
            var rolesSql = $"SELECT role_id FROM b4_role_permission WHERE permission_id = '{permissionToApplyRole}'";

            var roleIds = new List<long>();
            using (var reader = provider.ExecuteQuery(rolesSql))
            {
                while (reader.Read())
                {
                    roleIds.Add(reader.GetInt64(0));
                }
            }

            PermissionMigrator.ApplyRolePermission(provider, namespaceOrPermission, roleIds.ToArray());
        }

        /// <summary>
        /// Применить права доступа по статусам для ролей, у которых есть права на указанную в параметре <paramref name="permissionToApplyRole"/> роль
        /// </summary>
        /// <param name="provider">Провайдер мигратора</param>
        /// <param name="namespaceOrPermission">Пространство имен или право доступа для применения</param>
        /// <param name="permissionToApplyRole">Роль для проверки</param>
        public static void ApplyStatePermission(ITransformationProvider provider, string namespaceOrPermission, string permissionToApplyRole)
        {
            var rolesSql = $"SELECT role_id, state_id FROM B4_STATE_ROLE_PERMISSION WHERE permission_id = '{permissionToApplyRole}'";

            var roleIds = new List<Tuple<long, long>>();
            using (var reader = provider.ExecuteQuery(rolesSql))
            {
                while (reader.Read())
                {
                    roleIds.Add(Tuple.Create(reader.GetInt64(0), reader.GetInt64(1)));
                }
            }

            PermissionMigrator.ApplyRolePermission(provider, namespaceOrPermission, roleIds.ToArray());
        }

        /// <summary>
        /// Разрешить права доступа для всех ролей
        /// </summary>
        /// <param name="provider">Провайдер мигратора</param>
        /// <param name="permissionKeys">Коллекция ключей прав доступа</param>
        public static void GrantForAllRole(ITransformationProvider provider, string[] permissionKeys)
        {
            var appliablePermissions = PermissionMigrator.GetAppliablePermissions(permissionKeys);
            var roleIds = PermissionMigrator.GetAllRoleIds(provider);

            foreach (var roleId in roleIds)
            {
                foreach (var permission in appliablePermissions)
                {
                    var checkPermissionQuery = $@"SELECT COUNT(*) > 0 FROM b4_role_permission 
                            WHERE permission_id = '{permission}'
                            AND role_id = {roleId}";

                    if (!provider.ExecuteScalar(checkPermissionQuery).ToBool())
                    {
                        var sqlToInsert =
                            $@"INSERT INTO b4_role_permission (role_id, permission_id)
                            VALUES
                            {$"({roleId}, '{permission}')"}";

                        provider.ExecuteNonQuery(sqlToInsert);
                    }
                }
            }
        }

        /// <summary>
        /// Разрешить права доступа для всех ролей и для всех статусов сущности
        /// </summary>
        /// <param name="provider">Провайдер мигратора</param>
        /// <param name="entityTypeId">Тип сущности</param>
        /// <param name="permissionKeys">Коллекция ключей прав доступа</param>
        public static void GrantForAllState(ITransformationProvider provider, string entityTypeId, string[] permissionKeys)
        {
            var appliablePermissions = PermissionMigrator.GetAppliablePermissions(permissionKeys);
            var roleIds = PermissionMigrator.GetAllRoleIds(provider);
            var stateIds = PermissionMigrator.GetStateIds(provider, entityTypeId);
            var roleStates = new List<Tuple<long, long>>();

            foreach (var roleId in roleIds)
            {
                roleStates.AddRange(stateIds.Select(stateId => Tuple.Create(roleId, stateId)));
            }

            foreach (var permission in appliablePermissions)
            {
                foreach (var roleState in roleStates)
                {
                    var checkPermissionQuery = $@"SELECT COUNT(*) > 0 FROM b4_state_role_permission 
                            WHERE permission_id = '{permission}'
                            AND role_id = {roleState.Item1}
                            AND state_id = {roleState.Item2}";

                    if (!provider.ExecuteScalar(checkPermissionQuery).ToBool())
                    {
                        var sqlToInsert = $@"INSERT INTO b4_state_role_permission
                            (object_version, object_create_date, object_edit_date, role_id, permission_id, state_id)
                            VALUES (0, now(), now(), {roleState.Item1}, '{permission}', {roleState.Item2})";

                        provider.ExecuteNonQuery(sqlToInsert);
                    }
                }
            }
        }

        private static List<long> GetAllRoleIds(ITransformationProvider provider)
        {
            var roleIds = new List<long>();
            var rolesSql = "SELECT ID FROM B4_ROLE";
            using (var reader = provider.ExecuteQuery(rolesSql))
            {
                while (reader.Read())
                {
                    roleIds.Add(reader["id"].ToLong());
                }
            }
            return roleIds;
        }

        private static List<long> GetStateIds(ITransformationProvider provider, string entityTypeId)
        {
            var stateIds = new List<long>();
            var stateSql = $"select id from b4_state where type_id = '{entityTypeId}'";
            using (var reader = provider.ExecuteReader(stateSql))
            {
                while (reader.Read())
                {
                    stateIds.Add(reader["id"].ToLong());
                }
            }

            return stateIds;
        }

        private static void ApplyRolePermission(ITransformationProvider provider, string namespaceOrPermission, long[] roleIds)
        {
            if (roleIds.Length == 0) 
            {
                return;
            }
          
            var appliablePermissions = PermissionMigrator.GetAppliablePermissions(namespaceOrPermission);

            foreach (var permission in appliablePermissions)
            {
                var sqlToDelete = $@"DELETE FROM b4_role_permission 
                    WHERE role_id in ({roleIds.AggregateWithSeparator(x => x.ToStr(), ",")}) and
                          permission_id = '{permission}'";

                provider.ExecuteNonQuery(sqlToDelete);

                var sqlToInsert = $@"INSERT INTO b4_role_permission (role_id, permission_id)
                    VALUES
                    {roleIds.Select(x => $"({x}, '{permission}')").AggregateWithSeparator(",")}";

                provider.ExecuteNonQuery(sqlToInsert);
            }
        }

        private static void ApplyRolePermission(ITransformationProvider provider, string namespaceOrPermission, Tuple<long, long>[] roleStates)
        {
            if (roleStates.Length == 0) 
            {
                return;
            }
          
            var appliablePermissions = PermissionMigrator.GetAppliablePermissions(namespaceOrPermission);

            foreach (var permission in appliablePermissions)
            {
                foreach (var roleState in roleStates)
                {
                    var checkPermissionQuery = $@"SELECT COUNT(*) > 0 FROM b4_state_role_permission 
                            WHERE permission_id = '{permission}'
                            AND role_id = {roleState.Item1}
                            AND state_id = {roleState.Item2}";

                    if (!provider.ExecuteScalar(checkPermissionQuery).ToBool())
                    {
                        var sqlToInsert = $@"INSERT INTO b4_state_role_permission
                            (object_version, object_create_date, object_edit_date, role_id, permission_id, state_id)
                            VALUES (0, now(), now(), {roleState.Item1}, '{permission}', {roleState.Item2})";

                        provider.ExecuteNonQuery(sqlToInsert);
                    }
                }
            }
        }

        private static string[] GetAppliablePermissions(string namespaceOrPermission)
        {
            var container = ApplicationContext.Current.Container;
            var permissionProvider = container.Resolve<IPermissionProvider>();

            return permissionProvider.GetAllPermissions()
                .Where(x => x.PermissionID.StartsWith(namespaceOrPermission) && !x.IsNamespace)
                .Select(x => x.PermissionID)
                .ToArray();
        }

        private static string[] GetAppliablePermissions(string[] permissionKeys)
        {
            var container = ApplicationContext.Current.Container;
            var permissionProvider = container.Resolve<IPermissionProvider>();

            return permissionProvider.GetAllPermissions()
                .Where(x => !x.IsNamespace)
                .Where(x => permissionKeys.Contains(x.PermissionID))
                .Select(x => x.PermissionID)
                .ToArray();
        }
    }
}