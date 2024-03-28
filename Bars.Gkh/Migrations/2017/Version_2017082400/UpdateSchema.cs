namespace Bars.Gkh.Migrations._2017.Version_2017082400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017082400")]
    [MigrationDependsOn(typeof(Version_2017082200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GKH_CONTRAGENT", new Column("TIMEZONE_TYPE", DbType.Int16));
            this.Database.AddColumn("GKH_CONTRAGENT", new Column("OKOGU", DbType.Int16));
            this.Database.AddColumn("GKH_CONTRAGENT", new Column("OKFS", DbType.Int16));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GKH_CONTRAGENT", "TIMEZONE_TYPE");
            this.Database.RemoveColumn("GKH_CONTRAGENT", "OKOGU");
            this.Database.RemoveColumn("GKH_CONTRAGENT", "OKFS");
        }
    }
}