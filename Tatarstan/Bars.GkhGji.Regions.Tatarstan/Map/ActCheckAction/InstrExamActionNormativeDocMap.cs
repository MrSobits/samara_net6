namespace Bars.GkhGji.Regions.Tatarstan.Map.ActCheckAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction.InstrExamAction;

    public class InstrExamActionNormativeDocMap : BaseEntityMap<InstrExamActionNormativeDoc>
    {
        public InstrExamActionNormativeDocMap()
            : base("Нормативно-правовой акт действия \"Инструментальное обследование\"", "GJI_ACTCHECK_INSTR_EXAM_ACTION_NORMATIVE_DOC")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.InstrExamAction, "Действие \"Инструментальное обследование\"").Column("INSTR_EXAM_ACTION_ID");
            this.Reference(x => x.NormativeDoc, "Нормативно-правовой акт").Column("NORMATIVE_DOC_ID");
        }
    }
}