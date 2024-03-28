namespace Bars.Gkh.DomainService.Permission.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.B4.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Сервис настройки прав доступа на форме
    /// </summary>
    public class FormPermssionService : IFormPermssionService
    {
        public IWindsorContainer Container { get; set; }
        public IPermissionProvider PermissionProvider { get; set; }
        public IStateProvider StateProvider { get; set; }
        public IRepository<StateRolePermission> StateRolePermissionRepository { get; set; }
        public IRepository<RolePermission> RolePermissionRepository { get; set; }
        public IFilterPermissionService FilterPermissionService { get; set; }
        public ILocalAdminRoleService LocalAdminRoleService { get; set; }

        /// <summary>
        /// Получить права доступа для формы
        /// </summary>
        public IDataResult GetFormPermissions(BaseParams baseParams)
        {
            var stateful = baseParams.Params.GetAs<bool>("stateful");
            var roleId = baseParams.Params.GetAsId("roleId");
            var typeId = baseParams.Params.GetAs<string>("typeId");
            var stateId = baseParams.Params.GetAsId("stateId");
            var formPermissions = baseParams.Params.GetAs<List<string>>("formPermissions");

            if (roleId == 0 || stateful && (typeId.IsNull() || stateId == 0))
            {
                return new BaseDataResult(Enumerable.Empty<RolePermissionProxy>());
            }

            var resultPermissions = new List<RolePermissionProxy>();
            var root = this.Container.Resolve<PermissionTreeNode>();
            var allowPermissions = this.FilterPermissionService.GetAllowRolePermissionNodes(roleId);

            this.ResolvePermission(resultPermissions, root, formPermissions);
            resultPermissions = resultPermissions.WhereIf(this.LocalAdminRoleService.IsThisUserLocalAdmin(), x => allowPermissions.Contains(x.PermissionId))
                .ToList();

            if (stateful)
            {
                var statefulEntityInfo = this.StateProvider.GetStatefulEntityInfo(typeId);

                var permissionKeysForEntity = this.PermissionProvider.GetAllPermissions()
                    .Where(x => x.EntityType == statefulEntityInfo.Type)
                    .Select(x => x.PermissionID)
                    .ToList();

                resultPermissions = resultPermissions
                    .Where(x => permissionKeysForEntity.Contains(x.PermissionId))
                    .ToList();
            }

            this.FillGrant(stateful, resultPermissions, roleId, stateId);

            return resultPermissions
                .AsQueryable()
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }

        /// <summary>
        /// Получить список типов на форме
        /// </summary>
        public IDataResult GetEntityTypes(BaseParams baseParams)
        {
            var formPermissions = baseParams.Params.GetAs<List<string>>("formPermissions");
            var roleId = baseParams.Params.GetAsId("roleId");

            var allowStatefulEntities = this.FilterPermissionService.GetAllowStatefulEntities(roleId);

            var types = this.PermissionProvider.GetAllPermissions()
                .Where(x => formPermissions.Contains(x.PermissionID))
                .Select(x => x.EntityType)
                .ToList();

            var typeIds = this.StateProvider.GetAllInfo()
                .WhereIf(this.LocalAdminRoleService.IsThisUserLocalAdmin(), x => allowStatefulEntities.Contains(x.TypeId))
                .Where(x => types.Contains(x.Type))
                .Select(x => new {x.TypeId, x.Name})
                .OrderBy(x => x.Name)
                .ToList();

            return new BaseDataResult(typeIds);
        }

        private void FillGrant(bool stateful, List<RolePermissionProxy> resultPermissions, long roleId, long stateId)
        {
            List<string> existsPermissions;

            if (stateful)
            {
                existsPermissions = this.StateRolePermissionRepository.GetAll()
                    .Where(x => x.Role.Id == roleId)
                    .Where(x => x.State.Id == stateId)
                    .Select(x => x.PermissionId)
                    .ToList();
            }
            else
            {
                existsPermissions = this.RolePermissionRepository.GetAll()
                    .Where(x => x.Role.Id == roleId)
                    .Select(x => x.PermissionId)
                    .ToList();
            }

            foreach (var permission in resultPermissions)
            {
                permission.Grant = existsPermissions.Contains(permission.PermissionId);
            }
        }

        private void ResolvePermission(
            ICollection<RolePermissionProxy> permissions,
            PermissionTreeNode node,
            List<string> formPermissions,
            string idPrefix = null,
            string namePrefix = null)
        {
            string currentPermissionId = null;
            string currentName = null;
            const string securityPrefix = "B4.Security";

            if (node.IDPart.IsNotEmpty())
            {
                currentPermissionId = $"{(idPrefix == null ? "" : idPrefix + ".")}{node.IDPart}";

                if (currentPermissionId == securityPrefix)
                {
                    return;
                }

                currentName = $"{(namePrefix == null ? "" : namePrefix + "/ ")}{node.Description}";

                if (!node.IsNamespace && formPermissions.Contains(currentPermissionId))
                {
                    permissions.Add(
                        new RolePermissionProxy
                        {
                            PermissionId = currentPermissionId,
                            Path = namePrefix,
                            Name = node.Description
                        });
                }
            }

            foreach (var child in node.Children.Values)
            {
                this.ResolvePermission(permissions, child, formPermissions, currentPermissionId, currentName);
            }
        }

        /// <summary>
        /// Прокси-класс ограничения
        /// </summary>
        protected class RolePermissionProxy
        {
            /// <summary>
            /// Идентификатор
            /// </summary>
            public string PermissionId { get; set; }

            /// <summary>
            /// Путь
            /// </summary>
            public string Path { get; set; }

            /// <summary>
            /// Название
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Разрешение
            /// </summary>
            public bool Grant { get; set; }
        }
    }
}