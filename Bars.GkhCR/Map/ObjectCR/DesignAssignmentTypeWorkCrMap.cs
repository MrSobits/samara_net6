namespace Bars.GkhCr.Map
{
    using Bars.B4.Modules.Mapping.Mappers; using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Маппинг - Связь <see cref="DesignAssignment"/>(Задание на проектирование) и <see cref="TypeWorkCr"/>(Вид работ)
    /// </summary>
    public class DesignAssignmentTypeWorkCrMap : BaseImportableEntityMap<DesignAssignmentTypeWorkCr>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public DesignAssignmentTypeWorkCrMap()
            : base("Связь Задание на проектирование и Вид работ", "CR_OBJ_DESIGN_ASGMNT_TYPE_WORK")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.DesignAssignment, "Задание на проектирование").Column("ASSIGNMENT_ID").NotNull().Fetch();
            this.Reference(x => x.TypeWorkCr, "Вид работ").Column("TYPE_WORK_ID").NotNull().Fetch();
        }
    }
}