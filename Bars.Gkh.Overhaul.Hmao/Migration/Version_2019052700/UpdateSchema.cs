namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2019052700
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2019052700")]
    [MigrationDependsOn(typeof(Version_2018111900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddRefColumn("OVRHL_PRG_VERSION", new RefColumn("STATE_ID", "OVRHL_PRG_VERSION_STATE", "B4_STATE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_PRG_VERSION", "STATE_ID");
        }
    }
}