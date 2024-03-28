namespace Bars.Gkh.Gasu.DomainService
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Gasu.Enums;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class GasuIndicatorService : IGasuIndicatorService
    {

        public IWindsorContainer Container { get; set; }

        public List<EbirModule> GetAvailableModulesEbir()
        {
            var userManager = Container.Resolve<IGkhUserManager>();
            var rolePermissionDomain = Container.ResolveDomain<RolePermission>();
            var userRoleDomain = Container.ResolveDomain<UserRole>();

            var result = new List<EbirModule>();
            try
            {
                var user = userManager.GetActiveUser();

                var roleIds = userRoleDomain.GetAll().Where(x => x.User.Id == user.Id).Select(x => x.Role.Id).ToList();

                var permissions = rolePermissionDomain.GetAll()
                    .Where(x => roleIds.Contains(x.Role.Id))
                    .Where(x => x.PermissionId.Contains("Administration.ExportData.ModuleEbir"))
                    .Select(x => x.PermissionId)
                    .ToList();

                foreach (EbirModule ebirModule in Enum.GetValues(typeof (EbirModule)))
                {
                    if (permissions.Any(x => x.Contains(ebirModule.GetEnumMeta().Description)))
                    {
                        result.Add(ebirModule);
                    }
                }
            }
            finally
            {
                Container.Release(userManager);
                Container.Release(rolePermissionDomain);
                Container.Release(userRoleDomain);
            }

            return result;
        }
    }
}
