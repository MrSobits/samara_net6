namespace Bars.Gkh.Ris.Migrations.Version_2016051100
{
    using System.Data;

    using B4.Modules.Ecm7.Framework;

    [Migration("2016051100")]
    [MigrationDependsOn(typeof(Version_2016050500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //this.Database.AddColumn("RIS_ACCOUNT", new Column("BEGINDATE", DbType.DateTime));
            //this.Database.AddColumn("RIS_ACCOUNT", new Column("IS_RENTER", DbType.Boolean));
        }

        public override void Down()
        {
            //this.Database.RemoveColumn("RIS_ACCOUNT", "BEGINDATE");
            //this.Database.RemoveColumn("RIS_ACCOUNT", "IS_RENTER");
        }
    }
}