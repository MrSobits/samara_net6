namespace Bars.Gkh.Migrations._2017.Version_2017111400
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2017111400")]
    [MigrationDependsOn(typeof(Version_2017102600.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            if (!this.Database.ColumnExists("GKH_CONTRAGENT", "TIMEZONE_TYPE"))
            {
                this.Database.AddColumn("GKH_CONTRAGENT", new Column("TIMEZONE_TYPE", DbType.Int16));
            }
            if (!this.Database.ColumnExists("GKH_CONTRAGENT", "OKOGU"))
            {
                this.Database.AddColumn("GKH_CONTRAGENT", new Column("OKOGU", DbType.Int16));
            }
            if (!this.Database.ColumnExists("GKH_CONTRAGENT", "OKFS"))
            {
                this.Database.AddColumn("GKH_CONTRAGENT", new Column("OKFS", DbType.Int16));
            }
        }

        public override void Down()
        {
        }
    }
}