namespace Bars.Gkh.Regions.Tatarstan.Map.Fssp.CourtOrderGku
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities.Fssp.CourtOrderGku;

    public class FsspAddressMap : BaseEntityMap<FsspAddress>
    {
        public FsspAddressMap()
            : base(typeof(FsspAddress).FullName, "FSSP_ADDRESS")
        {
            
        }
        
        protected override void Map()
        {
            Property(x => x.Address, "Адрес").Column("ADDRESS");
            Reference(x => x.PgmuAddress, "Адрес ПГМУ").Column("PGMU_ADDRESS_ID");
        }
    }
}