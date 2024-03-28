namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_20180417000
{
    using B4.Modules.Ecm7.Framework;
    using Gkh;
    using System.Data;

    [Migration("2018041700")]
    [MigrationDependsOn(typeof(Version_2017112200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_PROTOCOL197", new Column("UIN", DbType.String));


        }
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_PROTOCOL197", "UIN");

        }
    }
}