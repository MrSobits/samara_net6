namespace Bars.GkhGji.Regions.BaseChelyabinsk.Map.Reminder
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Reminder;

    public class ChelyabinskReminderMap : JoinedSubClassMap<ChelyabinskReminder>
    {
        public ChelyabinskReminderMap() : base("Bars.GkhGji.Regions.Tomsk.Entities.TomskProtocol", "CHELYABINSK_GJI_REMINDER")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.AppealCitsExecutant, "Проверка ГЖИ").Column("APPEAL_CITS_EXECUTANT_ID");

        }
    }
}