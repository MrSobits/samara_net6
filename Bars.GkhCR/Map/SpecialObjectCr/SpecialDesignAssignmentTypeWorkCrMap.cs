namespace Bars.GkhCr.Map
{
    using Bars.Gkh.Map;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Маппинг - Связь <see cref="SpecialDesignAssignment"/>(Задание на проектирование) и <see cref="SpecialTypeWorkCr"/>(Вид работ)
    /// </summary>
    public class SpecialDesignAssignmentTypeWorkCrMap : BaseImportableEntityMap<SpecialDesignAssignmentTypeWorkCr>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public SpecialDesignAssignmentTypeWorkCrMap()
            : base("Связь Задание на проектирование и Вид работ", "CR_SPECIAL_OBJ_DESIGN_ASGMNT_TYPE_WORK")
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