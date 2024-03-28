namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Utils;

    using NHibernate.Util;

    /// <summary>
    /// Дать права доступа локальным администраторам к системным разделам
    /// </summary>
    public class GrantForLocalAdminAction : BaseExecutionAction
    {
        /// <inheritdoc />
        public override string Name => "Права доступа для локальных администраторов";

        /// <inheritdoc />
        public override Func<IDataResult> Action => this.Execute;

        /// <inheritdoc />
        public override string Description => "Действие открывает доступ к системным разделам всем локальным администраторам";

        /// <inheritdoc />
        private BaseDataResult Execute()
        {
            var localAdminRoleService = this.Container.Resolve<ILocalAdminRoleService>();
            var allowPermissionList = new []
            {
                "Administration.Operator.Create",
                "Administration.Operator.Delete",
                "Administration.Operator.Edit",
                "Administration.Operator.View",
                "B4.Security.AccessRights",
                "B4.States.StateTransfer.Create",
                "B4.States.StateTransfer.Delete",
                "B4.States.StateTransfer.Edit",
                "B4.States.StateTransfer.View",
                "B4.States.StateTransferRule.Create",
                "B4.States.StateTransferRule.Delete",
                "B4.States.StateTransferRule.Edit",
                "B4.States.StateTransferRule.View",
                "B4.States.StatePermission.Create",
                "B4.States.StatePermission.Delete",
                "B4.States.StatePermission.Edit",
                "B4.States.StatePermission.View"
            };

            var denyPermissionList = new[]
            {
                "B4.Security.Role",
                "B4.Security.LocalAdminRoleSettings",
                "B4.Security.FieldRequirement",
                "B4.States.State.Create",
                "B4.States.State.Delete",
                "B4.States.State.Edit",
                "B4.States.State.View"
            };

            var sb = new StringBuilder();

            using (this.Container.Using(localAdminRoleService))
            {
                var roleRepository = this.Container.Resolve<IRepository<Role>>();
                var rolePermissionRepository = this.Container.Resolve<IRepository<RolePermission>>();

                using (this.Container.Using(roleRepository, rolePermissionRepository))
                {
                    this.Container.InTransaction(() =>
                    {
                        foreach (var localAdminRole in localAdminRoleService.GetAll())
                        {
                            var existsPermissions = rolePermissionRepository.GetAll()
                                .Where(x => x.Role.Id == localAdminRole.Id)
                                .WhereContainsBulked(x => x.PermissionId, allowPermissionList)
                                .Select(x => x.PermissionId)
                                .ToList();

                            allowPermissionList
                                .Except(existsPermissions)
                                .ForEach(x =>
                                {
                                    sb.AppendLine($"Добавлено: {localAdminRole.Name} - {x}");
                                    rolePermissionRepository.Save(new RolePermission
                                    {
                                        Role = localAdminRole,
                                        PermissionId = x
                                    });
                                });

                            rolePermissionRepository.GetAll()
                                .Where(x => x.Role.Id == localAdminRole.Id)
                                .WhereContainsBulked(x => x.PermissionId, denyPermissionList)
                                .ForEach(x =>
                                {
                                    sb.AppendLine($"Удалено: {localAdminRole.Name} - {x.PermissionId}");
                                    rolePermissionRepository.Delete(x.Id);
                                });
                        }
                    });
                }
            }

            return new BaseDataResult(sb.ToString());
        }
    }
}