namespace Bars.GkhGji.Regions.Tatarstan.Map.ActCheckAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    public class ActCheckActionRemarkMap : BaseEntityMap<ActCheckActionRemark>
    {
        public ActCheckActionRemarkMap()
            : base("Замечание действия акта проверки", "GJI_ACTCHECK_ACTION_REMARK")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Remark, "Замечание").Column("REMARK");
            this.Property(x => x.MemberFio, "ФИО участника").Column("MEMBER_FIO");
            
            this.Reference(x => x.ActCheckAction, "Действие акта проверки").Column("ACTCHECK_ACTION_ID");
        }
    }
}