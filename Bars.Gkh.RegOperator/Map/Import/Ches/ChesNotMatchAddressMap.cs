namespace Bars.Gkh.RegOperator.Map.Import
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;

    using NHibernate.Mapping.ByCode.Conformist;

    public class ChesNotMatchAddressMap : PersistentObjectMap<ChesNotMatchAddress>
    {
        /// <inheritdoc />
        public ChesNotMatchAddressMap()
            : base("Несопоставленный в периоде адрес", "CHES_NOT_MATCH_ADDRESS")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.ExternalAddress, "Адрес ЧЭС").Column("EXTERNAL_ADDRESS").NotNull();
            this.Property(x => x.HouseGuid, "Гуид дома в ФИАС").Column("HOUSE_GUID");
            this.Reference(x => x.Period, "Период").Column("PERIOD_ID");
        }
    }

    public class ChesNotMatchAddressNhMaping : ClassMapping<ChesNotMatchAddress>
    {
        public ChesNotMatchAddressNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}