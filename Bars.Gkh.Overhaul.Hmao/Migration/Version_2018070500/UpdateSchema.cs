namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2018070500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2018070500")]
    [MigrationDependsOn(typeof(Version_2018070400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("OVRHL_KPKR_COST_LIMITS", new RefColumn("MUNICIPALITY_ID", ColumnProperty.Null, "OVRHL_KPKR_COST_LIMITS_GKH_DICT_MUNICIPALITY_MUNICIPALITY_ID_ID", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("OVRHL_KPKR_COST_LIMITS", "MUNICIPALITY_ID");
        }
    }
}