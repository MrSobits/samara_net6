namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.GasEquipmentOrg;

    public class GasEquipmentOrgRealityObjMap : BaseEntityMap<GasEquipmentOrgRealityObj>
    {
        public GasEquipmentOrgRealityObjMap() :
            base("Мониторинг СМР", "GKH_GAS_EQUIP_ORG_CONTRACT")
        {
        }

        protected override void Map()
        {
            this.Reference(x => x.GasEquipmentOrg, "ВДГО").Column("GAS_EQUIPMENT_ORG_ID").NotNull();
            this.Reference(x => x.RealityObject, "Жилой дом").Column("REALITY_OBJECT_ID").NotNull();
            this.Property(x => x.StartDate, "Дата начала предоставления услуги").Column("START_DATE").NotNull();
            this.Property(x => x.EndDate, "Дата окончания предоставления услуги").Column("END_DATE").NotNull();
            this.Property(x => x.DocumentNum, "Номер документа").Column("DOCUMENT_NUM").Length(255);
            this.Reference(x => x.File, "Файл").Column("FILE_ID");
        }
    }
}



       
    