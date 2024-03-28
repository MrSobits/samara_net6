namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;

    using B4;
    using B4.Modules.Security;

    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Utils;

    using Entities;

    /// <summary>
    /// Болванка потому что от этого класса могли наследоваться
    /// </summary>
    public class OperatorViewModel : OperatorViewModel<Operator>
    {
    }

    /// <summary>
    /// Generic класс для лучшего расширения в регионных модулях
    /// </summary>
    /// <typeparam name="T"> Тип расширения оператора </typeparam>
    public class OperatorViewModel<T> : BaseViewModel<T>
        where T : Operator
    {
        public ILocalAdminRoleService LocalAdminRoleService { get; set; }
        public IDomainService<UserRole> UserRoleDomain { get; set; }
        public IDomainService<OperatorContragent> OperatorContragentDomain { get; set; }
        public IDomainService<OperatorMunicipality> OperatorMunicipalityDomain { get; set; }
        public IGkhUserManager GkhUserManager { get; set; }

        /// <inheritdoc />
        public override IDataResult List(IDomainService<T> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            //Справочник ролей
            var userRoleQuery = this.UserRoleDomain.GetAll()
                .Select(x => new
                {
                    x.User.Id,
                    RoleId = x.Role.Id,
                    RoleName = x.Role.Name
                });

            if (this.LocalAdminRoleService.IsThisUserLocalAdmin())
            {
                var userRole = this.GkhUserManager.GetActiveOperatorRoles().First();
                var childRoleList = this.LocalAdminRoleService.GetChildRoleList(userRole.Id)
                    .Select(x => x.Id)
                    .ToList();

                userRoleQuery = userRoleQuery.WhereContainsBulked(x => x.RoleId, childRoleList);
            }

            var userRoles = userRoleQuery
                .AsEnumerable()
                .GroupBy(x => x.Id, x => x.RoleName)
                .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            //Справочник контрагентов
            var contragentQuery = OperatorContragentDomain.GetAll()
                .Select(x => new
                {
                    x.Operator.Id,
                    x.Contragent.Name,
                    x.Contragent.Inn,
                    Municipalities = GetOperatorMunicipalities(x.Operator)
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.FirstOrDefault());

            return domain.GetAll()
                .WhereContainsBulked(x => x.User.Id, userRoles.Keys)
                .Select(x => new
                {
                    x.Id,
                    UserId = x.User.Id,
                    x.User.Name,
                    x.User.Login,
                    x.Phone,
                    x.IsActive,
                    x.ExportFormat
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.Login,
                    Role = userRoles[x.UserId],
                    Phone = x.Phone ?? string.Empty,
                    Municipality = contragentQuery.ContainsKey(x.Id) ? contragentQuery[x.Id].Municipalities : "",
                    Inn = contragentQuery.ContainsKey(x.Id) ? contragentQuery[x.Id].Inn : "",
                    ContragentName = contragentQuery.ContainsKey(x.Id) ? contragentQuery[x.Id].Name : "",
                    x.IsActive,
                    x.ExportFormat
                })
                .ToListDataResult(loadParams);
        }

        /// <inheritdoc />
        public override IDataResult Get(IDomainService<T> domain, BaseParams baseParams)
        {
            var userRolesService = this.Container.Resolve<IDomainService<UserRole>>();
            try
            {
                var id = baseParams.Params.GetAsId();
                var obj = domain.Get(id);

                return obj != null ? new BaseDataResult(
                    new
                        {
                            obj.Id,
                            obj.User.Name,
                            obj.User.Login,
                            Password = string.Empty,
                            NewPassword = string.Empty,
                            Role = userRolesService.GetAll().Where(x => x.User.Id == obj.User.Id).Select(x => x.Role).FirstOrDefault(),
                            obj.User.Email,
                            obj.TypeWorkplace,
                            obj.Phone,
                            obj.IsActive,
                            obj.Inspector,
                            Contragent = obj.Contragent != null ? new { obj.Contragent.Id, obj.Contragent.Name } : null,
                            GisGkhContragent = obj.GisGkhContragent != null ? new { obj.GisGkhContragent.Id, obj.GisGkhContragent.Name } : null,
                            obj.ContragentType,
                            obj.RisToken,
                            obj.ExportFormat,
                            obj.MobileApplicationAccessEnabled,
                            obj.UserPhoto
                        }) : new BaseDataResult();
            }
            finally
            {
                this.Container.Release(userRolesService);
            }
        }

        private string GetOperatorMunicipalities(Operator @operator)
        {
            return String.Join(", ", OperatorMunicipalityDomain.GetAll().Where(x => x.Operator.Id == @operator.Id).Select(x => x.Municipality.Name));
        }
    }
}