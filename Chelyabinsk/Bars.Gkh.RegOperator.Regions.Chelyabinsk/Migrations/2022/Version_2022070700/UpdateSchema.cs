using Bars.B4.Modules.Ecm7.Framework;

using System.Data;

namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk._2022.Version_2022070700
{
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022070700")]
    [MigrationDependsOn(typeof(_2020.Version_2020092900.UpdateSchema))]
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
       
        public override void Up()
        {
            Database.AddEntityTable("AGENT_PIR",
                   new Column("AP_DATE_START", DbType.DateTime),
                   new Column("AP_DATE_END", DbType.DateTime),
                   new Column("AP_CONTRACT_DATE", DbType.DateTime),
                   new Column("AP_CONTRACT_NUMBER", DbType.Int32),
                   new RefColumn("AP_CONTRAGENT_ID", "AGENT_PIR_GKH_CONTRAGENT", "GKH_CONTRAGENT", "ID"),
                   new RefColumn("AP_FILE_INFO_ID", "AGENT_PIR_FILE_INFO", "B4_FILE_INFO", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("AGENT_PIR");            
        }
    }
}