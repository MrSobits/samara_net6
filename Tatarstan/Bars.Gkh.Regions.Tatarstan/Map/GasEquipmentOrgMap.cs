namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.GasEquipmentOrg;

    public class GasEquipmentOrgMap : BaseEntityMap<GasEquipmentOrg>
    {
        public GasEquipmentOrgMap() :
            base("Мониторинг СМР", "GKH_GAS_EQUIPMENT_ORG")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.Contragent, "Контрагент").Column("CONTRAGENT_ID").NotNull();
            this.Reference(x => x.Contact, "Контакт").Column("CONTACT_ID");
        }
    }
}