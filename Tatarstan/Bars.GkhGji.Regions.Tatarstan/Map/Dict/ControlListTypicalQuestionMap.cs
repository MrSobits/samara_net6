namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class ControlListTypicalQuestionMap : BaseEntityMap<ControlListTypicalQuestion>
    {
        /// <inheritdoc />
        public ControlListTypicalQuestionMap()
            : base("Bars.GkhGji.Regions.Tatarstan.Entities.Dict.ControlListTypicalQuestion", "GJI_DICT_CONTROL_LIST_TYPICAL_QUESTION")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Question, "Question").Length(500).Column("QUESTION").NotNull();
            this.Reference(x => x.NormativeDoc, "NormativeDoc").Column("NORMATIVE_DOC_ID").Fetch();
            this.Reference(x => x.MandatoryRequirement, "MandatoryRequirement").Column("MANDATORY_REQ_ID").Fetch();
        }
    }
}
