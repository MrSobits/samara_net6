namespace Bars.Gkh.Regions.Voronezh.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Entities;
    using Entities;
    using NHibernate.Mapping.ByCode.Conformist;

    /// <summary>Маппинг для "Bars.Gkh.Entities.RosRegExtractReg"</summary>
    public class RosRegExtractRegMap : PersistentObjectMap<RosRegExtractReg>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public RosRegExtractRegMap()
            :
                base("Bars.Gkh.Regions.Voronezh.Entities", "RosRegExtractReg")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Reg_ID_Record, "Reg_ID_Record").Column("Reg_ID_Record");
            this.Property(x => x.Reg_RegNumber, "Reg_RegNumber").Column("Reg_RegNumber");
            this.Property(x => x.Reg_Type, "Reg_Type").Column("Reg_Type");
            this.Property(x => x.Reg_Name, "Reg_Name").Column("Reg_Name");
            this.Property(x => x.Reg_RegDate, "Reg_RegDate").Column("Reg_RegDate");
            this.Property(x => x.Reg_ShareText, "Reg_ShareText").Column("Reg_ShareText");
        }
    }
    public class RosRegExtractRegNhMaping : ClassMapping<RosRegExtractReg>
    {
        public RosRegExtractRegNhMaping()
        {
            this.Schema("IMPORT");
        }
    }
}
