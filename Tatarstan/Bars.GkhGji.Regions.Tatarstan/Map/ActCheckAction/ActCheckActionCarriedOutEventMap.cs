namespace Bars.GkhGji.Regions.Tatarstan.Map.ActCheckAction
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    public class ActCheckActionCarriedOutEventMap : BaseEntityMap<ActCheckActionCarriedOutEvent>
    {
        public ActCheckActionCarriedOutEventMap()
            : base("Выполненное мероприятие действия акта проверки", "GJI_ACTCHECK_ACTION_CARRIED_OUT_EVENT")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.EventType, "Вид мероприятия").Column("EVENT_TYPE");

            this.Reference(x => x.ActCheckAction, "Действие акта проверки").Column("ACTCHECK_ACTION_ID");
        }
    }
}