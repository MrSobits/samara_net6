namespace Bars.Gkh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;

    public class AddressMatchMap : BaseEntityMap<AddressMatch>
    {
        /// <inheritdoc />
        public AddressMatchMap()
            : base("Сущность сопоставления внешнего адреса и адреса ЖКХ.Комплекс", "GKH_ADDRESS_MATCH")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.ExternalAddress, "Внешний адрес").Column("EXTERNAL_ADDRESS").NotNull();
            this.Property(x => x.HouseGuid, "Гуид дома").Column("HOUSE_GUID");

            this.Reference(x => x.RealityObject, "Дом в ЖКХ.Комплекс").Column("RO_ID").NotNull();
        }
    }
}