namespace Bars.Gkh.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Контроллер правил перехода по статусам
    /// </summary>
    public class StateTransferRuleController : B4.Modules.States.Controller.StateTransferRuleController
    {
        public ILocalAdminRoleService LocalAdminRoleService { get; set; }
        public IDomainService<LocalAdminRoleRelations> LocalAdminRoleRelationsDomain { get; set; }
        public IDomainService<LocalAdminStatefulEntity> LocalAdminStatefulEntityDomain { get; set; }
        public IGkhUserManager GkhUserManager { get; set; }

        /// <inheritdoc />
        public override ActionResult List(StoreLoadParams baseParams)
        {
            var query = this.DomainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.RuleId,
                    x.StateTransfer.TypeId,
                    NewState = x.StateTransfer.NewState.Name,
                    CurrentState = x.StateTransfer.CurrentState.Name,
                    RoleName = x.StateTransfer.Role.Name,
                    RoleId = x.StateTransfer.Role.Id
                });

            if (this.LocalAdminRoleService.IsThisUserLocalAdmin())
            {
                var userRole = this.GkhUserManager.GetActiveOperatorRoles().First();
                var childRoleList = this.LocalAdminRoleService.GetChildRoleList(userRole.Id)
                    .Select(x => x.Id)
                    .ToList();

                var allowTypeList = this.LocalAdminStatefulEntityDomain.GetAll()
                    .Where(x => childRoleList.Contains(x.Role.Id))
                    .Select(x => x.TypeId)
                    .ToList();

                query = query.Where(x => childRoleList.Contains(x.RoleId))
                    .Where(x => allowTypeList.Contains(x.TypeId));
            }

            var ruleDict = this.Container.ResolveAll<IRuleChangeStatus>()
                .ToDictionary(x => x.Id, x => new
                {
                    x.Name,
                    x.Description
                });

            var typeDict = this.Container.Resolve<IStateProvider>().GetAllInfo()
                .ToDictionary(x => x.TypeId, x => x.Name);

            return query
                .WhereContainsBulked(x => x.RuleId, ruleDict.Keys)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    RuleName = ruleDict.Get(x.RuleId)?.Name,
                    RuleDescription = ruleDict.Get(x.RuleId)?.Description,
                    x.TypeId,
                    TypeName = typeDict.Get(x.TypeId),
                    x.NewState,
                    x.CurrentState,
                    x.RoleName
                })
                .ToListDataResult(baseParams)
                .ToJsonResult();
        }

        /// <inheritdoc />
        public override ActionResult Get(long id)
        {
            if (this.LocalAdminRoleService.IsThisUserLocalAdmin())
            {
                var userRole = this.GkhUserManager.GetActiveOperatorRoles().First();
                var childRoleList = this.LocalAdminRoleService.GetChildRoleList(userRole.Id)
                    .Select(x => x.Id)
                    .ToList();

                var allowTypeList = this.LocalAdminStatefulEntityDomain.GetAll()
                    .Where(x => childRoleList.Contains(x.Role.Id))
                    .Select(x => x.TypeId)
                    .ToList();

                var isAllow = this.DomainService.GetAll()
                    .Where(x => childRoleList.Contains(x.StateTransfer.Role.Id))
                    .Any(x => allowTypeList.Contains(x.StateTransfer.TypeId));

                if (!isAllow)
                {
                    throw new AuthorizationException("У текущей роли нет доступа");
                }
            }

            var types = this.Container.Resolve<IStateProvider>().GetAllInfo();

            var value = this.DomainService.Load(id);
            var typeId = value.StateTransfer.TypeId;
            var typeName = types.Where(x => x.TypeId == typeId).Select(x => x.Name).FirstOrDefault();

            var ruleChangeStatus = this.Container.ResolveAll<IRuleChangeStatus>().FirstOrDefault(x => x.Id == value.RuleId);

            if (ruleChangeStatus == null)
            {
                throw new Exception("Правило ненайдено:" + value.RuleId);
            }

            var name = $"{value.StateTransfer.CurrentState.Name} -> {value.StateTransfer.NewState.Name}";

            return new JsonGetResult(new
            {
                value.Id,
                value.StateTransfer.Role,
                Object = new { TypeId = typeId, Name = typeName },
                StateTransfer = new { value.StateTransfer.Id, Name = name, TypeId = typeId },
                RuleId = new {ruleChangeStatus.Id, ruleChangeStatus.Name, ruleChangeStatus.TypeId, ruleChangeStatus.Description}
            });
        }
    }
}
