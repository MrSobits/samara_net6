namespace Bars.GkhGji.Regions.Habarovsk.Migrations.Version_2022060600
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022060600")]
    [MigrationDependsOn(typeof(Version_2022053100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_CH_SMEV_MVD", new RefColumn("PERSON_ID", ColumnProperty.None, "GJI_CH_SMEV_MVD_PERSON_ID", "GKH_PERSON", "ID"));
        }

        public override void Down()
        {
            Database.RemoveColumn("GJI_CH_SMEV_MVD", "PERSON_ID");
        }
    }
}