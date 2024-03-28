using Bars.B4.Modules.Mapping.Mappers;
using Bars.Gkh.InspectorMobile.Entities;

namespace Bars.Gkh.InspectorMobile.Map
{
    public class MpRoleMap : BaseEntityMap<MpRole>
    {
        public MpRoleMap() : base(typeof(MpRole).FullName, "MP_ROLE")
        {
        }

        protected override void Map()
        {
            Reference(x => x.Role, "Роль ЖКХ").Column("ROLE_ID").Fetch();
        }
    }
}