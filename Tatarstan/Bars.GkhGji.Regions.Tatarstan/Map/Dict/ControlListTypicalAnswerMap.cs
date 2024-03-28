namespace Bars.GkhGji.Regions.Tatarstan.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class ControlListTypicalAnswerMap : BaseEntityMap<ControlListTypicalAnswer>
    {
        /// <inheritdoc />
        public ControlListTypicalAnswerMap()
            : base("Bars.GkhGji.Regions.Tatarstan.Entities.Dict.ControlListTypicalAnswer", "GJI_DICT_CONTROL_LIST_TYPICAL_ANSWER")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.TorId, "TorId").Column("TOR_ID");
            this.Property(x => x.Answer, "Answer").Length(500).Column("ANSWER").NotNull();
        }
    }
}
