namespace Bars.Gkh.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security;
    using Bars.B4.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Контроллер настройки ограничений
    /// </summary>
    public class GkhPermissionController : B4.Modules.Security.PermissionController
    {
        public ILocalAdminRoleService LocalAdminRoleService { get; set; }
        public IFilterPermissionService FilterPermissionService { get; set; }

        /// <summary>
        /// Получить отфильтрованное дерево прав доступа
        /// </summary>
        [ActionPermission("B4.Security.AccessRights")]
        public ActionResult GetFiltredRolePermissions(long? roleId)
        {
            if (roleId.ToLong() == 0)
            {
                return new JsonNetResult(new object[0]);
            }

            if (!this.LocalAdminRoleService.IsThisUserLocalAdmin())
            {
                return base.GetRolePermissions(roleId);
            }

            var rolePermissionRepository = this.Container.Resolve<IRepository<RolePermission>>();
            var root = this.Container.Resolve<PermissionTreeNode>();

            using (this.Container.Using(rolePermissionRepository, root))
            {
                var allowPermissionSet = new HashSet<string>(rolePermissionRepository.GetAll()
                    .Where(x => x.Role.Id == roleId)
                    .Select(x => x.PermissionId)
                    .ToList());

                var allowPermissionNodeSet = this.FilterPermissionService.GetAllowRolePermissionNodes(roleId.Value);

                return new JsonNetResult(this.LocalAdminRoleService.ConvertTreeToJObject(root, allowPermissionSet, allowPermissionNodeSet));
            }
        }

        /// <summary>
        /// Доступные для редактирования узлы дерева прав доступа
        /// </summary>
        [ActionPermission("B4.Security.AccessRights")]
        public ActionResult GetNodePermissions(long? roleId)
        {
            if (roleId.ToLong() == 0)
            {
                return new JsonNetResult(new object[0]);
            }

            var root = this.Container.Resolve<PermissionTreeNode>();

            using (this.Container.Using(root))
            {
                var allowPermissionSet = this.FilterPermissionService.GetAllowRolePermissionNodes(roleId.Value);

                return new JsonNetResult(this.LocalAdminRoleService.ConvertTreeToJObject(root, allowPermissionSet));
            }
        }

        /// <summary>
        /// Обновить права на доступные для редактирования узлы дерева прав доступа
        /// </summary>
        [ActionPermission("B4.Security.AccessRights")]
        public ActionResult UpdateNodePermissions(long roleId, string permissions)
        {
            var roleRepository = this.Container.Resolve<IRepository<Role>>();
            var rolePermissionRepository = this.Container.Resolve<IRepository<LocalAdminRolePermission>>();

            using (this.Container.Using(roleRepository, rolePermissionRepository))
            {
                this.Container.InTransaction(() =>
                {
                    var data = JsonNetConvert.DeserializeObject<Dictionary<string, bool>>(this.Container, permissions);
                    var role = roleRepository.Load(roleId);

                    var dicPermissions = rolePermissionRepository.GetAll()
                        .Where(x => x.Role.Id == roleId)
                        .ToDictionary(x => x.PermissionId);

                    foreach (var pair in data)
                    {
                        if (pair.Value)
                        {
                            if (!dicPermissions.ContainsKey(pair.Key))
                            {
                                rolePermissionRepository.Save(new LocalAdminRolePermission
                                {
                                    Role = role,
                                    PermissionId = pair.Key
                                });
                            }
                        }
                        else
                        {
                            if (dicPermissions.ContainsKey(pair.Key))
                            {
                                rolePermissionRepository.Delete(dicPermissions[pair.Key].Id);
                            }
                        }
                    }
                });
            }

            return JsonNetResult.Success;
        }

        /// <summary>
        /// Копирование прав доступа на основе фильтрации для локальных администраторов
        /// </summary>
        [ActionPermission("B4.Security.AccessRights")]
        public ActionResult FiltredCopyRolePermission(long fromRoleId, long toRoleId)
        {
            if (!this.LocalAdminRoleService.IsThisUserLocalAdmin())
            {
                return base.CopyRolePermission(fromRoleId, toRoleId);
            }

            var roleRepository = this.Container.Resolve<IRepository<Role>>();
            var rolePermissionRepository = this.Container.Resolve<IRepository<RolePermission>>();

            using (this.Container.Using(roleRepository, rolePermissionRepository))
            {
                this.Container.InTransaction(() =>
                {
                    var role = roleRepository.Get(toRoleId);

                    var allowedNodes = this.FilterPermissionService.GetAllowRolePermissionNodes(toRoleId);

                    var toPermissions = rolePermissionRepository.GetAll()
                        .Where(x => x.Role.Id == toRoleId)
                        .Select(x => x.PermissionId)
                        .ToList();

                    rolePermissionRepository.GetAll()
                        .Where(x => x.Role.Id == fromRoleId)
                        .WhereContainsBulked(x => x.PermissionId, allowedNodes)
                        .Select(x => x.PermissionId)
                        .AsEnumerable()
                        .Except(toPermissions)
                        .ForEach(x =>
                        {
                            rolePermissionRepository.Save(new RolePermission { Role = role, PermissionId = x });
                        });
                });
            }

            return JsonNetResult.Success;
        }

        /// <summary>
        /// Копирование прав доступа на узлы дерева на основе фильтрации для локальных администраторов
        /// </summary>
        [ActionPermission("B4.Security.AccessRights")]
        public ActionResult CopyNodePermission(long fromRoleId, long toRoleId)
        {
            var roleRepository = this.Container.Resolve<IRepository<Role>>();
            var rolePermissionRepository = this.Container.Resolve<IRepository<LocalAdminRolePermission>>();

            using (this.Container.Using(roleRepository, rolePermissionRepository))
            {
                this.Container.InTransaction(() =>
                {
                    var role = roleRepository.Get(toRoleId);

                    var toPermissions = rolePermissionRepository.GetAll()
                        .Where(x => x.Role.Id == toRoleId)
                        .Select(x => x.PermissionId)
                        .ToList();

                    rolePermissionRepository.GetAll()
                        .Where(x => x.Role.Id == fromRoleId)
                        .Select(x => x.PermissionId)
                        .AsEnumerable()
                        .Except(toPermissions)
                        .ForEach(x =>
                        {
                            rolePermissionRepository.Save(new LocalAdminRolePermission { Role = role, PermissionId = x });
                        });
                });
            }

            return JsonNetResult.Success;
        }
    }
}
