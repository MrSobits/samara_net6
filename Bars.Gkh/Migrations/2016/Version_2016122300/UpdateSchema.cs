namespace Bars.Gkh.Migrations._2016.Version_2016122300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2016122300")]
    [MigrationDependsOn(typeof(Version_2016121600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            if (!this.Database.ColumnExists("GKH_CONTRAGENT", "FRGU_REG_NUMBER"))
            {
                this.Database.AddColumn("GKH_CONTRAGENT", new Column("FRGU_REG_NUMBER", DbType.String, 36));
            }
            if (!this.Database.ColumnExists("GKH_CONTRAGENT", "FRGU_ORG_NUMBER"))
            {
                this.Database.AddColumn("GKH_CONTRAGENT", new Column("FRGU_ORG_NUMBER", DbType.String, 36));
            }
            if (!this.Database.ColumnExists("GKH_CONTRAGENT", "FRGU_SERVICE_NUMBER"))
            {
                this.Database.AddColumn("GKH_CONTRAGENT", new Column("FRGU_SERVICE_NUMBER", DbType.String, 36));
            }
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_CONTRAGENT", "FRGU_REG_NUMBER");
            this.Database.RemoveColumn("GKH_CONTRAGENT", "FRGU_ORG_NUMBER");
            this.Database.RemoveColumn("GKH_CONTRAGENT", "FRGU_SERVICE_NUMBER");
        }
    }
}