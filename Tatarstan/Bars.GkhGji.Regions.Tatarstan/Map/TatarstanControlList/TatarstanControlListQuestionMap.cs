namespace Bars.GkhGji.Regions.Tatarstan.Map.ControlList
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanControlList;

    public class TatarstanControlListQuestionMap : BaseEntityMap<TatarstanControlListQuestion>
    {
        /// <inheritdoc />
        public TatarstanControlListQuestionMap()
            : base("Bars.GkhGji.Regions.Tatarstan.Entities.ControlList.TatarstanControlListQuestion", "GJI_CONTROL_LIST_QUESTION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.QuestionContent, "QuestionContent").Length(500).Column("QUESTION_CONTENT").NotNull();
            this.Reference(x => x.TypicalQuestion, "TypicalQuestion").Column("TYPICAL_QUESTION_ID").Fetch();
            this.Reference(x => x.TypicalAnswer, "TypicalAnswer").Column("TYPICAL_ANSWER_ID").Fetch();
            this.Reference(x => x.ControlList, "ControlList").Column("CONTROL_LIST_ID").Fetch();
            this.Property(x => x.ErpGuid, "ErpGuid").Column("ERP_GUID");
        }
    }
}
