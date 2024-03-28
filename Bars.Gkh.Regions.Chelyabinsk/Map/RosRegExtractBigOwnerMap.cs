namespace Bars.Gkh.Regions.Chelyabinsk.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using Entities;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Entities.RosRegExtractBig"</summary>
    public class RosRegExtractBigOwnerMap : PersistentObjectMap<RosRegExtractBigOwner>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public RosRegExtractBigOwnerMap()
            :
                base("Bars.Gkh.Regions.Chelyabinsk.Entities", "RosRegExtractBigOwner")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.ExtractId, "ExtractId").Column("ExtractId");
            this.Property(x => x.OwnerName, "OwnerName").Column("OwnerName");
            this.Property(x => x.AreaShareNum, "AreaShareNum").Column("AreaShareNum");
            this.Property(x => x.AreaShareDen, "AreaShareDen").Column("AreaShareDen");
            this.Property(x => x.RightNumber, "RightNumber").Column("RightNumber");
        }
    }
    public class RosRegExtractBigOwnerNhMaping : ClassMapping<RosRegExtractBigOwner>
    {
        public RosRegExtractBigOwnerNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}
