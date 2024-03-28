namespace Bars.Gkh.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using Entities;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Entities.RosRegExtractRight"</summary>
    public class RosRegExtractRightMap : PersistentObjectMap<RosRegExtractRight>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public RosRegExtractRightMap()
            :
                base("Bars.Gkh.Regions.Voronezh.Entities", "RosRegExtractRight")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            //this.Reference(x => x.gov_id, "gov_id").Column("gov_id");
            //this.Reference(x => x.org_id, "org_id").Column("org_id");
            //this.Reference(x => x.pers_id, "pers_id").Column("pers_id");
            this.Reference(x => x.reg_id, "reg_id").Column("reg_id").Fetch();
            this.Reference(x => x.owner_id, "owner_id").Column("owner_id").Fetch();
            this.Property(x => x.RightNumber, "rightnumber").Column("rightnumber");
            //this.Property(x => x.Reg_ID_Record, "Reg_ID_Record").Column("Reg_ID_Record");
            //this.Property(x => x.Reg_RegNumber, "Reg_RegNumber").Column("Reg_RegNumber");
            //this.Property(x => x.Reg_Type, "Reg_Type").Column("Reg_Type");
            //this.Property(x => x.Reg_Name, "Reg_Name").Column("Reg_Name");
            //this.Property(x => x.Reg_RegDate, "Reg_RegDate").Column("Reg_RegDate");
            //this.Property(x => x.Reg_ShareText, "Reg_ShareText").Column("Reg_ShareText");
        }
    }
    public class RosRegExtractRightNhMaping : ClassMapping<RosRegExtractRight>
    {
        public RosRegExtractRightNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}
