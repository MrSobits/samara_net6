namespace Bars.Gkh.Migrations._2016.Version_2016121500
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2016121500")]
    [MigrationDependsOn(typeof(Version_2016121400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            if (!this.Database.ColumnExists("GKH_REALITY_OBJECT", "LATEST_TECH_MONITORING"))
            {
                this.Database.AddColumn("GKH_REALITY_OBJECT", new Column("LATEST_TECH_MONITORING", DbType.DateTime));
            }
            if (!this.Database.ColumnExists("GKH_REALITY_OBJECT", "IS_NOT_INVOLVED_CR_REASON"))
            {
                this.Database.AddColumn("GKH_REALITY_OBJECT", new Column("IS_NOT_INVOLVED_CR_REASON", DbType.Int16, 0));
            }
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "LATEST_TECH_MONITORING");
            this.Database.RemoveColumn("GKH_REALITY_OBJECT", "IS_NOT_INVOLVED_CR_REASON");
        }
    }
}