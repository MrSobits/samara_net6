namespace Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014061900
{
    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2014061900")]
    [MigrationDependsOn(typeof(Version_2014061100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            //устаревшая сущность удалена
        }

        public override void Down()
        {
        }
    }
}