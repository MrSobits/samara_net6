namespace Bars.GisIntegration.Base.Map.GisRole
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities.GisRole;

    /// <summary>
    /// Маппинг для связи роли ГИС с контрагентом РИС
    /// </summary>
    public class RisContragentRoleMap : BaseEntityMap<RisContragentRole> 
    {
        public RisContragentRoleMap()
            : base("Bars.GisIntegration.Base.Entities.Delegacy.GisRoleContragent", "GIS_ROLE_CONTRAGENT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.GisOperator, "GisOperator").Column("GIS_OPERATOR_ID").NotNull().Fetch();
            this.Reference(x => x.Role, "Role").Column("GIS_ROLE_ID").NotNull().Fetch();
        }
    }
}
