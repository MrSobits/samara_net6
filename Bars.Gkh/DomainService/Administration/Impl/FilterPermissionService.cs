namespace Bars.Gkh.DomainService.Administration.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class FilterPermissionService : IFilterPermissionService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public ISet<string> GetAllowRolePermissionNodes(long roleId)
        {
            var localAdminRolePermissionRepository = this.Container.Resolve<IRepository<LocalAdminRolePermission>>();

            using (this.Container.Using(localAdminRolePermissionRepository))
            {
                return new HashSet<string>(localAdminRolePermissionRepository.GetAll()
                    .Where(x => x.Role.Id == roleId)
                    .Select(x => x.PermissionId)
                    .ToArray());
            }
        }

        /// <inheritdoc />
        public ISet<string> GetAllowStatePermissionNodes(long roleId, long stateId)
        {
            var localAdminStatePermissionRepository = this.Container.Resolve<IRepository<LocalAdminStatePermission>>();

            using (this.Container.Using(localAdminStatePermissionRepository))
            {
                return new HashSet<string>(localAdminStatePermissionRepository.GetAll()
                    .Where(x => x.Role.Id == roleId)
                    .Where(x => x.State.Id == stateId)
                    .Select(x => x.PermissionId)
                    .ToArray());
            }
        }

        /// <inheritdoc />
        public ISet<string> GetAllowStatefulEntities(long roleId)
        {
            var localAdminStatefulEntityRepository = this.Container.Resolve<IRepository<LocalAdminStatefulEntity>>();

            using (this.Container.Using(localAdminStatefulEntityRepository))
            {
                return new HashSet<string>(localAdminStatefulEntityRepository.GetAll()
                    .Where(x => x.Role.Id == roleId)
                    .Select(x => x.TypeId)
                    .ToArray());
            }
        }
    }
}