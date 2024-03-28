namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021081000
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021081000")]
    [MigrationDependsOn(typeof(Version_2021061800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL197", new Column("CONTROL_TYPE", DbType.Int32, 0));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL197", "CONTROL_TYPE");
        }

    }
}