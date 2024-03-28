using Bars.B4;
using Bars.B4.Modules.Security;
using Bars.B4.Utils;
using Bars.Gkh.InspectorMobile.Entities;
using Bars.Gkh.Utils;
using Castle.Windsor;
using System.Linq;

namespace Bars.Gkh.InspectorMobile.Services.Impl
{
    // <inheritdoc />
    public class MpRoleService : IMpRoleService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-мервис ролей мобильного оператора
        /// </summary>
        public IDomainService<MpRole> MpRoleDomain { get; set; }

        // <inheritdoc />
        public IDataResult AddRoles(BaseParams baseParams)
        {
            var roleIds = baseParams.Params.GetAs<long[]>("roleIds");

                var existRoleIds = this.MpRoleDomain.GetAll()
                    .Where(x => roleIds.Contains(x.Role.Id))
                    .Select(x => x.Role.Id);

                roleIds.Except(existRoleIds).ForEach(x =>
                {
                    var newMpRole = new MpRole { Role = new Role { Id = x } };
                    this.MpRoleDomain.Save(newMpRole);
                });

                return new BaseDataResult();
        }

        // <inheritdoc />
        public IDataResult GetRoles(BaseParams baseParams)
        {
            return this.MpRoleDomain.GetAll()
                .Select(x => new
                {
                    x.Role.Id,
                    x.Role.Name
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}
