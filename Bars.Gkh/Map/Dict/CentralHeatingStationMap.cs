namespace Bars.Gkh.Map.Dict
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Маппинг для справочника ЦТП
    /// </summary>
    public class CentralHeatingStationMap : BaseImportableEntityMap<CentralHeatingStation>
    {
        public CentralHeatingStationMap()
            :
            base("Справочник ЦТП", "GKH_DICT_CENTRAL_HEATING_STATION")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Name, "Наименование").Column("NAME").Length(300).NotNull();
            this.Property(x => x.Abbreviation, "Аббревиатура").Column("ABBREVIATION").Length(50);
            this.Reference(x => x.Address, "Адрес").Column("ADDRESS_ID").Fetch();
        }
    }
}