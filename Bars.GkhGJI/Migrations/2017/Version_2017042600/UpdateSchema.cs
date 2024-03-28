namespace Bars.GkhGji.Migrations._2017.Version_2017042600
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2017042600")]
    [MigrationDependsOn(typeof(Version_2017042400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddRefColumn("GJI_INSPECTION",
                new RefColumn("INSPECTION_TYPE_BASE_ID",
                    "GJI_INSPECTION_INSPECTION_TYPE_BASE",
                    "GJI_DICT_INSPECTION_BASE_TYPE",
                    "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_INSPECTION", "INSPECTION_TYPE_BASE_ID");
        }
    }
}