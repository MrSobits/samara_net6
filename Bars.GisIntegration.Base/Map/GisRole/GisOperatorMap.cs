namespace Bars.GisIntegration.Base.Map.GisRole
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GisIntegration.Base.Entities.GisRole;

    /// <summary>
    /// Маппинг для связи роли ГИС с контрагентом РИС
    /// </summary>
    public class GisOperatorMap : BaseEntityMap<GisOperator> 
    {
        public GisOperatorMap()
            : base("Bars.GisIntegration.Base.Entities.GisRole.GisRoleContragent", "GIS_OPERATOR")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Contragent, "Contragent").Column("RIS_CONTRAGENT_ID").NotNull().Fetch();
        }
    }
}
