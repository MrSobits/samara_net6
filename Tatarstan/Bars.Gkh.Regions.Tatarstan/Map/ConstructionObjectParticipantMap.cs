namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class ConstructionObjectParticipantMap : BaseEntityMap<ConstructionObjectParticipant>
    {
        public ConstructionObjectParticipantMap() :
            base("Участник объекта строительства", "GKH_CONSTRUCT_OBJ_PARTICIPANT")
        {      
        }

        protected override void Map()
        {
			this.Property(x => x.ParticipantType, "Тип участника строительства").Column("PARTICIPANT_TYPE");
			this.Property(x => x.CustomerType, "Тип заказчика").Column("CUSTOMER_TYPE");
			this.Property(x => x.Description, "Дополнительная информация").Column("DESCRIPTION");

			this.Reference(x => x.ConstructionObject, "Объект строительства").Column("OBJECT_ID").NotNull().Fetch();
			this.Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").Fetch();
        }
    }
}