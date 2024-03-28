namespace Bars.Gkh.Regions.Tatarstan.DomainService.Impl
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;

    using System.Linq;

    using Bars.Gkh.Authentification;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    using Castle.Windsor;

    public class RoleTypeHousePermissionService : IRoleTypeHousePermissionService
    {
        public IDomainService<RoleTypeHousePermission> RoleTypeHousePermissionDomainService { get; set; }

        public IRepository<Role> RoleRepository { get; set; }

        public IWindsorContainer Container { get; set; }

        public IDataResult UpdatePermissions(BaseParams baseParams)
        {
            var roleId = baseParams.Params.GetAs<long>("roleId");

            if (roleId == 0)
            {
                return new BaseDataResult(false);
            }

            var permissionList = baseParams.Params.GetAs<List<DynamicDictionary>>("permissions");

            var permissions = permissionList
                .Select(x => new { TypeHouse = x.GetAs<TypeHouse>("Code"), Allowed = x.GetAs("Allowed", false) })
                .ToList();

            var allowed = RoleTypeHousePermissionDomainService.GetAll()
                .Where(x => x.Role.Id == roleId)
                .Select(x => new { x.Id, x.TypeHouse })
                .ToDictionary(x => x.TypeHouse, x => x.Id);
            
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var roleTypeHousePermission in permissions)
                    {
                        if (roleTypeHousePermission.Allowed)
                        {
                            if (!allowed.ContainsKey(roleTypeHousePermission.TypeHouse))
                            {
                                var newRecord = new RoleTypeHousePermission
                                {
                                    Role = RoleRepository.Load(roleId),
                                    TypeHouse =
                                        roleTypeHousePermission.TypeHouse
                                };

                                RoleTypeHousePermissionDomainService.Save(newRecord);
                            }
                        }
                        else
                        {
                            if (allowed.ContainsKey(roleTypeHousePermission.TypeHouse))
                            {
                                RoleTypeHousePermissionDomainService.Delete(allowed[roleTypeHousePermission.TypeHouse]);
                            }
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                        return new BaseDataResult(false, exc.Message);
                    }
                    catch (TransactionRollbackException ex)
                    {
                        return new BaseDataResult(false, ex.Message);
                    }
                    catch
                    {
                        return new BaseDataResult(false, "Произошла неизвестная ошибка при откате транзакции");
                    }
                }
            }

            return new BaseDataResult();
        }

        public IDataResult GetRoleTypeHouses(BaseParams baseParams)
        {
            var gkhOperator = Container.Resolve<IGkhUserManager>().GetActiveOperator();

            var roles = new List<long>();

            if (gkhOperator == null || gkhOperator.User == null)
            {
                var user = Container.Resolve<IUserIdentity>();
                if (user != null && user.Name == "admin_tat")
                {
                    roles = RoleRepository.GetAll().Where(x => x.Name == "Администратор").Select(x => x.Id).ToList();
                }
                else
                {
                    return new ListDataResult();
                }
            }
            else
            {
                roles = gkhOperator.User.Roles.Select(x => x.Role.Id).ToList();
            }
            
            var allowed = RoleTypeHousePermissionDomainService.GetAll()
                .Where(x => roles.Contains(x.Role.Id))
                .Select(x => x.TypeHouse)
                .AsEnumerable()
                .Distinct()
                .Select(x => new { Value = x, Name = x.GetEnumMeta().Display })
                .ToArray();

            return new ListDataResult(allowed, allowed.Count());
        }
    }
}