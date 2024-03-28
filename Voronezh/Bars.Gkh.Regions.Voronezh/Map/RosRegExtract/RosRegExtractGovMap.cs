namespace Bars.Gkh.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using Entities;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Entities.RosregExtractGov"</summary>
    public class RosRegExtractGovMap : PersistentObjectMap<RosRegExtractGov>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public RosRegExtractGovMap()
            :
                base("Bars.Gkh.Regions.Voronezh.Entities", "RosRegExtractGov")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Gov_Code_SP, "Gov_Code_SP").Column("Gov_Code_SP");
            this.Property(x => x.Gov_Content, "Gov_Content").Column("Gov_Content");
            this.Property(x => x.Gov_Name, "Gov_Name").Column("Gov_Name");
            this.Property(x => x.Gov_OKATO_Code, "Gov_OKATO_Code").Column("Gov_OKATO_Code");
            this.Property(x => x.Gov_Country, "Gov_Country").Column("Gov_Country");
            this.Property(x => x.Gov_Address, "Gov_Address").Column("Gov_Address");
        }
    }
    public class RosRegExtractGovNhMaping : ClassMapping<RosRegExtractGov>
    {
        public RosRegExtractGovNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}
