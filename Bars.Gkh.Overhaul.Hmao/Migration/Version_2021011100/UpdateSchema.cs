namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2021011100
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021011100")]
    [MigrationDependsOn(typeof(Version_2020101400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("OVRHL_PROP_OWN_PROTOCOLS_INSPECTOR",
                new RefColumn("PROTOCOL_ID", ColumnProperty.NotNull, "OVRHL_PROP_OWN_PROTOCOLS_INSP_PID", "OVRHL_PROP_OWN_PROTOCOLS", "ID"),
                new RefColumn("INSPECTOR_ID", ColumnProperty.NotNull, "OVRHL_PROP_OWN_PROTOCOLS_INSP_IID", "GKH_DICT_INSPECTOR", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("OVRHL_PROP_OWN_PROTOCOLS_INSPECTOR");
        }
    }
}