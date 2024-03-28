namespace Bars.GkhGji.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Маппинг полей сущности <see cref="WarningDocViolationsDetail"/>
    /// </summary>
    public class WarningDocViolationsDetailMap : BaseEntityMap<WarningDocViolationsDetail>
    {
        /// <inheritdoc />
        public WarningDocViolationsDetailMap()
            : base("Связь нарушения требований с нарушением ГЖИ", "GJI_WARNING_DOC_VIOLATIONS_DETAIL")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.WarningDocViolations, "Нарушение требований").Column("WARNING_DOC_VIOL_ID");
            this.Reference(x => x.ViolationGji, "Нарушение").Column("VIOLATION_ID");
        }
    }
}