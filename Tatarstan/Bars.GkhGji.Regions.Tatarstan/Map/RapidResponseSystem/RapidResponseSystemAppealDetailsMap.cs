namespace Bars.GkhGji.Regions.Tatarstan.Map.RapidResponseSystem
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;

    public class RapidResponseSystemAppealDetailsMap : BaseEntityMap<RapidResponseSystemAppealDetails>
    {
        /// <inheritdoc />
        public RapidResponseSystemAppealDetailsMap()
            : base(nameof(RapidResponseSystemAppealDetails), "GJI_RAPID_RESPONSE_SYSTEM_APPEAL_DETAILS")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            Property(x => x.ReceiptDate, "Дата поступления").Column("RECEIPT_DATE");
            Property(x => x.ControlPeriod, "Контрольный срок").Column("CONTROL_PERIOD");
            Reference(x => x.State, "Статус обращения").Column("STATE_ID");
            Reference(x => x.User, "Пользователь").Column("USER_ID");
            Reference(x => x.AppealCitsRealityObject, "Место возникновения проблемы").Column("APPEAL_CITS_REALITY_OBJECT_ID");
            Reference(x => x.RapidResponseSystemAppeal, "Обращение в СОПР").Column("RAPID_RESPONSE_SYSTEM_APPEAL_ID").NotNull();
        }
    }
}