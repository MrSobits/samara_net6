namespace Bars.Gkh.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using Entities;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Entities.RosRegExtractOwner"</summary>
    public class RosRegExtractOwnerMap : PersistentObjectMap<RosRegExtractOwner>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public RosRegExtractOwnerMap()
            :
                base("Bars.Gkh.Regions.Voronezh.Entities", "RosRegExtractOwner")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.gov_id, "gov_id").Column("gov_id").Fetch();
            this.Reference(x => x.org_id, "org_id").Column("org_id").Fetch();
            this.Reference(x => x.pers_id, "pers_id").Column("pers_id").Fetch();

        }
    }
    public class RosRegExtractOwnerNhMaping : ClassMapping<RosRegExtractOwner>
    {
        public RosRegExtractOwnerNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}
