namespace Bars.Gkh.ViewModel.Administration
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    public class RoleViewModel : BaseViewModel<Role>
    {
        public ILocalAdminRoleService LocalAdminRoleService { get; set; }
        public IDomainService<LocalAdminRoleRelations> LocalAdminRoleRelationsDomain { get; set; }
        public IGkhUserManager GkhUserManager { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<Role> domainService, BaseParams baseParams)
        {
            var userRole = this.GkhUserManager.GetActiveUser().Roles.First().Role;

            var localAdminList = this.LocalAdminRoleRelationsDomain.GetAll()
                .Select(x => x.ParentRole.Id)
                .ToList();

            if (this.LocalAdminRoleService.IsLocalAdmin(userRole.Id))
            {
                return this.LocalAdminRoleService.GetChildRoleList(userRole.Id)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        LocalAdmin = localAdminList.Contains(x.Id) ? YesNo.Yes : YesNo.No
                    })
                    .ToListDataResult(baseParams.GetLoadParam());
            }

            return domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    LocalAdmin = localAdminList.Contains(x.Id) ? YesNo.Yes : YesNo.No
                })
                .OrderBy(x => x.Name)
                .ToListDataResult(baseParams.GetLoadParam());
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<Role> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var role = domainService.Get(id);

            if (role == null)
            {
                return BaseDataResult.Error($"Не найдена роль с идентификатором '{id}'");
            }

            var childRoleList = this.LocalAdminRoleRelationsDomain.GetAll()
                .Where(x => x.ParentRole.Id == id)
                .Select(x => new
                {
                    x.ChildRole.Id,
                    x.ChildRole.Name
                })
                .ToList();

            return new BaseDataResult(new
            {
                role.Id,
                role.Name,
                LocalAdmin = childRoleList.IsNotEmpty() ? YesNo.Yes : YesNo.No,
                RoleList = childRoleList
            });
        }
    }
}