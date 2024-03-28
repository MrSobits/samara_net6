namespace Bars.Gkh.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.B4.Security;
    using Bars.B4.Utils;
    using Bars.B4.Utils.Annotations;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Контроллер настройки ограничений по статусам
    /// </summary>
    public class StatePermissionController : B4.Modules.States.Controller.StatePermissionController
    {
        public ILocalAdminRoleService LocalAdminRoleService { get; set; }
        public IFilterPermissionService FilterPermissionService { get; set; }

        /// <summary>
        /// Получить отфильтрованное дерево прав доступа по статусам
        /// </summary>
        public ActionResult GetFiltredRoleStatePermissions(long? roleId, long? stateId, string typeId)
        {
            if (roleId.ToLong() == 0 || stateId.ToLong() == 0 || string.IsNullOrEmpty(typeId))
            {
                return new JsonNetResult(new object[0]);
            }

            if (!this.LocalAdminRoleService.IsThisUserLocalAdmin())
            {
                return base.GetRoleStatePermissions(roleId, stateId, typeId);
            }

            var statePermissionRepository = this.Container.Resolve<IRepository<StateRolePermission>>();
            var treeBuilder = this.Container.Resolve<IPermissionTreeBuilder>();
            var stateProvider = this.Container.Resolve<IStateProvider>();

            using (this.Container.Using(treeBuilder, stateProvider))
            {
                var statefulEntityInfo = stateProvider.GetStatefulEntityInfo(typeId);
                if (statefulEntityInfo == null)
                {
                    return new JsonNetResult(new object[0]);
                }

                var root = treeBuilder.GetPermissionsTree(statefulEntityInfo.Type);

                var allowPermissionSet = new HashSet<string>(statePermissionRepository.GetAll()
                    .Where(x => x.Role.Id == roleId)
                    .Where(x => x.State.Id == stateId)
                    .Select(x => x.PermissionId)
                    .ToList());

                var allowPermissionNodeSet = this.FilterPermissionService.GetAllowStatePermissionNodes(roleId.Value, stateId.Value);

                return new JsonNetResult(this.LocalAdminRoleService.ConvertTreeToJObject(root, allowPermissionSet, allowPermissionNodeSet));
            }
        }

        /// <summary>
        /// Доступные для редактирования узлы дерева прав доступа
        /// </summary>
        public ActionResult GetNodePermissions(long? roleId, long? stateId, string typeId)
        {
            if (roleId.ToLong() == 0 || stateId.ToLong() == 0 || string.IsNullOrEmpty(typeId))
            {
                return new JsonNetResult(new object[0]);
            }

            var treeBuilder = this.Container.Resolve<IPermissionTreeBuilder>();
            var stateProvider = this.Container.Resolve<IStateProvider>();

            using (this.Container.Using(treeBuilder, stateProvider))
            {
                var statefulEntityInfo = stateProvider.GetStatefulEntityInfo(typeId);
                if (statefulEntityInfo == null)
                {
                    return new JsonNetResult(new object[0]);
                }

                var root = treeBuilder.GetPermissionsTree(statefulEntityInfo.Type);

                var allowPermissionSet = this.FilterPermissionService.GetAllowStatePermissionNodes(roleId.Value, stateId.Value);

                return new JsonNetResult(this.LocalAdminRoleService.ConvertTreeToJObject(root, allowPermissionSet));
            }
        }

        /// <summary>
        /// Обновить права на доступные для редактирования узлы дерева прав доступа
        /// </summary>
        [ActionPermission("B4.States.StatePermission.Edit")]
        public ActionResult UpdateNodePermissions(long roleId, long stateId, string permissions)
        {
            var roleRepository = this.Container.Resolve<IRepository<Role>>();
            var stateRepository = this.Container.Resolve<IRepository<State>>();
            var statePermissionRepository = this.Container.Resolve<IRepository<LocalAdminStatePermission>>();

            using (this.Container.Using(roleRepository, statePermissionRepository))
            {
                this.Container.InTransaction(() =>
                {
                    var data = JsonNetConvert.DeserializeObject<Dictionary<string, bool>>(this.Container, permissions);
                    var role = roleRepository.Load(roleId);
                    var state = stateRepository.Load(stateId);

                    var dicPermissions = statePermissionRepository.GetAll()
                        .Where(x => x.Role.Id == roleId)
                        .Where(x => x.State.Id == stateId)
                        .ToDictionary(x => x.PermissionId);

                    foreach (var pair in data)
                    {
                        if (pair.Value)
                        {
                            if (!dicPermissions.ContainsKey(pair.Key))
                            {
                                statePermissionRepository.Save(new LocalAdminStatePermission
                                {
                                    Role = role,
                                    State = state,
                                    PermissionId = pair.Key
                                });
                            }
                        }
                        else
                        {
                            if (dicPermissions.ContainsKey(pair.Key))
                            {
                                statePermissionRepository.Delete(dicPermissions[pair.Key].Id);
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
        [ActionPermission("B4.States.StatePermission.Edit")]
        public ActionResult FiltredCopyRolePermission(BaseParams baseParams)
        {
            var fromRoleId = baseParams.Params.GetAsId("fromRoleId");
            var toRoleId = baseParams.Params.GetAsId("toRoleId");
            var fromStateId = baseParams.Params.GetAsId("fromStateId");
            var toStateIdStr = baseParams.Params.GetAs<string>("toStateId");
            var toAllStates = toStateIdStr.Contains("All");
            var toStateIds = toAllStates ? new long[0] : toStateIdStr.ToLongArray();
            var fromTypeId = baseParams.Params.GetAs<string>("fromTypeId");

            if (fromRoleId == 0 || toRoleId == 0 || fromStateId == 0 || (toStateIds.Length == 0 && !toAllStates))
            {
                return this.JsFailure("Не верно указан один из параметров");
            }

            var roleRepository = this.Container.Resolve<IRepository<Role>>();
            var stateRepository = this.Container.Resolve<IRepository<State>>();
            var stateRolePermissionRepository = this.Container.Resolve<IRepository<StateRolePermission>>();
            var stateProvider = this.Container.Resolve<IStateProvider>();

            using (this.Container.Using(roleRepository, stateRepository, stateProvider, stateRolePermissionRepository))
            {
                this.Container.InTransaction(() =>
                {
                    var stateRoleQuery = stateRolePermissionRepository.GetAll()
                        .Where(x => x.Role.Id == fromRoleId)
                        .Where(x => x.State.Id == fromStateId);

                    var role = roleRepository.Get(toRoleId);
                    if (this.LocalAdminRoleService.IsThisUserLocalAdmin())
                    {
                        var statefulEntityInfo = stateProvider.GetStatefulEntityInfo(fromTypeId);

                        ArgumentChecker.NotNull(statefulEntityInfo, nameof(statefulEntityInfo));

                        var isAllow = this.FilterPermissionService.GetAllowStatefulEntities(toRoleId)
                            .Contains(fromTypeId);

                        if (!isAllow)
                        {
                            throw new Exception($"Нет доступа для редактирования '{statefulEntityInfo.Name}' у роли '{role.Name}'");
                        }
                    }

                    var statesQuery = stateRepository.GetAll()
                        .WhereIf(fromRoleId == toRoleId, x => x.Id != fromStateId);

                    statesQuery = toAllStates
                        ? statesQuery.Where(x => x.TypeId == fromTypeId)
                        : statesQuery.Where(x => toStateIds.Contains(x.Id));

                    var stateIds = statesQuery.Select(x => x.Id).ToArray();

                    var toPermissions = stateRolePermissionRepository.GetAll()
                        .Where(x => x.Role.Id == toRoleId)
                        .Where(x => stateIds.Contains(x.State.Id))
                        .Select(x => new
                        {
                            StateId = x.State.Id,
                            x.PermissionId
                        })
                        .AsEnumerable()
                        .GroupBy(x => $"{x.StateId}#{x.PermissionId}")
                        .Select(x => x.Key)
                        .ToHashSet();

                    stateRoleQuery
                        .Select(x => x.PermissionId)
                        .AsEnumerable()
                        .ForEach(x =>
                        {
                            foreach (var stateId in stateIds)
                            {
                                if (!toPermissions.Contains($"{stateId}#{x}"))
                                {
                                    stateRolePermissionRepository.Save(new StateRolePermission
                                        {
                                            Role = role,
                                            PermissionId = x,
                                            State = new State { Id = stateId}
                                        });
                                }
                            }
                        });
                });
            }

            return JsonNetResult.Success;
        }

        /// <summary>
        /// Копирование прав доступа на узлы дерева на основе фильтрации для локальных администраторов
        /// </summary>
        [ActionPermission("B4.States.StatePermission.Edit")]
        public ActionResult CopyNodePermission(BaseParams baseParams)
        {
            var fromRoleId = baseParams.Params.GetAsId("fromRoleId");
            var toRoleId = baseParams.Params.GetAsId("toRoleId");
            var fromStateId = baseParams.Params.GetAsId("fromStateId");
            var toStateId = baseParams.Params.GetAsId("toStateId");

            if (fromRoleId == 0 || toRoleId == 0 || fromStateId == 0 || toStateId == 0)
            {
                return this.JsFailure("Не верно указан один из параметров");
            }

            var roleRepository = this.Container.Resolve<IRepository<Role>>();
            var stateRepository = this.Container.Resolve<IRepository<State>>();
            var stateRolePermissionRepository = this.Container.Resolve<IRepository<LocalAdminStatePermission>>();
            var stateProvider = this.Container.Resolve<IStateProvider>();

            using (this.Container.Using(roleRepository, stateRepository, stateProvider, stateRolePermissionRepository))
            {
                this.Container.InTransaction(() =>
                {
                    var stateRoleQuery = stateRolePermissionRepository.GetAll()
                        .Where(x => x.Role.Id == fromRoleId)
                        .Where(x => x.State.Id == fromStateId);

                    var role = roleRepository.Get(toRoleId);
                    var state = stateRepository.Get(toStateId);

                    var toPermissions = stateRolePermissionRepository.GetAll()
                        .Where(x => x.Role.Id == toRoleId)
                        .Where(x => x.State.Id == toStateId)
                        .Select(x => x.PermissionId)
                        .ToList();

                    stateRoleQuery
                        .Select(x => x.PermissionId)
                        .AsEnumerable()
                        .Except(toPermissions)
                        .ForEach(x =>
                        {
                            stateRolePermissionRepository.Save(new LocalAdminStatePermission
                            {
                                Role = role,
                                PermissionId = x,
                                State = state
                            });
                        });
                });
            }

            return JsonNetResult.Success;
        }
    }
}
