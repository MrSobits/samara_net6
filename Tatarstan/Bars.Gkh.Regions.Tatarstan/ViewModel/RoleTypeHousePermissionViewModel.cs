namespace Bars.Gkh.Regions.Tatarstan.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class RoleTypeHousePermissionViewModel : BaseViewModel<RoleTypeHousePermission>
    {
        public override IDataResult List(IDomainService<RoleTypeHousePermission> domain, BaseParams baseParams)
        {
            var roleId = baseParams.Params.GetAs<long>("roleId");

            var allowed = domain.GetAll()
                .Where(x => x.Role.Id == roleId)
                .Select(x => x.TypeHouse)
                .ToList();

            var data = Enum.GetValues(typeof(TypeHouse))
                .Cast<TypeHouse>()
                .Select(x => new { Code = (int)x, Name = x.GetEnumMeta().Display, Allowed = allowed.Contains(x) })
                .ToList();

            return new ListDataResult(data.ToList(), data.Count);
        }
    }
}