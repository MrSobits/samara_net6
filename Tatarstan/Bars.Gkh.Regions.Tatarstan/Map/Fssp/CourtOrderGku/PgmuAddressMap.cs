namespace Bars.Gkh.Regions.Tatarstan.Map.Fssp.CourtOrderGku
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;

    public class PgmuAddressMap : BaseEntityMap<PgmuAddress>
    {
        public PgmuAddressMap()
            : base(typeof(PgmuAddress).FullName, "PGMU_ADDRESSES")
        {
            
        }
        
        protected override void Map()
        {
            Property(x => x.ErcCode, "Код расчетного центра").Column("ERC_CODE");
            Property(x => x.PostCode, "Почтовый индекс").Column("POST_CODE");
            Property(x => x.Town, "Город").Column("TOWN");
            Property(x => x.District, "Район").Column("DISTRICT");
            Property(x => x.Street, "Улица").Column("STREET");
            Property(x => x.House, "Дом").Column("HOUSE");
            Property(x => x.Building, "Корпус").Column("BUILDING");
            Property(x => x.Apartment, "Квартира").Column("APARTMENT");
            Property(x => x.Room, "Комната").Column("ROOM");
        }
    }
}