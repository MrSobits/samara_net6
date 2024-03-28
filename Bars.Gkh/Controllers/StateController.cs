namespace Bars.Gkh.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    using Newtonsoft.Json.Linq;

    /// <summary>
    /// Контроллер статусных сущностей
    /// </summary>
    public class StateController : B4.Modules.States.Controller.StateController
    {
        public ILocalAdminRoleService LocalAdminRoleService { get; set; }
        public IFilterPermissionService FilterPermissionService { get; set; }

        /// <summary>
        /// Получить отфильтрованное дерево доступных статусных сущностей
        /// </summary>
        public ActionResult GetObjectPermissions(BaseParams baseParams)
        {
            var roleId = baseParams.Params.GetAsId("roleId");

            if (roleId == 0)
            {
                return new JsonNetResult(new object[0]);
            }

            var stateProvider = this.Container.Resolve<IStateProvider>();

            using (this.Container.Using(stateProvider))
            {
                var allowTypeList = this.FilterPermissionService.GetAllowStatefulEntities(roleId);

                var result = new JArray();

                foreach (var statefulEntityInfo in stateProvider.GetAllInfo().OrderBy(x => x.Name))
                {
                    var jnode = new JObject
                    {
                        ["id"] = statefulEntityInfo.TypeId,
                        ["order"] = 1,
                        ["text"] = statefulEntityInfo.Name,
                        ["leaf"] = true,
                        ["checked"] = allowTypeList.Contains(statefulEntityInfo.TypeId),
                    };

                    result.Add(jnode);
                }

                return new JsonNetResult(result);
            }
        }

        /// <summary>
        /// Отфильтрованный список доступных статусных сущностей
        /// </summary>
        public ActionResult FiltredStatefulEntityList(BaseParams baseParams)
        {
            var stateProvider = this.Container.Resolve<IStateProvider>();
            try
            {
                if (!this.LocalAdminRoleService.IsThisUserLocalAdmin())
                {
                    return stateProvider.GetAllInfo().OrderBy(x => x.Name)
                        .Select(x => new {x.TypeId, x.Name})
                        .ToListDataResult(baseParams.GetLoadParam())
                        .ToJsonResult();
                }

                var roleId = baseParams.Params.GetAsId("roleId");

                var allowTypeList = this.FilterPermissionService.GetAllowStatefulEntities(roleId);

                return stateProvider.GetAllInfo()
                    .Where(x => allowTypeList.Contains(x.TypeId))
                    .Select(x => new {x.TypeId, x.Name})
                    .ToListDataResult(baseParams.GetLoadParam())
                    .ToJsonResult();
            }
            finally
            {
                this.Container.Release(stateProvider);
            }
        }

        /// <summary>
        /// Обновить права
        /// </summary>
        [ActionPermission("B4.States.StatePermission.Edit")]
        public ActionResult UpdateObjectPermissions(BaseParams baseParams)
        {
            var roleId = baseParams.Params.GetAsId("roleId");
            var permissions = baseParams.Params.GetAs<DynamicDictionary>("permissions");

            var roleRepository = this.Container.Resolve<IRepository<Role>>();
            var statefulEntityRepository = this.Container.Resolve<IRepository<LocalAdminStatefulEntity>>();
            using (this.Container.Using(roleRepository, statefulEntityRepository))
            {
                this.Container.InTransaction(() =>
                {
                    var role = roleRepository.Load(roleId);

                    var dicPermissions = statefulEntityRepository.GetAll()
                        .Where(x => x.Role.Id == roleId)
                        .ToDictionary(x => x.TypeId);

                    foreach (var pair in permissions)
                    {
                        if (pair.Value.ToBool())
                        {
                            if (!dicPermissions.ContainsKey(pair.Key))
                            {
                                statefulEntityRepository.Save(new LocalAdminStatefulEntity
                                {
                                    Role = role,
                                    TypeId = pair.Key
                                });
                            }
                        }
                        else
                        {
                            if (dicPermissions.ContainsKey(pair.Key))
                            {
                                statefulEntityRepository.Delete(dicPermissions[pair.Key].Id);
                            }
                        }
                    }
                });
            }

            return JsonNetResult.Success;
        }

        /// <summary>
        /// Копировать права
        /// </summary>
        [ActionPermission("B4.States.StatePermission.Edit")]
        public ActionResult CopyPermissions(BaseParams baseParams)
        {
            var fromRoleId = baseParams.Params.GetAsId("fromRoleId");
            var toRoleId = baseParams.Params.GetAsId("toRoleId");

            var roleRepository = this.Container.Resolve<IRepository<Role>>();
            var statefulEntityRepository = this.Container.Resolve<IRepository<LocalAdminStatefulEntity>>();

            using (this.Container.Using(roleRepository, statefulEntityRepository))
            {
                this.Container.InTransaction(() =>
                {
                    var role = roleRepository.Get(toRoleId);

                    var toPermissions = statefulEntityRepository.GetAll()
                        .Where(x => x.Role.Id == toRoleId)
                        .Select(x => x.TypeId)
                        .ToList();

                    statefulEntityRepository.GetAll()
                        .Where(x => x.Role.Id == fromRoleId)
                        .Select(x => x.TypeId)
                        .AsEnumerable()
                        .Except(toPermissions)
                        .ForEach(x =>
                        {
                            statefulEntityRepository.Save(new LocalAdminStatefulEntity { Role = role, TypeId = x });
                        });
                });
            }

            return JsonNetResult.Success;
        }
    }
}
