namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2018051100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018051100")]
    [MigrationDependsOn(typeof(Version_2018050100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_DPKR_ACTUAL_CRITERIAS", new RefColumn("KESTATE_ID", ColumnProperty.Null, "OVRHL_SUBPROGRAM_CRITERIAS_B4_STATE_KESTATE_ID_ID", "B4_STATE", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_DPKR_ACTUAL_CRITERIAS", "KESTATE_ID");
        }
    }
}