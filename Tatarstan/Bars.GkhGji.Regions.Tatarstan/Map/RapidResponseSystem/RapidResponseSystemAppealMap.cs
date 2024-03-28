namespace Bars.GkhGji.Regions.Tatarstan.Map.RapidResponseSystem
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;

    public class RapidResponseSystemAppealMap : BaseEntityMap<RapidResponseSystemAppeal>
    {
        /// <inheritdoc />
        public RapidResponseSystemAppealMap()
            : base(nameof(RapidResponseSystemAppeal), "GJI_RAPID_RESPONSE_SYSTEM_APPEAL")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            Reference(x => x.AppealCits, "Обращение гражданина").Column("APPEAL_CITS_ID").NotNull();
            Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID");
        }
    }
}