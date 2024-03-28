namespace Bars.GkhGji.Migrations._2017.Version_2017051500
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017051500")]
    [MigrationDependsOn(typeof(Version_2017050400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("GJI_ACTCHECK", new RefColumn("DOCUMENT_PLACE_FIAS_ID", "GJI_ACTCHECK_DOCUMENT_PLACE_FIAS", "B4_FIAS_ADDRESS", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_ACTCHECK", "DOCUMENT_PLACE_FIAS_ID");
        }
    }
}