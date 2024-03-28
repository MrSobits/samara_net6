namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    /// <summary>
    /// Маппинг полей сущности <see cref="ControlTypeInspectorPositions"/>
    /// </summary>
    public class ControlTypeInspectorPositionsMap : BaseEntityMap<ControlTypeInspectorPositions>
    {
        public ControlTypeInspectorPositionsMap()
            : base("Bars.GkhGji.Regions.Tatarstan.Entities.Dict.ControlTypeInspectorPositions", "GJI_DICT_CONTROL_TYPE_INSPECTOR_POSITIONS")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.ControlType, "ControlType").Column("CONTROL_TYPE_ID").NotNull();
            this.Reference(x => x.InspectorPosition, "InspectorPosition").Column("INSPECTOR_POS_ID");
            this.Property(x => x.IsIssuer, "IsIssuer").Column("IS_ISSUER");
            this.Property(x => x.IsMember, "IsMember").Column("IS_MEMBER");
            this.Property(x => x.ErvkId, "ErvkId").Column("ERVK_ID").Length(36);
        }
    }
}